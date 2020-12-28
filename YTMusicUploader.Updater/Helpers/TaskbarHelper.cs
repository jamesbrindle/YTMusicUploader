namespace YTMusicUploader.Updater
{
    public class TaskbarHelper
    {
        public static void SetTaskbarProgress(int current, int total)
        {
            Microsoft.WindowsAPICodePack.Taskbar.TaskbarManager.Instance.SetProgressValue(current, total);
        }
    }
}
