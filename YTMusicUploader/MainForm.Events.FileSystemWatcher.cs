using JBToolkit.Threads;
using System;
using System.IO;
using System.Threading;
using static YTMusicUploader.Helpers.FileSystemWatcherPoller;

namespace YTMusicUploader
{
    public partial class MainForm
    {
        private void FolderWatcher_OnPollDiscoveredChange(object source, PollDiscoveredChangeArgs e)
        {
            try
            {
                if (LastFolderChangeTime == null)
                {
                    PausePolling();
                    Logger.LogInfo("FolderWatcher_OnPollDiscoveredChange", "Library file change detected: Starting scan process");

                    LastFolderChangeTime = DateTime.Now;
                    FlagStartQueue();
                }

                LastFolderChangeTime = DateTime.Now;
            }
            catch { }
        }

        private void FolderWatcher_OnChanged(object source, FileSystemEventArgs e)
        {
            try
            {
                if (LastFolderChangeTime == null)
                {
                    PausePolling();
                    Logger.LogInfo("FolderWatcher_OnChanged", "Library file change detected: Starting scan process");

                    LastFolderChangeTime = DateTime.Now;
                    FlagStartQueue();            
                }

                LastFolderChangeTime = DateTime.Now;
            }
            catch { }
        }

        private void FolderWatcher_OnRenamed(object source, RenamedEventArgs e)
        {
            try
            {
                if (LastFolderChangeTime == null)
                {
                    PausePolling();
                    Logger.LogInfo("FolderWatcher_OnRenamed", "Library file change detected: Starting scan process");

                    LastFolderChangeTime = DateTime.Now;
                    FlagStartQueue();
                }

                LastFolderChangeTime = DateTime.Now;
            }
            catch { }
        }

        private void FlagStartQueue()
        {
            try
            {
                ThreadPool.QueueUserWorkItem(delegate
                {
                    while (LastFolderChangeTime > DateTime.Now.AddSeconds(-5))
                        ThreadHelper.SafeSleep(1000);

                    while (Paused)
                        Thread.Sleep(1500);

                    LastFolderChangeTime = null;
                    FileScanner.RecountLibraryFiles();
                    QueueChecker.Queue = true;
                    ResumePolling();
                });
            }
            catch { }
        }

        private void PausePolling()
        {
            foreach (var watcher in FileSystemFolderWatchers)
                watcher.Stop();
        }

        private void ResumePolling()
        {
            foreach (var watcher in FileSystemFolderWatchers)
                watcher.Start();
        }
    }
}
