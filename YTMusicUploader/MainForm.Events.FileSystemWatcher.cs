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

        private void FlagStartQueue()
        {
            new Thread((ThreadStart)delegate
            {
                while (LastFolderChangeTime > DateTime.Now.AddSeconds(-10))
                {
                    try
                    {
                        Thread.Sleep(1000);
                    }
                    catch { }
                }

                LastFolderChangeTime = null;
                FileScanner.RecountLibraryFiles();
                Queue = true;

            }).Start();
        }
    }
}
