namespace YTMusicUploader.Updater
{
    /// <summary>
    /// Windows 7+ Taskbar helper
    /// </summary>
    public class TaskbarHelper
    {
        /// <summary>
        /// Sets the progess on the taskbar icon (green overlay indicating progress)
        /// </summary>
        /// <param name="current">0 - 100 (current progress)</param>
        /// <param name="total">i.e. 100 - Total progres possible</param>
        public static void SetTaskbarProgress(int current, int total)
        {
            Microsoft.WindowsAPICodePack.Taskbar.TaskbarManager.Instance.SetProgressValue(current, total);
        }
    }
}
