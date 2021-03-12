﻿using JBToolkit.Assemblies;
using JBToolkit.Threads;
using MetroFramework;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using YTMusicUploader.Dialogues;
using YTMusicUploader.Helpers;
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
                ThreadPool.QueueUserWorkItem(delegate
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
                    ThreadPool.QueueUserWorkItem(delegate
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
            try
            {
                Logger.LogInfo("AddWatchFolder", "Watch folder added: " + FolderSelector.SelectedPath);

                WatchFolderRepo.Insert(new WatchFolder
                {
                    Path = FolderSelector.SelectedPath
                }).Wait();

                await BindWatchFoldersList();
                Restart();
            }
            catch (Exception e)
            {
                Logger.Log(e, "AddWatchFolder");
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
            if (lbWatchFolders.SelectedIndex > -1)
            {
                Aborting = true;
                Task.Run(async () => await RemoveWachFolder());
            }
        }
        private async Task RemoveWachFolder()
        {
            try
            {
                if ((WatchFolder)lbWatchFolders.SelectedItem != null)
                {
                    Logger.LogInfo("RemoveWachFolder", "Watch folder removed: " + ((WatchFolder)lbWatchFolders.SelectedItem).Path);

                    await WatchFolderRepo.Delete(((WatchFolder)lbWatchFolders.SelectedItem).Id);
                    await MusicFileRepo.DeleteWatchFolder(((WatchFolder)lbWatchFolders.SelectedItem).Path);
                    RepopulateAmountLables();
                }
            }
            catch (Exception e)
            {
                Logger.Log(e, "RemoveWachFolder");
            }

            await BindWatchFoldersList();
            Restart();
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

                var result = IssueLogForm.ShowDialog();
                if (result == DialogResult.Yes)
                    Restart();
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
            ManagingYTMusicStatus = ManagingYTMusicStatusEnum.Showing;
            Requests.UploadCheckCache.Pause = true;

            if (ManageYTMusic == null || ManageYTMusic.IsDisposed)
                ManageYTMusic = new ManageYTMusic(this);

            SetPaused(true);
            EnableTrayPauseResume(false);

            var result = ManageYTMusic.ShowDialog();
            if (!IsPausedFromTray)
            {
                if (result == DialogResult.Yes)
                {
                    ManagingYTMusicStatus = ManagingYTMusicStatusEnum.CloseChanges;
                    Restart();
                    ManagingYTMusicStatus = ManagingYTMusicStatusEnum.CloseChangesComplete;
                }
                else
                    ManagingYTMusicStatus = ManagingYTMusicStatusEnum.CloseNoChange;

                Requests.UploadCheckCache.Pause = false;
                SetPaused(false);

                pbYtMusicManage.Image = Properties.Resources.ytmusic_manage;
                ThreadPool.QueueUserWorkItem(delegate
                {
                    ThreadHelper.SafeSleep(100);
                    SetManageTYMusicButtonImage(Properties.Resources.ytmusic_manage);
                    ThreadHelper.SafeSleep(250);
                    SetPaused(false);
                });
            }

            EnableTrayPauseResume(true);
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

        // Log

        private void PbLog_MouseDown(object sender, MouseEventArgs e)
        {
            pbLog.Image = Properties.Resources.log_down;
        }

        private void PbLog_MouseEnter(object sender, EventArgs e)
        {
            pbLog.Image = Properties.Resources.log_hover;
        }

        private void PbLog_MouseLeave(object sender, EventArgs e)
        {
            pbLog.Image = Properties.Resources.log_up;
        }

        private void PLog_MouseUp(object sender, MouseEventArgs e)
        {
            pbLog.Image = Properties.Resources.log_hover;
        }

        private void PbLog_Click(object sender, EventArgs e)
        {
            try
            {
                if (GeneralLogForm == null)
                    GeneralLogForm = new ApplicationLog();

                GeneralLogForm.ShowDialog();
            }
            catch
            {
                GeneralLogForm = new ApplicationLog();
                GeneralLogForm.ShowDialog();
            }

            pbLog.Image = Properties.Resources.log_up;
            ThreadPool.QueueUserWorkItem(delegate
            {
                ThreadHelper.SafeSleep(100);
                SetLogButtonImage(Properties.Resources.log_up);
            });
        }

        //
        // Latest Version
        //

        private void PbUpdate_Click(object sender, EventArgs e)
        {
            pbUpdate.Image = Properties.Resources.update_up;
            var result = MetroMessageBox.Show(
                           this,
                           Environment.NewLine +
                           "A new version of YT Music Uploader is available which will likely offer bug fixes and feature enhancements." +
                           Environment.NewLine + Environment.NewLine +
                           "Current version: " + VersionHelper.GetVersionFull() +
                           Environment.NewLine +
                           "Latest version: " + LatestVersionTag +
                           Environment.NewLine + Environment.NewLine +
                           "Would you like to install the new version (automatic download and install)?",
                           "New YT Music Uploader Available",
                           MessageBoxButtons.YesNoCancel,
                           MessageBoxIcon.Question,
                           270);

            pbUpdate.Image = Properties.Resources.update_up;
            ThreadPool.QueueUserWorkItem(delegate
            {
                ThreadHelper.SafeSleep(100);
                SetUpdateButtonImage(Properties.Resources.update_up);
            });

            if (result == DialogResult.Yes)
            {
                string platform = InstalledApplicationHelper.GetInstalledPlatform();
                string downloadUrl = LatestVersionUrl.Replace("/tag/", "/download/") + $"/YT.Music.Uploader.v{LatestVersionTag.Replace("v", "")}.Installer-{platform}.msi";
                string downloadPath = Path.Combine(Global.AppDataLocation, "Updates");
                string version = LatestVersionTag.Replace("v", "");
                string installedLocation = System.Reflection.Assembly.GetEntryAssembly().Location;

                var process = new Process();
                process.StartInfo.FileName = UpdaterPath;
                process.StartInfo.Arguments = $"\"{downloadUrl}\" \"{downloadPath}\" \"{version}\" \"{installedLocation}\"";
                process.Start();
            }
        }

        private void PbUpdate_MouseDown(object sender, MouseEventArgs e)
        {
            pbUpdate.Image = Properties.Resources.update_down;
        }

        private void PbUpdate_MouseEnter(object sender, EventArgs e)
        {
            pbUpdate.Image = Properties.Resources.update_hover;
        }

        private void PbUpdate_MouseLeave(object sender, EventArgs e)
        {
            pbUpdate.Image = Properties.Resources.update_up;
        }

        private void PbUpdate_MouseUp(object sender, MouseEventArgs e)
        {
            pbUpdate.Image = Properties.Resources.update_hover;
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
            ThreadPool.QueueUserWorkItem(delegate
            {
                ThreadHelper.SafeSleep(100);
                SetAboutButtonImage(Properties.Resources.yt_logo);
            });
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

        // Upload Playlists Checkbox Option Info


        private void PbUploadPlaylistsInfo_Click(object sender, EventArgs e)
        {
            MetroMessageBox.Show(
               this,
               Environment.NewLine + 
               "Unfortunatley, YouTube / YouTube Music has a playlists creation limit of 25 per day and 10 for rapid playlist creation (i.e. via automation / API). " +
               "After which YTM will block any playlist creation for 24 hours." +
               Environment.NewLine + Environment.NewLine +
               $"This application prevents 'rapid creation' by waiting between {Global.PlaylistCreationWait} and {Global.PlaylistCreationWait * 2} " + 
               "seconds after creating a playlist and to not abuse YTM playlist creation policy " + 
               "and then will cease playlist creation. Therefore it may take some time / days for all your playlists to be uploaded. An 'upload playlist session' will be automatically " +
               $"started after {Global.SessionRestartHours} hours if left running. You do not need to restart the application." +
               Environment.NewLine + Environment.NewLine +
               "If you find this application is interfering with your playlist creation on the YTM website you can turn uploading playlists off.",
               "Include the Uploading of Playlists",
               MessageBoxButtons.OK,
               MessageBoxIcon.Information,
               345);
        }

        private void PbUploadPlaylistsInfo_MouseDown(object sender, MouseEventArgs e)
        {
            pbUploadPlaylistsInfo.Image = Properties.Resources.info_down;
        }

        private void PbUploadPlaylistsInfo_MouseEnter(object sender, EventArgs e)
        {
            pbUploadPlaylistsInfo.Image = Properties.Resources.info_hover;
        }

        private void PbUploadPlaylistsInfo_MouseLeave(object sender, EventArgs e)
        {
            pbUploadPlaylistsInfo.Image = Properties.Resources.info_up;
        }

        private void PbUploadPlaylistsInfo_MouseUp(object sender, MouseEventArgs e)
        {
            pbUploadPlaylistsInfo.Image = Properties.Resources.info_hover;
        }
    }
}
