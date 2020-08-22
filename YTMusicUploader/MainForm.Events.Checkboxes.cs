using System;
using YTMusicUploader.Helpers;

namespace YTMusicUploader
{
    public partial class MainForm
    {
        private void CbStartWithWindows_CheckedChanged(object sender, EventArgs e)
        {
            Settings.StartWithWindows = cbStartWithWindows.Checked;
            Settings.Save();
            RegistryHelper.SetStartWithWindows(cbStartWithWindows.Checked);
        }
    }
}
