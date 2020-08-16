using System;
using System.IO;
using System.Threading;

namespace JBToolkit.SafeStream
{

    /// <summary>
    /// This is a wrapper around a FileStream.  While it is not a Stream itself, it can be cast to
    /// one (keep in mind that this might throw an exception). It's a mechanism for creating a stream from a file without the issue if it still
    /// being used by the calling process later when you want to access it again.
    /// </summary>
#pragma warning disable CA1063 // Implement IDisposable Correctly
    public class SafeFileStream : IDisposable
#pragma warning restore CA1063 // Implement IDisposable Correctly
    {
        #region Private Members

        private Mutex m_mutex;
        private Stream m_stream;
        private readonly string m_path;
        private readonly FileMode m_fileMode;
        private readonly FileAccess m_fileAccess;
        private readonly FileShare m_fileShare;

        #endregion//Private Members

        #region Constructors
        public SafeFileStream(string path, FileMode mode, FileAccess access, FileShare share)
        {
            m_mutex = new Mutex(false, String.Format("Global\\{0}", path.Replace('\\', '/')));
            m_path = path;
            m_fileMode = mode;
            m_fileAccess = access;
            m_fileShare = share;
        }
        #endregion//Constructors

        #region Properties
        public Stream UnderlyingStream
        {
            get
            {
                if (!IsOpen)
                {
                    throw new InvalidOperationException("The underlying stream does not exist - try opening this stream.");
                }

                return m_stream;
            }
        }

        public bool IsOpen
        {
            get { return m_stream != null; }
        }
        #endregion//Properties

        #region Functions
        /// <summary>
        /// Opens the stream when it is not locked.  If the file is locked, then
        /// </summary>
        public void Open()
        {
            if (m_stream != null)
            {
                throw new InvalidOperationException("File Open");
            }

            m_mutex.WaitOne();
            m_stream = File.Open(m_path, m_fileMode, m_fileAccess, m_fileShare);
        }

        public bool TryOpen(TimeSpan span)
        {
            if (m_stream != null)
            {
                throw new InvalidOperationException("File Open");
            }

            if (m_mutex.WaitOne(span))
            {
                m_stream = File.Open(m_path, m_fileMode, m_fileAccess, m_fileShare);
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Close()
        {
            if (m_stream != null)
            {
                m_stream.Close();
                m_stream = null;
                m_mutex.ReleaseMutex();
            }
        }

#pragma warning disable CA1063 // Implement IDisposable Correctly
        public void Dispose()
#pragma warning restore CA1063 // Implement IDisposable Correctly
        {
            try
            {
                if (m_mutex != null)
                    m_mutex.Dispose();
            }
            catch { }

            Close();
            GC.SuppressFinalize(this);
        }

        public static explicit operator Stream(SafeFileStream sfs)
        {
            return sfs.UnderlyingStream;
        }

        /// <summary>
        /// Read text file and return lines without locking it. If it is locked it should read anyway
        /// </summary>
        public static string[] ReadAllLines(string filePath)
        {
            string[] lines = null;

            var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using (var sr = new StreamReader(fs))
            {
                string s = sr.ReadToEnd().Replace('\r', '\n').Replace("\n\n", "\n");
                lines = s.Split('\n');
                for (int i = 0; i < lines.Length; i++)
                {
                    lines[i] = lines[i].Trim();
                }
            }

            return lines;
        }

        /// <summary>
        /// Read text file without locking it. If it is locked it should read anyway
        /// </summary>
        public static string ReadAllText(string filePath)
        {
            var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using (var sr = new StreamReader(fs))
            {
                return sr.ReadToEnd();
            }
        }

        /// <summary>
        /// Get bytes of file without locking it. If it is locked it should read anyway
        /// </summary>
        public static byte[] GetBytes(string filePath)
        {
            var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            byte[] fileBytes = new byte[fs.Length];

            fs.Read(fileBytes, 0, fileBytes.Length);
            fs.Close();

            return fileBytes;
        }

        #endregion//Functions
    }
}