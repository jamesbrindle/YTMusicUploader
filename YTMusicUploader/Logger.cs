using JBToolkit.Assemblies;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using YTMusicUploader.Providers.DataModels;
using YTMusicUploader.Providers.Repos;
using static YTMusicUploader.Providers.DataModels.Log;

namespace YTMusicUploader
{
    /// <summary>
    /// Logs messages to the database (info or error)
    /// </summary>
    public static class Logger
    {
        private static LogsRepo m_logsRepo = null;
        private static string m_machineName = string.Empty;
        public static DateTime? AllowRemoteLogAt { get; set; } = null;
        public static bool DontLogToSourdceCauseEarlierVersion { get; set; } = false;

        private static LogsRepo LogsRepo
        {
            get
            {
                try
                {
                    if (m_logsRepo == null)
                        m_logsRepo = new LogsRepo();
                }
                catch { }

                return m_logsRepo;
            }
        }

        private static string MachineName
        {
            get
            {
                try
                {
                    if (string.IsNullOrEmpty(m_machineName))
                        m_machineName = Environment.MachineName;
                }
                catch { }

                return m_machineName;
            }
        }

        /// <summary>
        /// Log error from exception
        /// </summary>
        /// <param name="e">Exception to log</param>
        public static void Log(Exception e)
        {
            if (IsInIgnoreList(GetExceptionMessage(e)))
                return;

            try
            {
                ThreadPool.QueueUserWorkItem(delegate
                {
                    var log = new Log
                    {
                        Event = DateTime.Now,
                        LogTypeId = LogTypeEnum.Error,
                        Source = GetExceptionSource(e).Ellipse(200 - 5),
                        Message = GetExceptionMessage(e).Ellipse(1500 - 5),
                        StackTrace = GetExceptionStackTrace(e).Ellipse(4000 - 5),
                        Machine = MachineName.Ellipse(210),
                        Version = VersionHelper.GetVersionFull()
                    };

                    Task.Run(async () => await LogsRepo.Add(log));
                });
            }
            catch { }
        }

        /// <summary>
        /// Log error from exception with error severity
        /// </summary>
        /// <param name="e">Exception to log</param>
        public static void Log(Exception e, LogTypeEnum logType)
        {
            if (IsInIgnoreList(GetExceptionMessage(e)))
                return;

            try
            {
                ThreadPool.QueueUserWorkItem(delegate
                {
                    var log = new Log
                    {
                        Event = DateTime.Now,
                        LogTypeId = logType,
                        Source = GetExceptionSource(e).Ellipse(200 - 5),
                        Message = GetExceptionMessage(e).Ellipse(1500 - 5),
                        StackTrace = GetExceptionStackTrace(e).Ellipse(4000 - 5),
                        Machine = MachineName.Ellipse(210),
                        Version = VersionHelper.GetVersionFull()
                    };

                    Task.Run(async () => await LogsRepo.Add(log));
                });
            }
            catch { }
        }

        /// <summary>
        /// Log error from exception with additional messag prefix
        /// </summary>
        /// <param name="e">Exception to log</param>
        public static void Log(Exception e, string additionalMessage)
        {
            if (IsInIgnoreList(GetExceptionMessage(e)))
                return;

            try
            {
                ThreadPool.QueueUserWorkItem(delegate
                {
                    var log = new Log
                    {
                        Event = DateTime.Now,
                        LogTypeId = LogTypeEnum.Error,
                        Source = GetExceptionSource(e).Ellipse(200 - 5),
                        Message = (additionalMessage + ": " + GetExceptionMessage(e)).Ellipse(1500 - 5),
                        StackTrace = GetExceptionStackTrace(e).Ellipse(4000 - 5),
                        Machine = MachineName.Ellipse(210),
                        Version = VersionHelper.GetVersionFull()
                    };

                    Task.Run(async () => await LogsRepo.Add(log));
                });
            }
            catch { }
        }

        /// <summary>
        /// Log error from exception with error severity
        /// </summary>
        /// <param name="e">Exception to log</param>
        public static void Log(Exception e, string additionalMessage, LogTypeEnum logType)
        {
            if (IsInIgnoreList(GetExceptionMessage(e)))
                return;

            try
            {
                ThreadPool.QueueUserWorkItem(delegate
                {
                    var log = new Log
                    {
                        Event = DateTime.Now,
                        LogTypeId = logType,
                        Source = GetExceptionSource(e).Ellipse(200 - 5),
                        Message = (additionalMessage + ": " + GetExceptionMessage(e)).Ellipse(1500 - 5),
                        StackTrace = GetExceptionStackTrace(e).Ellipse(4000 - 5),
                        Machine = MachineName.Ellipse(210),
                        Version = VersionHelper.GetVersionFull()
                    };

                    Task.Run(async () => await LogsRepo.Add(log));
                });
            }
            catch { }
        }


        /// <summary>
        /// Log custom message
        /// </summary>
        /// <param name="logType">Error or info</param>
        /// <param name="source">Where the message originated</param>
        /// <param name="message">The messsage to log</param>
        public static void Log(LogTypeEnum logType, string source, string message)
        {
            if (IsInIgnoreList(message))
                return;

            try
            {
                ThreadPool.QueueUserWorkItem(delegate
                {
                    var log = new Log
                    {
                        Event = DateTime.Now,
                        LogTypeId = logType,
                        Source = source.Ellipse(200 - 5),
                        Message = message.Ellipse(1500 - 5),
                        StackTrace = null,
                        Machine = MachineName.Ellipse(210),
                        Version = VersionHelper.GetVersionFull()
                    };

                    Task.Run(async () => await LogsRepo.Add(log));
                });
            }
            catch { }
        }

        /// <summary>
        /// Log custom info message
        /// </summary>
        /// <param name="source">Where the message originated</param>
        /// <param name="message">The messsage to log</param>
        public static void LogInfo(string source, string message)
        {
            try
            {
                ThreadPool.QueueUserWorkItem(delegate
                {
                    var log = new Log
                    {
                        Event = DateTime.Now,
                        LogTypeId = LogTypeEnum.Info,
                        Source = source.Ellipse(200 - 5),
                        Message = message.Ellipse(1500 - 5),
                        StackTrace = null,
                        Machine = MachineName.Ellipse(210),
                        Version = VersionHelper.GetVersionFull()
                    };

                    Task.Run(async () => await LogsRepo.Add(log));
                });
            }
            catch { }
        }

        /// <summary>
        /// Log custom error message
        /// </summary>
        /// <param name="source">Where the message originated</param>
        /// <param name="message">The messsage to log</param>
        public static void LogError(string source, string message, string stackTrace = null)
        {
            if (IsInIgnoreList(message))
                return;

            try
            {
                ThreadPool.QueueUserWorkItem(delegate
                {
                    var log = new Log
                    {
                        Event = DateTime.Now,
                        LogTypeId = LogTypeEnum.Error,
                        Source = source.Ellipse(200 - 5),
                        Message = message.Ellipse(1500 - 5),
                        StackTrace = stackTrace,
                        Machine = MachineName.Ellipse(210),
                        Version = VersionHelper.GetVersionFull()
                    };

                    Task.Run(async () => await LogsRepo.Add(log));
                });
            }
            catch { }
        }

        /// <summary>
        /// Log custom info message
        /// </summary>
        /// <param name="source">Where the message originated</param>
        /// <param name="message">The messsage to log</param>
        public static void LogWarning(string source, string message)
        {
            try
            {
                ThreadPool.QueueUserWorkItem(delegate
                {
                    var log = new Log
                    {
                        Event = DateTime.Now,
                        LogTypeId = LogTypeEnum.Warning,
                        Source = source.Ellipse(200 - 5),
                        Message = message.Ellipse(1500 - 5),
                        StackTrace = null,
                        Machine = MachineName.Ellipse(210),
                        Version = VersionHelper.GetVersionFull()
                    };

                    Task.Run(async () => await LogsRepo.Add(log));
                });
            }
            catch { }
        }

        /// <summary>
        /// Log custom info message
        /// </summary>
        /// <param name="source">Where the message originated</param>
        /// <param name="message">The messsage to log</param>
        public static void LogCritial(string source, string message)
        {
            try
            {
                ThreadPool.QueueUserWorkItem(delegate
                {
                    var log = new Log
                    {
                        Event = DateTime.Now,
                        LogTypeId = LogTypeEnum.Critical,
                        Source = source.Ellipse(200 - 5),
                        Message = message.Ellipse(1500 - 5),
                        StackTrace = null,
                        Machine = MachineName.Ellipse(210),
                        Version = VersionHelper.GetVersionFull()
                    };

                    Task.Run(async () => await LogsRepo.Add(log));
                });
            }
            catch { }
        }

        private static string GetExceptionSource(Exception e)
        {
            if (e.Message.ToLower().Contains("one or more errors occurred"))
                return new StackTrace(e.InnerException).GetFrame(0).GetMethod().Name;

            return new StackTrace(e).GetFrame(0).GetMethod().Name;
        }

        private static string GetExceptionMessage(Exception e)
        {
            if (e.Message.ToLower().Contains("one or more errors occurred"))
                return e.InnerException.Message;

            return e.Message;
        }

        private static string GetExceptionStackTrace(Exception e)
        {
            if (e.Message.ToLower().Contains("one or more errors occurred"))
                return e.InnerException.StackTrace;

            return e.StackTrace;
        }

        private static bool IsInIgnoreList(string exceptionMessage)
        {
            if (exceptionMessage.Contains("Thread was being aborted"))
                return true;
            return false;
        }

        /// <summary>
        /// Clears historic logs early than the amount of days as defined in the App.config (default 30 days).
        /// </summary>
        public static void ClearHistoricLogs()
        {
            try
            {
                Task.Run(async () => await LogsRepo.DeleteOlderThan(DateTime.Now.AddDays(Global.ClearLogsAfterDays * -1)));
            }
            catch { }
        }
    }
}
