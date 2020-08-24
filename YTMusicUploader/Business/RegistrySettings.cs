using Microsoft.Win32;
using System.Windows.Forms;

namespace YTMusicUploader.Business
{
    /// <summary>
    /// Windows registry helper methods
    /// </summary>
    public class RegistrySettings
    {
        /// <summary>
        /// For setting whether the app starts with Windows or not.
        /// Uses the 'CurrentVersion\Run' sub key in the users key
        /// </summary>
        /// <param name="startWithWindows">Set to start with windows if true, false otherwise</param>
        public static void SetStartWithWindows(bool startWithWindows)
        {
            if (startWithWindows)
            {
                var path = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
                RegistryKey key = Registry.CurrentUser.OpenSubKey(path, true);
                key.SetValue("YT Music Uploader", "\"" + Application.ExecutablePath.ToString() + "\" -hidden");
            }
            else
            {
                var path = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
                RegistryKey key = Registry.CurrentUser.OpenSubKey(path, true);
                key.DeleteValue("YT Music Uploader", false);
            }
        }
    }
}
