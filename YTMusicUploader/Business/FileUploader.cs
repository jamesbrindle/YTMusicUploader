using JBToolkit.Network;
using JBToolkit.Windows;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
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

        private int _errors = 0;
        private int _uploaded = 0;
        private int _discoveredFiles = 0;

        public FileUploader(MainForm mainForm)
        {
            MainForm = mainForm;
        }

        /// <summary>
        /// Execute the upload process
        /// </summary>
        public void Process()
        {
            _errors = MainForm.MusicFileRepo.CountIssues();
            _uploaded = MainForm.MusicFileRepo.CountUploaded();
            _discoveredFiles = MainForm.MusicFileRepo.CountAll();

            MusicFiles = MainForm.MusicFileRepo.LoadAll(true, true, true);
            foreach (var musicFile in MusicFiles)
            {
                if (File.Exists(musicFile.Path))
                {
                    musicFile.Hash = DirectoryHelper.GetFileHash(musicFile.Path);

                    if (MainForm.Aborting)
                    {
                        Stopped = true;
                        MainForm.SetUploadingMessage("Idle", "idle");
                        return;
                    }

                    if (DoWeHaveAMusicFileWithTheSameHash(musicFile, out MusicFile existingMusicFile))
                    {
                        MainForm.SetUploadingMessage("Already Present: " + DirectoryHelper.EllipsisPath(musicFile.Path, 210), musicFile.Path);
                        MainForm.SetStatusMessage("Updating database for existing uploads");

                        existingMusicFile.Path = musicFile.Path;
                        existingMusicFile.LastUpload = DateTime.Now;
                        existingMusicFile.Removed = false;
                        existingMusicFile.MbId = string.IsNullOrEmpty(musicFile.MbId)
                                                          ? MainForm.MusicDataFetcher.GetTrackMbId(musicFile.Path)
                                                          : musicFile.MbId;

                        _uploaded++;
                        MainForm.SetUploadedLabel(_uploaded.ToString());

                        musicFile.Delete(true);
                        existingMusicFile.Save();
                    }
                    else
                    {
                        MainForm.SetUploadingMessage(DirectoryHelper.EllipsisPath(musicFile.Path, 210), musicFile.Path);
                        Stopped = false;

                        while (!NetworkHelper.InternetConnectionIsUp())
                        {
                            try
                            {
                                if (MainForm.Aborting)
                                {
                                    Stopped = true;
                                    MainForm.SetUploadingMessage("Idle", "idle");
                                    return;
                                }

                                Thread.Sleep(1000);
                            }
                            catch { }
                        }

                        if (Requests.IsSongUploaded(musicFile.Path, MainForm.Settings.AuthenticationCookie, MainForm.MusicDataFetcher))
                        {
                            MainForm.SetUploadingMessage("Already Present: " + DirectoryHelper.EllipsisPath(musicFile.Path, 210), musicFile.Path);
                            MainForm.SetStatusMessage("Updating database for existing uploads");

                            musicFile.LastUpload = DateTime.Now;
                            musicFile.Error = false;
                            musicFile.MbId = string.IsNullOrEmpty(musicFile.MbId)
                                                        ? MainForm.MusicDataFetcher.GetTrackMbId(musicFile.Path)
                                                        : musicFile.MbId;

                            _uploaded++;
                            MainForm.SetUploadedLabel(_uploaded.ToString());
                        }
                        else
                        {
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

                                _errors++;
                                MainForm.SetIssuesLabel(_errors.ToString());
                            }
                            else
                            {
                                musicFile.LastUpload = DateTime.Now;
                                musicFile.Error = false;
                                musicFile.MbId = string.IsNullOrEmpty(musicFile.MbId)
                                                            ? MainForm.MusicDataFetcher.GetTrackMbId(musicFile.Path)
                                                            : musicFile.MbId;

                                _uploaded++;
                                MainForm.SetUploadedLabel(_uploaded.ToString());
                            }

                            GC.Collect();
                            GC.WaitForPendingFinalizers();
                        }

                        musicFile.Save();
                    }
                }
                else
                {
                    _discoveredFiles--;
                    MainForm.SetDiscoveredFilesLabel(_discoveredFiles.ToString());
                    musicFile.Delete();
                }
            }

            MainForm.SetUploadingMessage("Idle", "idle");
            Stopped = true;
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

            string hash = DirectoryHelper.GetFileHash(musicFile.Path);
            var existing = MainForm.MusicFileRepo.GetDuplicate(hash, musicFile.Path);

            if (existing != null)
            {
                existingMusicFile = existing;
                return true;
            }

            return false;
        }
    }
}
