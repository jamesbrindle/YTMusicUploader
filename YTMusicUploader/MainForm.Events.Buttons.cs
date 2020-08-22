using MetroFramework;
using System;
using System.Threading;
using System.Windows.Forms;
using YTMusicUploader.Dialogues;
using YTMusicUploader.Providers;
using YTMusicUploader.Providers.Models;

namespace YTMusicUploader
{
    public partial class MainForm
    {
        // Connect to TY Music

        private void BtnConnectToYoutube_Click(object sender, EventArgs e)
        {
            try
            {
                ConnectToYTMusicForm.ShowDialog();
                new Thread((ThreadStart)delegate
                {
                    SetConnectedToYouTubeMusic(Requests.IsAuthenticated(Settings.AuthenticationCookie));
                });
            }
            catch (Exception)
            {
                try
                {
                    // HACK: Odd behaviour on 'reshow' form. This is a workaround.

                    ConnectToYTMusicForm.BrowserControl.Dispose();
                    ConnectToYTMusicForm.Dispose();
                    ConnectToYTMusicForm = new ConnectToYTMusic(this);

                    ConnectToYTMusicForm.ShowDialog();
                    new Thread((ThreadStart)delegate
                    {
                        SetConnectedToYouTubeMusic(Requests.IsAuthenticated(Settings.AuthenticationCookie));
                    });
                }
                catch (Exception)
                {
                    MetroMessageBox.Show(
                        this,
                        Environment.NewLine + "You must install the latest verion of Microsoft Edge from  the Canary channel for this to work:" +
                            Environment.NewLine + Environment.NewLine + "https://www.microsoftedgeinsider.com/en-us/download",
                        "Dependency Required",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Stop,
                        200);
                }
            }
        }

        // Add To Wath Folder

        private void BtnWatchFolder_Click(object sender, EventArgs e)
        {
            var openResult = FolderSelector.ShowDialog();
            if (openResult == DialogResult.OK)
            {
                Aborting = true;

                WatchFolderRepo.Insert(new WatchFolder
                {
                    Path = FolderSelector.SelectedPath
                });

                WatchFolders = WatchFolderRepo.Load();
                BindWatchFoldersList();

                Queue = true;
            }
        }

        private void BtnAddWatchFolder_MouseEnter(object sender, EventArgs e)
        {
            btnAddWatchFolder.Image = Properties.Resources.plus_hover;
        }

        private void BtnAddWatchFolder_MouseLeave(object sender, EventArgs e)
        {
            btnAddWatchFolder.Image = Properties.Resources.plus;
        }

        private void BtnAddWatchFolder_MouseDown(object sender, MouseEventArgs e)
        {
            btnAddWatchFolder.Image = Properties.Resources.plus_down;
        }

        // Remove from Watch Folder

        private void BtnRemoveWatchFolder_Click(object sender, EventArgs e)
        {
            Aborting = true;

            try
            {
                if ((WatchFolder)lbWatchFolders.SelectedItem != null)
                {
                    WatchFolderRepo.Delete(((WatchFolder)lbWatchFolders.SelectedItem).Id);
                    MusicFileRepo.DeleteWatchFolder(((WatchFolder)lbWatchFolders.SelectedItem).Path);
                }
            }
            catch { }

            BindWatchFoldersList();
            Queue = true;
        }

        private void BtnRemoveWatchFolder_MouseEnter(object sender, EventArgs e)
        {
            btnRemoveWatchFolder.Image = Properties.Resources.minus_hover;
        }

        private void BtnRemoveWatchFolder_MouseLeave(object sender, EventArgs e)
        {
            btnRemoveWatchFolder.Image = Properties.Resources.minus;
        }

        private void BtnRemoveWatchFolder_MouseDown(object sender, MouseEventArgs e)
        {
            btnRemoveWatchFolder.Image = Properties.Resources.minus_down;
        }

        // About

        private void PbAbout_Click(object sender, EventArgs e)
        {
            MetroMessageBox.Show(
               this,
               Environment.NewLine + "Version: " + Global.ApplicationVersion +
                    Environment.NewLine + Environment.NewLine + "By James Brindle" +
                    Environment.NewLine + Environment.NewLine + "https://github.com/jamesbrindle/YTMusicUploader",
               "YT Music Uploader",
               MessageBoxButtons.OK,
               MessageBoxIcon.Asterisk,
               200);
        }
    }
}
