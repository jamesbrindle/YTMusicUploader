﻿using JBToolkit.Threads;
using System.Threading;
using YTMusicUploader.Providers;

namespace YTMusicUploader.Business
{
    /// <summary>
    /// Constantly watches for the folder watcher state changed variable (Queue) in order to queue a new process
    /// when the music library changes (file added, deleted etc)
    /// </summary>
    public class QueueChecker
    {
        private MainForm MainForm { get; set; }
        public bool Queue { get; set; } = false;
        public bool Stopped { get; set; } = false;
        public Thread QueueCheckerThread { get; set; }

        public QueueChecker(MainForm mainForm)
        {
            MainForm = mainForm;

            QueueCheckerThread = new Thread((ThreadStart)delegate
            {
                while (MainForm.InstallingEdge)
                    ThreadHelper.SafeSleep(200);

                while (true)
                {
                    try
                    {
                        if (MainForm.Aborting || Stopped)
                            return;

                        ThreadHelper.SafeSleep(1000);
                        if (Queue)
                        {
                            while (!MainForm.FileUploader.Stopped)
                                ThreadHelper.SafeSleep(1000);

                            Queue = false;
                            MainForm.Aborting = false;
                            Requests.UploadCheckCache.CleanUp = true;
                            MainForm.FileUploader.Stopped = true;
                            MainForm.FileScanner.Reset();
                            MainForm.StartMainProcess();
                        }
                    }
                    catch { }
                }
            })
            {
                IsBackground = true
            };
            QueueCheckerThread.Start();
        }
    }
}