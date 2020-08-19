using JBToolkit.Windows;
using System;
using System.Collections.Generic;
using YTMusicUploader.Providers;
using YTMusicUploader.Providers.Models;

namespace YTMusicUploader.Business
{
    public class FileUploader
    {
        private MainForm MainForm;
        private List<MusicFile> MusicFiles;

        public FileUploader(MainForm mainForm)
        {
            MainForm = mainForm;
        }

        public void Process()
        {
            MusicFiles = MainForm.MusicFileRepo.LoadAll(true, true, true);     
            foreach (var musicFile in MusicFiles)
            {
                MainForm.SetUploadingMessage("Uploading: " + DirectoryHelper.EllipsisPath(musicFile.Path, 210));

                Requests.UploadSong(
                            MainForm.Settings.AuthenticationCookie, 
                            musicFile.Path, 
                            MainForm.Settings.ThrottleSpeed,
                            out string error);

                if (!string.IsNullOrEmpty(error))
                {
                    musicFile.Error = true;
                    musicFile.ErrorReason = error;
                    MainForm.SetIssuesLabel((MainForm.GetIssuesLabel().ToInt() + 1).ToString());
                }
                else
                {
                    musicFile.LastUpload = DateTime.Now;
                    MainForm.SetUploadedLabel((MainForm.GetUploadLabel().ToInt() + 1).ToString());
                }
               
                musicFile.Save();
            }
        }
    }
}
