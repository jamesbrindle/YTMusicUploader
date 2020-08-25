using System.Threading;

namespace JBToolkit.Threads
{
    /// <summary>
    /// Thread helper methods
    /// </summary>
    public static class ThreadHelper
    {
        /// <summary>
        /// Wraps Thread.Sleep in a try catch... Should only throw an exception when existing the application
        /// And another thread is running.
        /// </summary>
        /// <param name="milliseconds">Time to wait</param>
        public static void SafeSleep(int milliseconds)
        {
            try
            {
                Thread.Sleep(milliseconds);
            }
            catch { }
        }
    }
}
