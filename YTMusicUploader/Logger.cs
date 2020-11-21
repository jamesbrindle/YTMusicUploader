using JBToolkit.Network;
using JBToolkit.Threads;
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
        private static SettingsRepo m_settingsRepo = null;
        private static string m_externalIp = string.Empty;
        private static string m_machineName = string.Empty;
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

        private static SettingsRepo SettingsRepo
        {
            get
            {
                try
                {
                    if (m_settingsRepo == null)
                        m_settingsRepo = new SettingsRepo();
                }
                catch { }

                return m_settingsRepo;
            }
        }

        private static string ExternalIp
        {
            get
            {
                try
                {
                    if (string.IsNullOrEmpty(m_externalIp))
                        m_externalIp = NetworkHelper.GetExternalIPAddress();
                }
                catch { }

                return m_externalIp;
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
        private static bool SendLogToSource
        {
            get
            {
                try
                {
                    return SettingsRepo.Load().Result.SendLogsToSource;
                }
                catch
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Log error from exception
        /// </summary>
        /// <param name="e">Exception to log</param>
        public static void Log(Exception e)
        {
            try
            {
                ThreadPool.QueueUserWorkItem(delegate
                {
                    var log = new Log
                    {
                        Event = DateTime.Now,
                        LogTypeId = LogTypeEnum.Error,
                        Source = new StackTrace(e).GetFrame(0).GetMethod().Name.Ellipse(200 - 5),
                        Message = e.Message.Ellipse(1500 - 5),
                        StackTrace = e.StackTrace.Ellipse(4000 - 5),
                        Machine = MachineName.Ellipse(210)
                    };

                    Task.Run(async () => await LogsRepo.Add(log));
                    if (SendLogToSource)
                    {
                        log.Machine += $"@{ExternalIp}";
                        LogsRepo.RemoteAdd(log);
                    }
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
            try
            {
                ThreadPool.QueueUserWorkItem(delegate
                {
                    var log = new Log
                    {
                        Event = DateTime.Now,
                        LogTypeId = logType,
                        Source = new StackTrace(e).GetFrame(0).GetMethod().Name.Ellipse(200 - 5),
                        Message = e.Message.Ellipse(1500 - 5),
                        StackTrace = e.StackTrace.Ellipse(4000 - 5),
                        Machine = MachineName.Ellipse(210)
                    };

                    Task.Run(async () => await LogsRepo.Add(log));
                    if (SendLogToSource)
                    {
                        log.Machine += $"@{ExternalIp}";
                        LogsRepo.RemoteAdd(log);
                    }
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
            try
            {
                ThreadPool.QueueUserWorkItem(delegate
                {
                    var log = new Log
                    {
                        Event = DateTime.Now,
                        LogTypeId = LogTypeEnum.Error,
                        Source = new StackTrace(e).GetFrame(0).GetMethod().Name.Ellipse(200 - 5),
                        Message = (additionalMessage + ": " + e.Message).Ellipse(1500 - 5),
                        StackTrace = e.StackTrace.Ellipse(4000 - 5),
                        Machine = MachineName.Ellipse(210)
                    };

                    Task.Run(async () => await LogsRepo.Add(log));
                    if (SendLogToSource)
                    {
                        log.Machine += $"@{ExternalIp}";
                        LogsRepo.RemoteAdd(log);
                    }

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
            try
            {
                ThreadPool.QueueUserWorkItem(delegate
                {
                    var log = new Log
                    {
                        Event = DateTime.Now,
                        LogTypeId = logType,
                        Source = new StackTrace(e).GetFrame(0).GetMethod().Name.Ellipse(200 - 5),
                        Message = (additionalMessage + ": " + e.Message).Ellipse(1500 - 5),
                        StackTrace = e.StackTrace.Ellipse(4000 - 5),
                        Machine = MachineName.Ellipse(210)
                    };

                    Task.Run(async () => await LogsRepo.Add(log));
                    if (SendLogToSource)
                    {
                        log.Machine += $"@{ExternalIp}";
                        LogsRepo.RemoteAdd(log);
                    }

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
            try
            {
                ThreadPool.QueueUserWorkItem(delegate
                {
                    var log = new Log
                    {
                        Event = DateTime.Now,
                        LogTypeId = logType,
                        Source = source,
                        Message = message.Ellipse(1500 - 5),
                        StackTrace = null,
                        Machine = MachineName.Ellipse(210)
                    };

                    Task.Run(async () => await LogsRepo.Add(log));
                    if (SendLogToSource && logType != LogTypeEnum.Info)
                    {
                        log.Machine += $"@{ExternalIp}";
                        LogsRepo.RemoteAdd(log);
                    }

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
                        Source = source,
                        Message = message.Ellipse(1500 - 5),
                        StackTrace = null,
                        Machine = MachineName.Ellipse(210)
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
        public static void LogError(string source, string message)
        {
            try
            {
                ThreadPool.QueueUserWorkItem(delegate
                {
                    var log = new Log
                    {
                        Event = DateTime.Now,
                        LogTypeId = LogTypeEnum.Error,
                        Source = source,
                        Message = message.Ellipse(1500 - 5),
                        StackTrace = null,
                        Machine = MachineName.Ellipse(210)
                    };

                    Task.Run(async () => await LogsRepo.Add(log));
                    if (SendLogToSource)
                    {
                        log.Machine += $"@{ExternalIp}";
                        LogsRepo.RemoteAdd(log);
                    }

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
                        Source = source,
                        Message = message.Ellipse(1500 - 5),
                        StackTrace = null,
                        Machine = MachineName.Ellipse(210)
                    };

                    Task.Run(async () => await LogsRepo.Add(log));
                    if (SendLogToSource)
                    {
                        log.Machine += $"@{ExternalIp}";
                        LogsRepo.RemoteAdd(log);
                    }

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
                        LogTypeId = LogTypeEnum.Critcal,
                        Source = source,
                        Message = message.Ellipse(1500 - 5),
                        StackTrace = null,
                        Machine = MachineName.Ellipse(210)
                    };

                    Task.Run(async () => await LogsRepo.Add(log));
                    if (SendLogToSource)
                    {
                        log.Machine += $"@{ExternalIp}";
                        LogsRepo.RemoteAdd(log);
                    }

                });
            }
            catch { }
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
