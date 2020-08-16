using JBToolkit.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YTMusicUploader.Providers.Models;

namespace YTMusicUploader
{
    public partial class MainForm
    {
        delegate void SetConnectedToYouTubeMusicDelegate(bool connectedToYouTubeMusic);
        public void SetConnectedToYouTubeMusic(bool connectedToYouTubeMusic)
        {
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

        delegate void SetThrottleSpeedDelegate(int mbps);
        public void SetThrottleSpeed(int mbps)
        {
            if (tbThrottleSpeed.InvokeRequired)
            {
                SetThrottleSpeedDelegate d = new SetThrottleSpeedDelegate(SetThrottleSpeed);
                Invoke(d, new object[] { mbps });
            }
            else
            {
                if (mbps == 0 || mbps == -1)
                    tbThrottleSpeed.Text = "∞";
                else
                    tbThrottleSpeed.Text = mbps.ToString();
            }
        }

        delegate void BindWatchFoldersListDelegate();
        public void BindWatchFoldersList()
        {
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

                lbWatchFolders.Items.Clear();
                lbWatchFolders.DataSource = new BindingSource(WatchFolders, null);
                lbWatchFolders.DisplayMember = "Path";
                lbWatchFolders.ValueMember = "Id";               
            }
        }
    }
}
