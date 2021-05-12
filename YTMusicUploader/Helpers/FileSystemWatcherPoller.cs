using JBToolkit.Windows;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Timers;

namespace YTMusicUploader.Helpers
{
    public class FileSystemWatcherPoller : FileSystemWatcher
    {
        public int PollFrequency { get; set; } = 3600000; // Default: 1 Hour        
        private FileData[] FileData { get; set; }
        private Timer Timer = new Timer();

        public delegate void PollDiscoveredChangeHander(object objects, PollDiscoveredChangeArgs args);
        public event PollDiscoveredChangeHander OnPollDiscoveredChange;

        public FileSystemWatcherPoller()
        {
            Timer.Interval = PollFrequency;
            Timer.AutoReset = true;
            Timer.Enabled = true;
            Timer.Elapsed += OnTimedEvent;
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            var compareData = FastDirectoryEnumerator.EnumerateFiles(
                Path,
                Filter,
                this.IncludeSubdirectories
                    ? SearchOption.AllDirectories
                    : SearchOption.TopDirectoryOnly).ToArray();

            bool equal = FileData.Count() == compareData.Count() &&
                         !FileData.Except(compareData, new FileDataComparer()).Any() && 
                         !compareData.Except(FileData, new FileDataComparer()).Any();
            if (!equal)
            {
                var ags = new PollDiscoveredChangeArgs("File structure changed");
                OnPollDiscoveredChange(this, ags);
            }
        }

        public class FileDataComparer : IEqualityComparer<FileData>
        {
            bool IEqualityComparer<FileData>.Equals(FileData x, FileData y)
            {
                return x.CreationTime.Equals(y.CreationTime) && 
                       x.LastWriteTime.Equals(y.LastWriteTime) &&
                       x.Size.Equals(y.Size) &&
                       x.Path.Equals(y.Path);
            }

            int IEqualityComparer<FileData>.GetHashCode(FileData obj)
            {
                if (obj is null)
                    return 0;

                return obj.Path.GetHashCode() + 
                       obj.Size.GetHashCode() + 
                       obj.CreationTime.GetHashCode() + 
                       obj.LastWriteTime.GetHashCode();
            }
        }

        public void Start()
        {
            EnableRaisingEvents = true;
            FileData = FastDirectoryEnumerator.EnumerateFiles(
                Path,
                Filter,
                this.IncludeSubdirectories
                    ? SearchOption.AllDirectories
                    : SearchOption.TopDirectoryOnly).ToArray();

            Timer.Start();
        }

        public void Stop()
        {
            Timer.Stop();
            EnableRaisingEvents = false;
        }

        public class PollDiscoveredChangeArgs : EventArgs
        {
            private string message;

            public PollDiscoveredChangeArgs(string message)
            {
                this.message = message;
            }

            public string Message
            {
                get
                {
                    return message;
                }
            }
        }
    }
}
