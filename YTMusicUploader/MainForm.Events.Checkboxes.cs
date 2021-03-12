using System;
using System.Threading.Tasks;
using YTMusicUploader.Business;

namespace YTMusicUploader
{
    public partial class MainForm
    {
        private void CbStartWithWindows_CheckedChanged(object sender, EventArgs e)
        {
            Settings.StartWithWindows = cbStartWithWindows.Checked;
            Task.Run(async () => await Settings.Save());
            Task.Run(async () => await RegistrySettings.SetStartWithWindows(cbStartWithWindows.Checked));
        }

        private void CbSendErrorLogsToSource_CheckedChanged(object sender, EventArgs e)
        {
            Settings.SendLogsToSource = cbSendErrorLogsToSource.Checked;
            Task.Run(async () => await Settings.Save());
        }

        private void CbAlsoUploadPlaylists_CheckedChanged(object sender, EventArgs e)
        {
            Settings.UploadPlaylists = cbAlsoUploadPlaylists.Checked;
            Task.Run(async () => await Settings.Save());
        }
    }
}
