using JBToolkit.Windows;
using System.Windows.Forms;

namespace YTMusicUploader
{
    public partial class MainForm
    {
        delegate void SetStatusMessageDelegate(string statusMessage, string systemTrayIconText = null);
        public void SetStatusMessage(string statusMessage, string systemTrayIconText = null)
        {
            if (lblStatus.InvokeRequired)
            {
                SetStatusMessageDelegate d = new SetStatusMessageDelegate(SetStatusMessage);
                Invoke(d, new object[] { statusMessage, systemTrayIconText });
            }
            else
            {
                lblStatus.Text = statusMessage;

                if (!string.IsNullOrEmpty(systemTrayIconText))
                    SetSystemTrayIconText(systemTrayIconText);
            }
        }
        public void SetSystemTrayIconText(string text)
        {
            TrayIcon.Text = "YT Music Uploader\r\n" + text;
        }

        delegate void SetConnectedToYouTubeMusicDelegate(bool connectedToYouTubeMusic);
        public void SetConnectedToYouTubeMusic(bool connectedToYouTubeMusic)
        {
            try
            {
                ConnectedToYTMusic = connectedToYouTubeMusic;

                if (pbConnectedToYoutube.InvokeRequired ||
                    pbNotConnectedToYoutube.InvokeRequired)
                {
                    SetConnectedToYouTubeMusicDelegate d = new SetConnectedToYouTubeMusicDelegate(SetConnectedToYouTubeMusic);
                    Invoke(d, new object[] { connectedToYouTubeMusic });
                }
                else
                {
                    if (connectedToYouTubeMusic)
                    {
                        pbConnectedToYoutube.Visible = true;
                        pbNotConnectedToYoutube.Visible = false;
                    }
                    else
                    {
                        pbConnectedToYoutube.Visible = false;
                        pbNotConnectedToYoutube.Visible = true;
                    }
                }
            }
            catch { }
        }

        delegate void SetStartWithWindowsDelegate(bool startWithWindows);
        public void SetStartWithWindows(bool startWithWindows)
        {
            if (cbStartWithWindows.InvokeRequired)
            {
                SetStartWithWindowsDelegate d = new SetStartWithWindowsDelegate(SetStartWithWindows);
                Invoke(d, new object[] { startWithWindows });
            }
            else
            {
                cbStartWithWindows.Checked = startWithWindows;
            }
        }

        delegate void SetThrottleSpeedDelegate(string mbps);
        public void SetThrottleSpeed(string mbps)
        {
            if (tbThrottleSpeed.InvokeRequired)
            {
                SetThrottleSpeedDelegate d = new SetThrottleSpeedDelegate(SetThrottleSpeed);
                Invoke(d, new object[] { mbps });
            }
            else
            {
                if (mbps == "0" || mbps == "-1" || mbps == "∞")
                    tbThrottleSpeed.Text = "∞";
                else
                {
                    tbThrottleSpeed.Text = mbps.ToString();
                }
            }
        }

        delegate void SetVersionDelegate(string version);
        public void SetVersion(string version)
        {
            if (lblVersion.InvokeRequired)
            {
                SetVersionDelegate d = new SetVersionDelegate(SetVersion);
                Invoke(d, new object[] { version });
            }
            else
            {
                lblVersion.Text = version;
            }
        }

        delegate void SetUploadingMessageDelegate(string text);
        public void SetUploadingMessage(string text)
        {
            if (lblUploadingMessage.InvokeRequired)
            {
                SetUploadingMessageDelegate d = new SetUploadingMessageDelegate(SetUploadingMessage);
                Invoke(d, new object[] { text });
            }
            else
            {
                lblUploadingMessage.Text = text;
            }
        }

        delegate void SetDiscoveredFilesLabelDelegate(string text);
        public void SetDiscoveredFilesLabel(string text)
        {
            if (lblDiscoveredFiles.InvokeRequired)
            {
                SetDiscoveredFilesLabelDelegate d = new SetDiscoveredFilesLabelDelegate(SetDiscoveredFilesLabel);
                Invoke(d, new object[] { text });
            }
            else
            {
                lblDiscoveredFiles.Text = text;
            }
        }

        delegate void SetIssuesLabelDelegate(string text);
        public void SetIssuesLabel(string text)
        {
            if (lblIssues.InvokeRequired)
            {
                SetIssuesLabelDelegate d = new SetIssuesLabelDelegate(SetIssuesLabel);
                Invoke(d, new object[] { text });
            }
            else
            {
                lblIssues.Text = text;
            }
        }

        delegate string GetIssuesLabelDelegate();
        public string GetIssuesLabel()
        {
            if (lblIssues.InvokeRequired)
            {
                GetIssuesLabelDelegate d = new GetIssuesLabelDelegate(GetIssuesLabel);
                return (string)Invoke(d, new object[] { });
            }
            else
            {
                return lblIssues.Text;
            }
        }

        delegate void SetUploadedLabelDelegate(string text);
        public void SetUploadedLabel(string text)
        {
            if (lblUploaded.InvokeRequired)
            {
                SetUploadedLabelDelegate d = new SetUploadedLabelDelegate(SetUploadedLabel);
                Invoke(d, new object[] { text });
            }
            else
            {
                lblUploaded.Text = text;
            }
        }

        delegate string GetUploadLabelDelegate();
        public string GetUploadLabel()
        {
            if (lblUploaded.InvokeRequired)
            {
                GetUploadLabelDelegate d = new GetUploadLabelDelegate(GetUploadLabel);
                return (string)Invoke(d, new object[] { });
            }
            else
            {
                return lblUploaded.Text;
            }
        }

        delegate void BindWatchFoldersListDelegate();
        public void BindWatchFoldersList()
        {
            WatchFolders = WatchFolderRepo.Load();

            if (lbWatchFolders.InvokeRequired)
            {
                BindWatchFoldersListDelegate d = new BindWatchFoldersListDelegate(BindWatchFoldersList);
                Invoke(d, new object[] { });
            }
            else
            {
                foreach (var watchFolder in WatchFolders)
                {
                    if (watchFolder.Path == @"%USERPROFILE%\Music")
                        watchFolder.Path = DirectoryHelper.EllipsisPath(DirectoryHelper.GetPath(KnownFolder.Music), 100);
                    else
                        DirectoryHelper.EllipsisPath(watchFolder.Path, 100);
                }

                lbWatchFolders.DataSource = new BindingSource(WatchFolders, null);
                lbWatchFolders.DisplayMember = "Path";
                lbWatchFolders.ValueMember = "Id";
            }
        }

        delegate void SetConnectToYouTubeButtonEnabledDelegate(bool enabled);
        public void SetConnectToYouTubeButtonEnabled(bool enabled)
        {
            if (btnConnectToYoutube.InvokeRequired)
            {
                SetConnectToYouTubeButtonEnabledDelegate d =
                    new SetConnectToYouTubeButtonEnabledDelegate(SetConnectToYouTubeButtonEnabled);
                Invoke(d, new object[] { enabled });
            }
            else
            {
                btnConnectToYoutube.Enabled = enabled;
            }
        }
    }
}
