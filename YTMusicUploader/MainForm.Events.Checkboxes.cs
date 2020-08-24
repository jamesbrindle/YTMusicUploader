using System;
using YTMusicUploader.Business;

namespace YTMusicUploader
{
    public partial class MainForm
    {
        private void CbStartWithWindows_CheckedChanged(object sender, EventArgs e)
        {
            Settings.StartWithWindows = cbStartWithWindows.Checked;
            Settings.Save();
            RegistrySettings.SetStartWithWindows(cbStartWithWindows.Checked);
        }
    }
}
