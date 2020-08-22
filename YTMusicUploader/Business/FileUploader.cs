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
        public bool Stopped { get; set; } = true;

        public FileUploader(MainForm mainForm)
        {
            MainForm = mainForm;
        }

        public void Process()
        {
            MusicFiles = MainForm.MusicFileRepo.LoadAll(true, true, true);     
            foreach (var musicFile in MusicFiles)
            {
                if (MainForm.Aborting)
                {
                    Stopped = true;
                    MainForm.SetUploadingMessage("Uploading: N/A");
                    return;
                }

                MainForm.SetUploadingMessage("Uploading: " + DirectoryHelper.EllipsisPath(musicFile.Path, 210));

                Stopped = false;
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
                    MainForm.SetIssuesLabel((MainForm.GetIssuesLabel().ToInt() + 1).ToString());
                }
                else
                {
                    musicFile.LastUpload = DateTime.Now;
                    MainForm.SetUploadedLabel((MainForm.GetUploadLabel().ToInt() + 1).ToString());
                }
               
                musicFile.Save();
            }

            MainForm.SetUploadingMessage("Uploading: N/A");
            Stopped = true;
        }
    }
}
