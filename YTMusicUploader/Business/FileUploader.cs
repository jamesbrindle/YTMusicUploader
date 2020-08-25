using JBToolkit.Imaging;
using JBToolkit.Network;
using JBToolkit.Threads;
using JBToolkit.Windows;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using YTMusicUploader.Providers;
using YTMusicUploader.Providers.DataModels;

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

        public FileUploader(MainForm mainForm)
        {
            MainForm = mainForm;
        }

        /// <summary>
        /// Execute the upload process
        /// </summary>
        public async Task Process()
        {
            var errorsCount = await MainForm.MusicFileRepo.CountIssues();
            var uploadedCount = await MainForm.MusicFileRepo.CountUploaded();
            var discoveredFilesCount = await MainForm.MusicFileRepo.CountAll();

            MusicFiles = MainForm.MusicFileRepo.LoadAll(true, true, true).Result;      
            foreach (var musicFile in MusicFiles)
            {
                if (File.Exists(musicFile.Path))
                {
                    musicFile.Hash = await DirectoryHelper.GetFileHash(musicFile.Path);
                    if (MainForm.Aborting)
                    {
                        Stopped = true;
                        MainForm.SetUploadingMessage("Idle", "idle");
                        return;
                    }

                    if (DoWeHaveAMusicFileWithTheSameHash(musicFile, out MusicFile existingMusicFile))
                    {
                        await SetUploadDetails("Already Present: " + DirectoryHelper.EllipsisPath(musicFile.Path, 210), musicFile.Path, false);
                        await SetUploadDetails("Already Present: " + DirectoryHelper.EllipsisPath(musicFile.Path, 210), musicFile.Path, true);
                        MainForm.SetStatusMessage("Comparing file system against database for existing uploads", "Comparing file system against the DB");

                        existingMusicFile.Path = musicFile.Path;
                        existingMusicFile.LastUpload = DateTime.Now;
                        existingMusicFile.Removed = false;
                        existingMusicFile.MbId = string.IsNullOrEmpty(musicFile.MbId)
                                                          ? await MainForm.MusicDataFetcher.GetTrackMbId(musicFile.Path)
                                                          : musicFile.MbId;

                        uploadedCount++;
                        MainForm.SetUploadedLabel(uploadedCount.ToString());

                        musicFile.Delete(true).Wait();
                        await existingMusicFile.Save();
                    }
                    else
                    {
                        Stopped = false;
                        await SetUploadDetails(DirectoryHelper.EllipsisPath(musicFile.Path, 210), musicFile.Path, false);
                        await SetUploadDetails(DirectoryHelper.EllipsisPath(musicFile.Path, 210), musicFile.Path, true);

                        while (!NetworkHelper.InternetConnectionIsUp())
                        {
                            if (MainForm.Aborting)
                            {
                                Stopped = true;
                                MainForm.SetUploadingMessage("Idle", "idle");
                                return;
                            }

                            ThreadHelper.SafeSleep(1000);
                        }

                        //
                        // HACK: 30 seconds to complete this operations, it's been know to get stuck?
                        //
                        var checkUploadedTask = Requests.IsSongUploaded(musicFile.Path, MainForm.Settings.AuthenticationCookie, MainForm.MusicDataFetcher);
                        bool trackAlreadyUploaded = false;
                        if (await Task.WhenAny(checkUploadedTask, Task.Delay(30000)) == checkUploadedTask)
                            if (checkUploadedTask.Result == true)
                                trackAlreadyUploaded = true;

                        if (trackAlreadyUploaded)
                        {
                            if (MainForm.Aborting)
                            {
                                Stopped = true;
                                MainForm.SetUploadingMessage("Idle", "idle");
                                return;
                            }

                            await SetUploadDetails("Already Present: " + DirectoryHelper.EllipsisPath(musicFile.Path, 210), musicFile.Path, false);
                            await SetUploadDetails("Already Present: " + DirectoryHelper.EllipsisPath(musicFile.Path, 210), musicFile.Path, true);

                            MainForm.SetStatusMessage("Comparing and updating database with existing uploads", "Comparing files with YouTube Music");

                            musicFile.LastUpload = DateTime.Now;
                            musicFile.Error = false;
                            musicFile.MbId = string.IsNullOrEmpty(musicFile.MbId)
                                                        ? await MainForm.MusicDataFetcher.GetTrackMbId(musicFile.Path)
                                                        : musicFile.MbId;

                            uploadedCount++;
                            MainForm.SetUploadedLabel(uploadedCount.ToString());
                        }
                        else
                        {
                            if (MainForm.Aborting)
                            {
                                Stopped = true;
                                MainForm.SetUploadingMessage("Idle", "idle");
                                return;
                            }

                            Requests.UploadSong(
                                        MainForm,
                                        MainForm.Settings.AuthenticationCookie,
                                        musicFile.Path,
                                        MainForm.Settings.ThrottleSpeed,
                                        out string error);

                            if (!string.IsNullOrEmpty(error))
                            {
                                musicFile.Error = true;
                                musicFile.ErrorReason = error;

                                errorsCount++;
                                MainForm.SetIssuesLabel(errorsCount.ToString());
                            }
                            else
                            {
                                musicFile.LastUpload = DateTime.Now;
                                musicFile.Error = false;
                                musicFile.MbId = string.IsNullOrEmpty(musicFile.MbId)
                                                            ? await MainForm.MusicDataFetcher.GetTrackMbId(musicFile.Path)
                                                            : musicFile.MbId;

                                uploadedCount++;
                                MainForm.SetUploadedLabel(uploadedCount.ToString());
                            }

                            GC.Collect();
                            GC.WaitForPendingFinalizers();
                        }

                        await musicFile.Save();
                    }
                }
                else
                {
                    discoveredFilesCount--;
                    MainForm.SetDiscoveredFilesLabel(discoveredFilesCount.ToString());
                    await musicFile.Delete();
                }
            }

            MainForm.SetUploadingMessage("Idle", "idle");
            Stopped = true;
        }

        private async Task SetUploadDetails(string message, string musicPath, bool setArtworkImage)
        {
            string initialTooltipText = await MainForm.MusicDataFetcher.GetMusicFileMetaDataString(musicPath, false);
            if (!setArtworkImage)
            {
                // just set the text

                MainForm.SetUploadingMessage(
                   message,
                   initialTooltipText,
                   null,
                   false);

                await Task.Run(() => { });
            }
            else
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

                    if (MainForm.ArtworkImage != null && ImageHelper.IsSameImage(MainForm.ArtworkImage, Properties.Resources.default_artwork))
                    {
                        MainForm.SetUploadingMessage(
                              message,
                              initialTooltipText,
                              null,
                              false);
                    }
                    else
                    {
                        MainForm.SetUploadingMessage(
                            message,
                            initialTooltipText,
                            Properties.Resources.default_artwork,
                            true);
                    }
                }

                var threadStarter = new ThreadStart(async () =>
                {
                    var image = await MainForm.MusicDataFetcher.GetAlbumArtwork(musicPath);
                    var finalTooltipText = await MainForm.MusicDataFetcher.GetMusicFileMetaDataString(musicPath);

                    if (image != null && MainForm.ArtworkImage != null && ImageHelper.IsSameImage(image, MainForm.ArtworkImage))
                    {
                        MainForm.SetUploadingMessage(
                            message,
                            finalTooltipText,
                            null,
                            false);
                    }
                    else
                    {
                        MainForm.SetUploadingMessage(
                            message,
                            finalTooltipText,
                            image,
                            true);
                    }
                });

                threadStarter += () =>
                {
                    _artworkFetchThread = null;
                };

                _artworkFetchThread = new Thread(threadStarter);
                _artworkFetchThread.Start();

                await Task.Run(() => { });
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
    }
}
