using JBToolkit.Threads;
using System.Threading;

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
            Stopped = false;

            QueueCheckerThread = new Thread((ThreadStart)delegate
            {
                while (true)
                {
                    try
                    {
                        if (MainForm.Aborting)
                        {
                            Stopped = true;
                            return;
                        }

                        ThreadHelper.SafeSleep(1000);
                        if (Queue)
                        {
                            while (!MainForm.FileUploader.Stopped || !MainForm.PlaylistProcessor.Stopped)
                                ThreadHelper.SafeSleep(1000);

                            MainForm.Restart();
                        }
                    }
                    catch
                    {
                        ThreadHelper.SafeSleep(1000);
                    }
                }
            })
            {
                IsBackground = true,
                Priority = ThreadPriority.Lowest
            };

            QueueCheckerThread.Start();
        }
    }
}
