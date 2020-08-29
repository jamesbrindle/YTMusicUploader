using JBToolkit.Threads;
using System;
using System.IO;
using System.Threading;

namespace YTMusicUploader
{
    public partial class MainForm
    {
        private void FolderWatcher_OnChanged(object source, FileSystemEventArgs e)
        {
            if (LastFolderChangeTime == null)
            {
                LastFolderChangeTime = DateTime.Now;
                FlagStartQueue();
            }

            LastFolderChangeTime = DateTime.Now;
        }

        private void FolderWatcher_OnRenamed(object source, RenamedEventArgs e)
        {
            if (LastFolderChangeTime == null)
            {
                LastFolderChangeTime = DateTime.Now;
                FlagStartQueue();
            }

            LastFolderChangeTime = DateTime.Now;
        }

        private void FlagStartQueue()
        {
            new Thread((ThreadStart)delegate
            {
                while (LastFolderChangeTime > DateTime.Now.AddSeconds(-10))
                    ThreadHelper.SafeSleep(1000);

                LastFolderChangeTime = null;
                FileScanner.RecountLibraryFiles();
                QueueChecker.Queue = true;

            }).Start();
        }
    }
}
