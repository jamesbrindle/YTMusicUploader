using MetroFramework;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using YTMusicUploader.Dialogues;
using YTMusicUploader.Providers;
using YTMusicUploader.Providers.DataModels;

namespace YTMusicUploader
{
    public partial class MainForm
    {
        // Connect to TY Music

        private void BtnConnectToYouTube_Click(object sender, EventArgs e)
        {
            try
            {
                ConnectToYTMusicForm.Show();
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

                    ConnectToYTMusicForm.Show();
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
                Task.Run(async () => await AddWatchFolder());
            }
        }

        private async Task AddWatchFolder()
        {
            WatchFolderRepo.Insert(new WatchFolder
            {
                Path = FolderSelector.SelectedPath
            }).Wait();

            await BindWatchFoldersList();
            QueueChecker.Queue = true;
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
            Task.Run(async () => await RemoveWachFolder());
        }

        private async Task RemoveWachFolder()
        {
            try
            {
                if ((WatchFolder)lbWatchFolders.SelectedItem != null)
                {
                    await WatchFolderRepo.Delete(((WatchFolder)lbWatchFolders.SelectedItem).Id);
                    await MusicFileRepo.DeleteWatchFolder(((WatchFolder)lbWatchFolders.SelectedItem).Path);
                    RepopulateAmountLables();
                }
            }
            catch { }

            await BindWatchFoldersList();
            QueueChecker.Queue = true;
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

        // Log dialogues

        private void LblIssues_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                if (IssueLogForm == null)
                    IssueLogForm = new IssueLog(this);

                IssueLogForm.ShowDialog();
            }
            catch
            {
                IssueLogForm = new IssueLog(this);
                IssueLogForm.ShowDialog();
            }
        }

        private void LblUploaded_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                if (UploadLogForm == null)
                    UploadLogForm = new UploadLog(this);

                UploadLogForm.ShowDialog();
            }
            catch
            {
                UploadLogForm = new UploadLog(this);
                UploadLogForm.ShowDialog();
            }
        }

        private void LinkLabel_GotFocus(object sender, EventArgs e)
        {
            btnConnectToYoutube.Focus();
        }

        // YT Music Manage

        private void PbYtMusicManage_Click(object sender, EventArgs e)
        {
            if (ManageYTMusic == null || ManageYTMusic.IsDisposed)
                ManageYTMusic = new ManageYTMusic(this);
            else
            {
                ManageYTMusic.Dispose();
                ManageYTMusic = new ManageYTMusic(this);
            }

            ManageYTMusic.ShowDialog();

            pbYtMusicManage.Image = Properties.Resources.ytmusic_manage;
            new Thread((ThreadStart)delegate
            {
                Thread.Sleep(100);
                SetManageTYMusicButtonImage(Properties.Resources.ytmusic_manage);
            }).Start();
        }

        private void PbYtMusicManage_MouseDown(object sender, MouseEventArgs e)
        {
            pbYtMusicManage.Image = Properties.Resources.ytmusic_manage_down;
        }

        private void PbYtMusicManage_MouseEnter(object sender, EventArgs e)
        {
            pbYtMusicManage.Image = Properties.Resources.ytmusic_manage_hover;
        }

        private void PbYtMusicManage_MouseLeave(object sender, EventArgs e)
        {
            pbYtMusicManage.Image = Properties.Resources.ytmusic_manage;
        }

        private void PbYtMusicManage_MouseUp(object sender, MouseEventArgs e)
        {
            pbYtMusicManage.Image = Properties.Resources.ytmusic_manage_hover;
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

            pbAbout.Image = Properties.Resources.yt_logo;
            new Thread((ThreadStart)delegate
            {
                Thread.Sleep(100);
                SetAboutButtonImage(Properties.Resources.yt_logo);
            }).Start();
        }

        private void PbAbout_MouseDown(object sender, MouseEventArgs e)
        {
            pbAbout.Image = Properties.Resources.yt_logo_down;
        }

        private void PbAbout_MouseEnter(object sender, EventArgs e)
        {
            pbAbout.Image = Properties.Resources.yt_logo_hover;
        }

        private void PbAbout_MouseLeave(object sender, EventArgs e)
        {
            pbAbout.Image = Properties.Resources.yt_logo;
        }

        private void PbAbout_MouseUp(object sender, MouseEventArgs e)
        {
            pbAbout.Image = Properties.Resources.yt_logo_hover;
        }
    }
}
