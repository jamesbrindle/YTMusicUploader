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
using static YTMusicUploader.MainForm;

namespace YTMusicUploader.Business
{
    /// <summary>
    /// Responsible for managing uploading music files to YouTube Music.
    /// </summary>
    public class FileUploader
    {
        private MainForm MainForm;
        private List<MusicFile> MusicFiles;
        public bool Stopped { get; set; } = false;
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
            Stopped = false;

            _errorsCount = await MainForm.MusicFileRepo.CountIssues();
            _uploadedCount = await MainForm.MusicFileRepo.CountUploaded();
            _discoveredFilesCount = await MainForm.MusicFileRepo.CountAll();

            MusicFiles = MainForm.MusicFileRepo.LoadAll(true, true, true).Result;

            if (Requests.ArtistCache == null ||
                Requests.ArtistCache.LastSetTime < DateTime.Now.AddHours(-2) ||
                Requests.ArtistCache.Artists.Count == 0)
            {
                MainForm.SetManageTYMusicButtonEnabled(false);
                MainForm.SetStatusMessage("Gathering uploaded artists from YouTube Music...", "Gathering uploaded artists from YT Music");
                Requests.LoadArtistCache(MainForm.Settings.AuthenticationCookie);
                MainForm.SetManageTYMusicButtonEnabled(true);
            }

            if (Global.MultiThreadedRequests)
            {
                Requests.StartPrefetchingUploadedFilesCheck(
                                MusicFiles,
                                MainForm.Settings.AuthenticationCookie,
                                MainForm.MusicDataFetcher);
            }

            try
            {
                foreach (var musicFile in MusicFiles)
                {
                    if (MainFormIsAborting())
                        return;

                    if (File.Exists(musicFile.Path))
                    {
                        try
                        {
                            if (MainFormIsAborting())
                                return;

                            if (Requests.ArtistCache == null || Requests.ArtistCache.LastSetTime < DateTime.Now.AddHours(-2))
                            {
                                MainForm.SetManageTYMusicButtonEnabled(false);
                                Requests.LoadArtistCache(MainForm.Settings.AuthenticationCookie);
                                MainForm.SetManageTYMusicButtonEnabled(true);
                            }

                            if (MainFormIsAborting())
                                return;

                            musicFile.Hash = await DirectoryHelper.GetFileHash(musicFile.Path);
                            if (MainFormIsAborting())
                                return;

                            if (DoWeHaveAMusicFileWithTheSameHash(musicFile, out var existingMusicFile))
                            {
                                if (MainFormIsAborting())
                                    return;

                                HandleFileRenamedOrMove(musicFile, existingMusicFile).Wait();
                            }
                            else
                            {
                                while (!NetworkHelper.InternetConnectionIsUp())
                                {
                                    if (MainFormIsAborting())
                                        return;

                                    ThreadHelper.SafeSleep(1000);
                                }

                                while (MainForm.ManagingYTMusicStatus == ManagingYTMusicStatusEnum.Showing)
                                {
                                    MainForm.SetPaused(true);
                                    ThreadHelper.SafeSleep(1000);
                                }

                                if (MainForm.ManagingYTMusicStatus == ManagingYTMusicStatusEnum.CloseChanges)
                                    return;

                                if (MainFormIsAborting())
                                    return;

                                try
                                {
                                    HandleUploadCheck(musicFile).Wait();
                                }
                                catch (Exception e)
                                {
                                    bool success = false;
                                    for (int i = 0; i < 5; i++)
                                    {
                                        if (MainFormIsAborting())
                                            return;

                                        ThreadHelper.SafeSleep(1000);
                                        try
                                        {
                                            HandleUploadCheck(musicFile).Wait();
                                            success = true;
                                            break;
                                        }
                                        catch { }
                                    }

                                    var _ = e;
#if DEBUG
                                    Console.Out.WriteLine("FileUploader: Process: " + e.Message);
#endif
                                    if (!success)
                                        HandleFileNeedsUploading(musicFile).Wait();
                                }

                                await musicFile.Save();
                            }
                        }
                        catch (Exception e)
                        {
                            if (e.Message.ToLower().Contains("thread was being aborted") ||
                               (e.InnerException != null && e.InnerException.Message.ToLower().Contains("thread was being aborted")))
                            {
                                // Non-detrimental - Ignore to not clog up the application log
                                // Logger.Log(e, "Process.Process", Log.LogTypeEnum.Warning);
                            }
                            else
                            {
                                Logger.Log(e, "Process.Process", Log.LogTypeEnum.Critical);
                            }
                        }
                    }
                    else
                    {
                        _discoveredFilesCount--;
                        MainForm.SetDiscoveredFilesLabel(_discoveredFilesCount.ToString());
                        await musicFile.Delete();
                    }
                }
            }
            catch (Exception e)
            {
                if (e.Message.ToLower().Contains("thread was being aborted") ||
                   (e.InnerException != null && e.InnerException.Message.ToLower().Contains("thread was being aborted")))
                {
                    // Non-detrimental - Ignore to not clog up the application log
                    // Logger.Log(e, "Process.Process", Log.LogTypeEnum.Warning);
                }
                else
                {
                    Logger.Log(e, "Process.Process", Log.LogTypeEnum.Critical);
                }
            }

            if (MainForm.ManagingYTMusicStatus != ManagingYTMusicStatusEnum.Showing)
                await SetUploadDetails("Idle", null, true, false);

            Stopped = true;
        }

        private async Task HandleFileRenamedOrMove(MusicFile musicFile, MusicFile existingMusicFile)
        {
            // TODO: If there are mulitple exact files in the library (i.e. same album in 2 different folders?)
            // this method section will keep running every time on startup as the hash will be the same and it thinks
            // the files have been moved... Need to consider a how to handle this.

            if (MainFormIsAborting())
                return;

            Logger.LogInfo(
                "HandleFileRenamedOrMove",
                "File rename or moved detected. From: " + existingMusicFile.Path + " to: " + musicFile.Path);


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

        private async Task HandleUploadCheck(MusicFile musicFile)
        {
            if (MainFormIsAborting())
                return;

            var alreadyUploaded = Requests.IsSongUploaded(musicFile.Path,
                                                          MainForm.Settings.AuthenticationCookie,
                                                          out string entityId,
                                                          out string videoId,
                                                          MainForm.MusicDataFetcher);

            if (alreadyUploaded != Requests.UploadCheckResult.NotPresent)
            {
                if (MainFormIsAborting())
                    return;

                await HandleFileAlreadyUploaded(
                            musicFile,
                            entityId,
                            videoId);
            }
            else
            {
                if (MainFormIsAborting())
                    return;

                await HandleFileNeedsUploading(musicFile);
            }
        }

        private async Task HandleFileAlreadyUploaded(MusicFile musicFile, string entityId, string videoId)
        {
            if (MainFormIsAborting())
                return;

            await SetUploadDetails("Already Present: " + DirectoryHelper.EllipsisPath(musicFile.Path, 210), musicFile.Path, false, false);
            await SetUploadDetails("Already Present: " + DirectoryHelper.EllipsisPath(musicFile.Path, 210), musicFile.Path, true, false);
            MainForm.SetStatusMessage("Comparing and updating database with existing uploads", "Comparing files with YouTube Music");

            TrackAndReleaseMbId trackAndReleaseMbId = null;
            if (string.IsNullOrEmpty(musicFile.MbId) || string.IsNullOrEmpty(musicFile.ReleaseMbId))
                trackAndReleaseMbId = MainForm.MusicDataFetcher.GetTrackAndReleaseMbId(musicFile.Path, false);

            musicFile.LastUpload = DateTime.Now;
            musicFile.Error = false;
            musicFile.EntityId = entityId;
            musicFile.VideoId = videoId;

            try
            {
                musicFile.MbId = !string.IsNullOrEmpty(musicFile.MbId)
                                            ? musicFile.MbId
                                            : (!Requests.UploadCheckCache.CachedObjectHash.Contains(musicFile.Path)
                                                    ? trackAndReleaseMbId.MbId
                                                    : Requests.UploadCheckCache.CachedObjects.Where(m => m.MusicFilePath == musicFile.Path)
                                                                                             .FirstOrDefault()
                                                                                             .MbId);
            }
            catch { }

            try
            {
                musicFile.ReleaseMbId = !string.IsNullOrEmpty(musicFile.ReleaseMbId)
                                                ? musicFile.ReleaseMbId
                                                : (!Requests.UploadCheckCache.CachedObjectHash.Contains(musicFile.Path)
                                                        ? trackAndReleaseMbId.ReleaseMbId
                                                        : Requests.UploadCheckCache.CachedObjects.Where(m => m.MusicFilePath == musicFile.Path)
                                                                                                 .FirstOrDefault()
                                                                                                 .ReleaseMbId);
            }
            catch { }

            _uploadedCount++;
            MainForm.SetUploadedLabel(_uploadedCount.ToString());
        }

        private async Task HandleFileNeedsUploading(MusicFile musicFile)
        {
            if (MainFormIsAborting())
                return;

            Logger.LogInfo("HandleFileNeedsUploading", "Begin file upload: " + musicFile.Path);

            try
            {
                await SetUploadDetails(DirectoryHelper.EllipsisPath(musicFile.Path, 210), musicFile.Path, false, false);
                await SetUploadDetails(DirectoryHelper.EllipsisPath(musicFile.Path, 210), musicFile.Path, true, true); // Peform MusicBrainz lookup if required

                bool success = false;
                for (int i = 0; i < 10; i++)
                {
                    if (MainFormIsAborting())
                        return;

                    if (musicFile.Error != null &&
                        (bool)musicFile.Error &&
                        musicFile.UploadAttempts != null &&
                        musicFile.UploadAttempts >= Global.YTMusic500ErrorRetryAttempts + 1 &&
                        musicFile.LastUploadError > DateTime.Now.AddDays(-30))
                    {
                        break;
                    }

                    if (new FileInfo(musicFile.Path).Length > 299999999)
                    {
                        musicFile.Error = true;
                        musicFile.ErrorReason = "File size exeeds YTM limit of 300 MB per track.";
                        musicFile.LastUploadError = DateTime.Now;
                        musicFile.UploadAttempts = musicFile.UploadAttempts == null ? 0 : ((int)musicFile.UploadAttempts) + 1;
                        break;
                    }
                    else
                    {
                        if (MainFormIsAborting())
                            return;

                        Requests.UploadTrack(
                                MainForm,
                                MainForm.Settings.AuthenticationCookie,
                                musicFile.Path,
                                MainForm.Settings.ThrottleSpeed,
                                out string error);

                        if (MainFormIsAborting())
                            return;

                        if (!string.IsNullOrEmpty(error))
                        {
                            if (i >= Global.YTMusic500ErrorRetryAttempts)
                            {
                                musicFile.Error = true;
                                musicFile.ErrorReason = error;
                                musicFile.LastUploadError = DateTime.Now;
                                musicFile.UploadAttempts = musicFile.UploadAttempts == null 
                                                                ? 0 
                                                                : ((int)musicFile.UploadAttempts) + 1;

                                _errorsCount++;
                                MainForm.SetIssuesLabel(_errorsCount.ToString());

                                success = false;
                                break;
                            }
                            else
                            {
                                musicFile.Error = true;
                                musicFile.ErrorReason = error;
                                musicFile.LastUploadError = DateTime.Now;
                                musicFile.UploadAttempts = musicFile.UploadAttempts == null 
                                                                ? 0 
                                                                : ((int)musicFile.UploadAttempts) + 1;

                                MainForm.SetStatusMessage($"500 Error from YT Music. Waiting 10 seconds then trying again " +
                                                                $"({i + 1}/{Global.YTMusic500ErrorRetryAttempts})",
                                                          $"Retrying on 500 error " +
                                                          $"({i + 1}/{(musicFile.UploadAttempts == null || musicFile.UploadAttempts == 0 ? Global.YTMusic500ErrorRetryAttempts : 1)})");

                                ThreadHelper.SafeSleep(10000); // 10 seconds                       
                            }
                        }
                        else
                        {
                            success = true;
                            break;
                        }
                    }
                }

                if (success)
                {
                    if (MainFormIsAborting())
                        return;

                    TrackAndReleaseMbId trackAndReleaseMbId = null;
                    if (string.IsNullOrEmpty(musicFile.MbId) || string.IsNullOrEmpty(musicFile.ReleaseMbId))
                        trackAndReleaseMbId = MainForm.MusicDataFetcher.GetTrackAndReleaseMbId(musicFile.Path, true);

                    musicFile.LastUpload = DateTime.Now;
                    musicFile.Error = false;

                    try
                    {
                        musicFile.MbId = !string.IsNullOrEmpty(musicFile.MbId)
                                                    ? musicFile.MbId
                                                    : (!Requests.UploadCheckCache.CachedObjectHash.Contains(musicFile.Path)
                                                            ? trackAndReleaseMbId.MbId
                                                            : Requests.UploadCheckCache.CachedObjects.Where(m => m.MusicFilePath == musicFile.Path)
                                                                                                     .FirstOrDefault()
                                                                                                     .MbId);
                    }
                    catch { }

                    try
                    {
                        musicFile.ReleaseMbId = !string.IsNullOrEmpty(musicFile.ReleaseMbId)
                                                        ? musicFile.ReleaseMbId
                                                        : (!Requests.UploadCheckCache.CachedObjectHash.Contains(musicFile.Path)
                                                                ? trackAndReleaseMbId.ReleaseMbId
                                                                : Requests.UploadCheckCache.CachedObjects.Where(m => m.MusicFilePath == musicFile.Path)
                                                                                                         .FirstOrDefault()
                                                                                                         .ReleaseMbId);
                    }
                    catch { }

                    if (MainFormIsAborting())
                        return;

                    // We've uploaded it, so now see if we can get the YouTube Music entityId
                    if (Requests.IsSongUploaded(musicFile.Path,
                                            MainForm.Settings.AuthenticationCookie,
                                            out string entityId,
                                            out string videoId,
                                            MainForm.MusicDataFetcher,
                                            false) != Requests.UploadCheckResult.NotPresent)
                    {
                        musicFile.EntityId = entityId;
                        musicFile.VideoId = videoId;
                    }

                    _uploadedCount++;
                    MainForm.SetUploadedLabel(_uploadedCount.ToString());

                    Logger.LogInfo("HandleFileNeedsUploading", "File upload SUCCESS: " + musicFile.Path);
                }
            }
            catch (Exception e)
            {
                Logger.Log(e, "File upload FAIL: " + musicFile.Path);
            }
        }

        private async Task SetUploadDetails(
            string message,
            string musicPath,
            bool setArtworkImage,
            bool useMusicBrainzFallback)
        {
            try
            {
                string tooltipText = string.Empty;
                if (string.IsNullOrEmpty(musicPath))
                    tooltipText = "Idle";
                else
                    tooltipText = await MainForm.MusicDataFetcher.GetMusicFileMetaDataString(musicPath);

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
            catch (Exception e)
            {
                Logger.Log(e, "SetUploadDetails");
            }
        }

        private void SetCoverArtImageThreaded(string message, string tooltipText, string musicPath)
        {
            // Thread to set the cover artwork image, because this can take some unwanted time when
            // checking lots of music files to see if they're already uploaded to YouTube Music
            // and we want to cancel the thread if it's still running on the next track check
            // and just display the default cover artwork

            try
            {
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

                    _artworkFetchThread = new Thread(threadStarter)
                    {
                        IsBackground = true
                    };
                    _artworkFetchThread.Start();
                }
            }
            catch { }
        }

        /// <summary>
        /// Accounts for if the user moves the music directory to another location
        /// </summary>
        private bool DoWeHaveAMusicFileWithTheSameHash(
            MusicFile musicFile,
            out MusicFile existingMusicFile)
        {
            existingMusicFile = null;

            try
            {
                if (string.IsNullOrEmpty(musicFile.Hash))
                    return false;

                string hash = DirectoryHelper.GetFileHash(musicFile.Path).Result;
                var existing = MainForm.MusicFileRepo.GetDuplicate(hash, musicFile.Path).Result;

                if (existing != null)
                {
                    existingMusicFile = existing;
                    return true;
                }
            }
            catch (Exception e)
            {
                Logger.Log(e, "DoWeHaveAMusicFileWithTheSameHash");
            }

            return false;
        }

        private bool MainFormIsAborting()
        {
            if (MainForm.Aborting)
            {
                Stopped = true;
                SetUploadDetails("Restarting", "Restarting", true, true).Wait();
                Requests.UploadCheckCache.CleanUp = true;
                return true;
            }

            return false;
        }
    }
}
