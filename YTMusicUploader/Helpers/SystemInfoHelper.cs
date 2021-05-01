using System;

namespace YTMusicUploader.Helpers
{
    /// <summary>
    /// Get basic system info - To determine multi-threading settings to minimise resource hogging
    /// </summary>
    public class SystemInfoHelper
    {
        /// <summary>
        /// Get basic system info - To determine multi-threading settings to minimise resource hogging
        /// </summary>
        public static SystemInfo GetSystemInfo()
        {
            return new SystemInfo
            {
                CpuCores = Environment.ProcessorCount
            };
        }
    }

    /// <summary>
    /// Basic system info - To determine multi-threading settings to minimise resource hogging
    /// </summary>
    public class SystemInfo
    {
        public int CpuCores { get; set; }
    }
}
