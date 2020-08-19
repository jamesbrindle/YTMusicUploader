using System;
using System.IO;
using System.Threading;
using System.Timers;

namespace JBToolkit.StreamHelpers
{
    /// <summary>
    /// Throttle a stream -> Very useful for throttle HttpWebRequest uploads / downloads.
    /// 
    /// Thanks to: 0xDEADBEEF:
    ///     https://stackoverflow.com/users/909365/0xdeadbeef
    /// 
    /// Usage:
    /// 
    ///    var stream = request.GetRequestStream();
    ///    var throttledStream = new ThrottledStream(new MemoryStream(fileBytes), uploadSpeedInBytesPerSecond);
    ///    throttledStream.CopyTo(stream);
    ///    stream.Close();
    /// 
    /// </summary>
    public class ThrottledStream : Stream
    {
        #region Properties

        private int maxBytesPerSecond;

        /// <summary>
        /// Number of Bytes that are allowed per second
        /// </summary>
        public int MaxBytesPerSecond
        {
            get { return maxBytesPerSecond; }
            set
            {
                if (value < 1)
                    throw new ArgumentException("MaxBytesPerSecond has to be > 0");

                maxBytesPerSecond = value;
            }
        }

        #endregion

        #region Private Members

        private int processed;
        System.Timers.Timer resettimer;
        AutoResetEvent wh = new AutoResetEvent(true);
        private Stream parent;

        #endregion

        /// <summary>
        /// Creates a new Stream with Databandwith cap
        /// </summary>
        /// <param name="parentStream"></param>
        /// <param name="maxBytesPerSecond"></param>
        public ThrottledStream(Stream parentStream, int maxBytesPerSecond = int.MaxValue)
        {
            MaxBytesPerSecond = maxBytesPerSecond;
            parent = parentStream;
            processed = 0;
            resettimer = new System.Timers.Timer
            {
                Interval = 1000
            };
            resettimer.Elapsed += Resettimer_Elapsed;
            resettimer.Start();
        }

        protected void Throttle(int bytes)
        {
            try
            {
                processed += bytes;
                if (processed >= maxBytesPerSecond)
                    wh.WaitOne();
            }
            catch
            { }
        }
        private void Resettimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            processed = 0;
            wh.Set();
        }

        #region Stream-Overrides

        public override void Close()
        {
            resettimer.Stop();
            resettimer.Close();
            base.Close();
        }
        protected override void Dispose(bool disposing)
        {
            resettimer.Dispose();
            base.Dispose(disposing);
        }

        public override bool CanRead
        {
            get { return parent.CanRead; }
        }

        public override bool CanSeek
        {
            get { return parent.CanSeek; }
        }

        public override bool CanWrite
        {
            get { return parent.CanWrite; }
        }

        public override void Flush()
        {
            parent.Flush();
        }

        public override long Length
        {
            get { return parent.Length; }
        }

        public override long Position
        {
            get
            {
                return parent.Position;
            }
            set
            {
                parent.Position = value;
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            Throttle(count);
            return parent.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return parent.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            parent.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            Throttle(count);
            parent.Write(buffer, offset, count);
        }

        #endregion
    }
}
