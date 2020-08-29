using JBToolkit.Imaging;
using JBToolkit.Network;
using JBToolkit.Threads;
using JBToolkit.Windows;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YTMusicUploader.Providers;
using YTMusicUploader.Providers.DataModels;
using YTMusicUploader.Providers.RequestModels;
using static YTMusicUploader.Business.MusicDataFetcher;

namespace YTMusicUploader.Business
{
    /// <summary>
    /// Responsive for managing uploading music files to YouTube Music.
    /// </summary>
    public class FileUploader
    {
        private MainForm MainForm;
        private List<MusicFile> MusicFiles;
        public bool Stopped { get; set; } = true;
        private Thread _artworkFetchThread = null;

        private int _errorsCount;
        private int _uploadedCount;
        private int _discoveredFilesCount;
        public ArtistCache ArtistCache { get; set; }

        public FileUploader(MainForm mainForm)
        {
            MainForm = mainForm;
        }

        /// <summary>
        /// Execute the upload process
        /// </summary>
        public async Task Process()
        {
            _errorsCount = await MainForm.MusicFileRepo.CountIssues();
            _uploadedCount = await MainForm.MusicFileRepo.CountUploaded();
            _discoveredFilesCount = await MainForm.MusicFileRepo.CountAll();

            MusicFiles = MainForm.MusicFileRepo.LoadAll(true, true, true).Result;

            MainForm.SetStatusMessage("Gathering uploaded artists from YouTube Music...", "Gathering uploaded artists from YT Music");
            Requests.LoadArtistCache(MainForm.Settings.AuthenticationCookie);

            if (Global.MultiThreadedRequests)
            {
                Requests.StartPrefetchingUploadedFilesCheck(
                                MusicFiles,
                                MainForm.Settings.AuthenticationCookie,
                                MainForm.MusicDataFetcher);
            }

            foreach (var musicFile in MusicFiles)
            {
                if (File.Exists(musicFile.Path))
                {
                    if (Requests.ArtistCache.LastSetTime < DateTime.Now.AddHours(-2))
                    {
                        MainForm.SetStatusMessage("Gathering uploaded artists from YouTube Music...", "Gathering uploaded artists from YT Music");
                        Requests.LoadArtistCache(MainForm.Settings.AuthenticationCookie);
                    }

                    musicFile.Hash = await DirectoryHelper.GetFileHash(musicFile.Path);

                    if (DoWeHaveAMusicFileWithTheSameHash(musicFile, out MusicFile existingMusicFile))
                    {
                        if (ThreadIsAborting())
                            return;

                        HandleFileRenamedOrMove(musicFile, existingMusicFile).Wait();
                    }
                    else
                    {
                        Stopped = false;
                        while (!NetworkHelper.InternetConnectionIsUp())
                        {
                            if (ThreadIsAborting())
                                return;

                            ThreadHelper.SafeSleep(1000);
                        }

                        try
                        {
                            TryProcess(musicFile).Wait();
                        }
                        catch (Exception e)
                        {
                            for (int i = 0; i < 5; i++)
                            {
                                Thread.Sleep(1000);
                                try
                                {
                                    TryProcess(musicFile).Wait();
                                    break;
                                }
                                catch { }
                            }

                            var _ = e;
#if DEBUG
                            Console.Out.WriteLine("FileUploader: Process: " + e.Message);
#endif
                            HandleFileNeedsUploading(musicFile).Wait();
                        }

                        await musicFile.Save();
                    }
                }
                else
                {
                    _discoveredFilesCount--;
                    MainForm.SetDiscoveredFilesLabel(_discoveredFilesCount.ToString());
                    await musicFile.Delete();
                }
            }

            await SetUploadDetails("Idle", null, true, false);
            Stopped = true;
        }

        private async Task HandleFileRenamedOrMove(MusicFile musicFile, MusicFile existingMusicFile)
        {
            // TODO: If there are mulitple exact files in the library (i.e. same album in 2 different folders?)
            // this method section will keep running every time on startup as the hash will be the same and it thinks
            // the files have been moved... Need to consider a how to handle this.

            await SetUploadDetails("Already Present: " + DirectoryHelper.EllipsisPath(musicFile.Path, 210), musicFile.Path, false, false);
            await SetUploadDetails("Already Present: " + DirectoryHelper.EllipsisPath(musicFile.Path, 210), musicFile.Path, true, false);
            MainForm.SetStatusMessage("Comparing file system against database for existing uploads", "Comparing file system against the DB");

            existingMusicFile.Path = musicFile.Path;
            existingMusicFile.LastUpload = DateTime.Now;
            existingMusicFile.Removed = false;

            TrackAndReleaseMbId trackAndReleaseMbId = null;
            if (string.IsNullOrEmpty(musicFile.MbId) || string.IsNullOrEmpty(musicFile.ReleaseMbId))
                trackAndReleaseMbId = MainForm.MusicDataFetcher.GetTrackAndReleaseMbId(musicFile.Path, false);

            if (string.IsNullOrEmpty(existingMusicFile.MbId))
                existingMusicFile.MbId = string.IsNullOrEmpty(musicFile.MbId)
                                                  ? string.IsNullOrEmpty(trackAndReleaseMbId.MbId)
                                                                ? existingMusicFile.MbId
                                                                : trackAndReleaseMbId.MbId
                                                  : musicFile.MbId;

            if (string.IsNullOrEmpty(existingMusicFile.ReleaseMbId))
                existingMusicFile.ReleaseMbId = string.IsNullOrEmpty(musicFile.ReleaseMbId)
                                                  ? string.IsNullOrEmpty(trackAndReleaseMbId.ReleaseMbId)
                                                                ? existingMusicFile.ReleaseMbId
                                                                : trackAndReleaseMbId.ReleaseMbId
                                                  : musicFile.MbId;

            _uploadedCount++;
            MainForm.SetUploadedLabel(_uploadedCount.ToString());

            musicFile.Delete(true).Wait();
            await existingMusicFile.Save();
        }

        private async Task TryProcess(MusicFile musicFile)
        {
            var alreadyUploaded = Requests.IsSongUploaded(musicFile.Path,
                                                          MainForm.Settings.AuthenticationCookie,
                                                          out string entityId,
                                                          MainForm.MusicDataFetcher);

            if (alreadyUploaded != Requests.UploadCheckResult.NotPresent)
            {
                if (ThreadIsAborting())
                    return;

                await HandleFileAlreadyUploaded(
                            musicFile,
                            entityId);
            }
            else
            {
                if (ThreadIsAborting())
                    return;

                await HandleFileNeedsUploading(musicFile);
            }
        }

        private async Task HandleFileAlreadyUploaded(MusicFile musicFile, string entityId)
        {
            await SetUploadDetails("Already Present: " + DirectoryHelper.EllipsisPath(musicFile.Path, 210), musicFile.Path, false, false);
            await SetUploadDetails("Already Present: " + DirectoryHelper.EllipsisPath(musicFile.Path, 210), musicFile.Path, true, false);
            MainForm.SetStatusMessage("Comparing and updating database with existing uploads", "Comparing files with YouTube Music");

            TrackAndReleaseMbId trackAndReleaseMbId = null;
            if (string.IsNullOrEmpty(musicFile.MbId) || string.IsNullOrEmpty(musicFile.ReleaseMbId))
                trackAndReleaseMbId = MainForm.MusicDataFetcher.GetTrackAndReleaseMbId(musicFile.Path, false);

            musicFile.LastUpload = DateTime.Now;
            musicFile.Error = false;
            musicFile.EntityId = entityId;
            musicFile.MbId = !string.IsNullOrEmpty(musicFile.MbId)
                                        ? musicFile.MbId
                                        : (!Requests.UploadCheckCache.CachedObjectHash.Contains(musicFile.Path)
                                                ? trackAndReleaseMbId.MbId
                                                : Requests.UploadCheckCache.CachedObjects.Where(m => m.MusicFilePath == musicFile.Path)
                                                                                         .FirstOrDefault()
                                                                                         .MbId);

            musicFile.ReleaseMbId = !string.IsNullOrEmpty(musicFile.ReleaseMbId)
                                                ? musicFile.ReleaseMbId
                                                : (!Requests.UploadCheckCache.CachedObjectHash.Contains(musicFile.Path)
                                                        ? trackAndReleaseMbId.ReleaseMbId
                                                        : Requests.UploadCheckCache.CachedObjects.Where(m => m.MusicFilePath == musicFile.Path)
                                                                                                 .FirstOrDefault()
                                                                                                 .ReleaseMbId);

            _uploadedCount++;
            MainForm.SetUploadedLabel(_uploadedCount.ToString());
        }

        private async Task HandleFileNeedsUploading(MusicFile musicFile)
        {
            await SetUploadDetails(DirectoryHelper.EllipsisPath(musicFile.Path, 210), musicFile.Path, false, false);
            await SetUploadDetails(DirectoryHelper.EllipsisPath(musicFile.Path, 210), musicFile.Path, true, true); // Peform MusicBrainz lookup if required

            Requests.UploadTrack(
                        MainForm,
                        MainForm.Settings.AuthenticationCookie,
                        musicFile.Path,
                        MainForm.Settings.ThrottleSpeed,
                        out string error);

            if (!string.IsNullOrEmpty(error))
            {
                musicFile.Error = true;
                musicFile.ErrorReason = error;

                _errorsCount++;
                MainForm.SetIssuesLabel(_errorsCount.ToString());
            }
            else
            {
                TrackAndReleaseMbId trackAndReleaseMbId = null;
                if (string.IsNullOrEmpty(musicFile.MbId) || string.IsNullOrEmpty(musicFile.ReleaseMbId))
                    trackAndReleaseMbId = MainForm.MusicDataFetcher.GetTrackAndReleaseMbId(musicFile.Path, true);

                musicFile.LastUpload = DateTime.Now;
                musicFile.Error = false;
                musicFile.MbId = !string.IsNullOrEmpty(musicFile.MbId)
                                            ? musicFile.MbId
                                            : (!Requests.UploadCheckCache.CachedObjectHash.Contains(musicFile.Path)
                                                    ? trackAndReleaseMbId.MbId
                                                    : Requests.UploadCheckCache.CachedObjects.Where(m => m.MusicFilePath == musicFile.Path)
                                                                                             .FirstOrDefault()
                                                                                             .MbId);

                musicFile.ReleaseMbId = !string.IsNullOrEmpty(musicFile.ReleaseMbId)
                                                    ? musicFile.ReleaseMbId
                                                    : (!Requests.UploadCheckCache.CachedObjectHash.Contains(musicFile.Path)
                                                            ? trackAndReleaseMbId.ReleaseMbId
                                                            : Requests.UploadCheckCache.CachedObjects.Where(m => m.MusicFilePath == musicFile.Path)
                                                                                                     .FirstOrDefault()
                                                                                                     .ReleaseMbId);

                // We've uploaded it, so now see if we can get the YouTube Music entityId
                if (Requests.IsSongUploaded(musicFile.Path,
                                            MainForm.Settings.AuthenticationCookie,
                                            out string entityId,
                                            MainForm.MusicDataFetcher,
                                            false) != Requests.UploadCheckResult.NotPresent)
                {
                    musicFile.EntityId = entityId;
                }

                _uploadedCount++;
                MainForm.SetUploadedLabel(_uploadedCount.ToString());
            }

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        private async Task SetUploadDetails(
            string message,
            string musicPath,
            bool setArtworkImage,
            bool useMusicBrainzFallback)
        {
            string tooltipText = string.Empty;
            if (string.IsNullOrEmpty(musicPath))
                tooltipText = "Idle";

            else
                await MainForm.MusicDataFetcher.GetMusicFileMetaDataString(musicPath);

            if (!setArtworkImage)
            {
                // just set the text

                MainForm.SetUploadingMessage(
                           message,
                           tooltipText,
                           null,
                           false);

                await Task.Run(() => { });
            }
            else
            {
                if (!useMusicBrainzFallback)
                {
                    if (_artworkFetchThread != null)
                    {
                        try
                        {
                            _artworkFetchThread.Abort();
                        }
                        catch
                        { }
                    }

                    if (string.IsNullOrEmpty(musicPath))
                    {
                        MainForm.SetUploadingMessage(
                                    message,
                                    tooltipText,
                                    null,
                                    true);
                    }
                    else
                    {
                        var image = await MainForm.MusicDataFetcher.GetAlbumArtwork(musicPath, false);
                        if (image != null && MainForm.ArtworkImage != null && ImageHelper.IsSameImage(image, MainForm.ArtworkImage))
                        {
                            MainForm.SetUploadingMessage(
                                        message,
                                        tooltipText,
                                        null,
                                        false);
                        }
                        else
                        {
                            MainForm.SetUploadingMessage(
                                        message,
                                        tooltipText,
                                        image,
                                        true);
                        }
                    }
                }
                else
                {
                    // Only used for when 'uploading' a track - Will attempt to get info for MusicBrainz (i.e. cover image)
                    SetCoverArtImageThreaded(message, tooltipText, musicPath);
                }

                await Task.Run(() => { });
            }
        }

        private void SetCoverArtImageThreaded(string message, string tooltipText, string musicPath)
        {
            // Thread to set the cover artwork image, because this can take some unwanted time when
            // checking lots of music files to see if they're already uploaded to YouTube Music
            // and we want to cancel the thread if it's still running on the next track check
            // and just display the default cover artwork

            if (_artworkFetchThread != null)
            {
                try
                {
                    _artworkFetchThread.Abort();
                }
                catch
                { }

                if (MainForm.ArtworkImage != null &&
                    ImageHelper.IsSameImage(MainForm.ArtworkImage,
                                            Properties.Resources.default_artwork))
                {
                    MainForm.SetUploadingMessage(
                                  message,
                                  tooltipText,
                                  null,
                                  false);
                }
                else
                {
                    if (string.IsNullOrEmpty(musicPath))
                    {
                        MainForm.SetUploadingMessage(
                                    message,
                                    tooltipText,
                                    null,
                                    true);
                    }
                    else
                    {
                        MainForm.SetUploadingMessage(
                                    message,
                                    tooltipText,
                                    Properties.Resources.default_artwork,
                                    true);
                    }
                }
            }

            if (!string.IsNullOrEmpty(musicPath))
            {
                var threadStarter = new ThreadStart(async () =>
                {
                    var image = await MainForm.MusicDataFetcher.GetAlbumArtwork(musicPath);
                    if (image != null && MainForm.ArtworkImage != null && ImageHelper.IsSameImage(image, MainForm.ArtworkImage))
                    {
                        MainForm.SetUploadingMessage(
                                    message,
                                    tooltipText,
                                    null,
                                    false);
                    }
                    else
                    {
                        MainForm.SetUploadingMessage(
                                    message,
                                    tooltipText,
                                    image,
                                    true);
                    }
                });

                threadStarter += () =>
                {
                    _artworkFetchThread = null;
                };

                _artworkFetchThread = new Thread(threadStarter)
                {
                    IsBackground = true
                };
                _artworkFetchThread.Start();
            }
        }

        /// <summary>
        /// Accounts for if the user moves the music directory to another location
        /// </summary>
        private bool DoWeHaveAMusicFileWithTheSameHash(
            MusicFile musicFile,
            out MusicFile existingMusicFile)
        {
            existingMusicFile = null;
            if (string.IsNullOrEmpty(musicFile.Hash))
                return false;

            string hash = DirectoryHelper.GetFileHash(musicFile.Path).Result;
            var existing = MainForm.MusicFileRepo.GetDuplicate(hash, musicFile.Path).Result;

            if (existing != null)
            {
                existingMusicFile = existing;
                return true;
            }

            return false;
        }

        private bool ThreadIsAborting()
        {
            if (MainForm.Aborting)
            {
                Stopped = true;
                SetUploadDetails("Idle", null, true, false).Wait();
                Requests.UploadCheckCache.CleanUp = true;
                return true;
            }

            return false;
        }
    }
}
