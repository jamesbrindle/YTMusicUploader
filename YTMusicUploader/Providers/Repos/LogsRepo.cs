using Dapper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using YTMusicUploader.Providers.DataModels;

namespace YTMusicUploader.Providers.Repos
{
    /// <summary>
    /// Log events repository access
    /// </summary>
    public class LogsRepo : DataAccess
    {
        /// <summary>
        /// Loads all logs from the databse
        /// </summary>
        /// <returns>List of MusicFileObjects</returns>
        public Task<List<Log>> LoadAll()
        {
            using (var conn = DbConnection(true))
            {
                string cmd = string.Format(
                                @"SELECT * 
                                  FROM Logs
                                  ORDER BY Id DESC");

                conn.Open();
                var logs = conn.Query<Log>(cmd).ToList();
                conn.Close();

                return Task.FromResult(logs);               
            }
        }

        /// <summary>
        /// Loads specific logs from the databse
        /// </summary>
        /// <returns>List of MusicFileObjects</returns>
        public Task<List<Log>> LoadSpecific(string logTypes)
        {
            using (var conn = DbConnection(true))
            {
                string cmd = string.Format(
                                @"SELECT * 
                                  FROM Logs
                                  WHERE LogTypeId IN ({0})
                                  ORDER BY Id DESC", logTypes);

                conn.Open();
                var logs = conn.Query<Log>(cmd).ToList();
                conn.Close();

                return Task.FromResult(logs);                
            }
        }

        /// <summary>
        /// Adds the log to the databse
        /// </summary>
        /// <param name="log">Log object to record</param>
        /// <returns>DbOperationResult - Showing success or fail, with messages and stats</returns>
        public async Task<DbOperationResult> Add(Log log)
        {
            try
            {
                using (var conn = DbConnection())
                {
                    conn.Open();
                    conn.Execute(
                        @"INSERT 
                                INTO Logs (
                                        Event, 
                                        LogTypeId,
                                        Machine,
                                        Source,
                                        Message, 
                                        Version,
                                        StackTrace) 
                                VALUES (@Event,
                                        @LogTypeId,
                                        @Machine,
                                        @Source,
                                        @Message,
                                        @Version,
                                        @StackTrace);
                              SELECT last_insert_rowid()",
                        log);
                    conn.Close();
                }

                return await Task.FromResult(DbOperationResult.Success(-1, new TimeSpan(0)));
            }
            catch (Exception e)
            {
                return await Task.FromResult(DbOperationResult.Fail(e.Message, new TimeSpan(0)));
            }
        }

        /// <summary>
        /// Sends the log off to the source for diagnostic data
        /// </summary>
        /// <param name="log">Log object to record</param>
        internal void RemoteAdd(Log log)
        {
            try
            {
                if (!Logger.AllowRemoteLogAt.HasValue || 
                    Logger.AllowRemoteLogAt < DateTime.Now)
                {
                    using (var conn = RemoteDbConnection())
                    {
                        conn.Open();
                        conn.Execute(
                            @"INSERT 
                                INTO Logs (
                                        Event, 
                                        LogTypeId,
                                        Machine,
                                        Source,
                                        Message, 
                                        Version,
                                        StackTrace) 
                                VALUES (@Event,
                                        @LogTypeId,
                                        @Machine,
                                        @Source,
                                        @Message,
                                        @Version,
                                        @StackTrace);",
                            log);
                        conn.Close();
                    }
                }
            }
            catch
            {
                // Prevent too many requests
                Logger.AllowRemoteLogAt = DateTime.Now.AddMinutes(5);
            }
        }

        /// <summary>
        /// Deletes logs older than a particular date from the databae
        /// </summary>
        /// <param name="dateTime">Delete before date</param>
        /// <returns>DbOperationResult - Showing success or fail, with messages and stats</returns>
        public async Task<DbOperationResult> DeleteOlderThan(DateTime dateTime)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            try
            {
                using (var conn = DbConnection())
                {
                    conn.Open();
                    conn.Execute(string.Format(
                          @"DELETE FROM Logs
                            WHERE Event < '{0}'",
                          dateTime.ToSQLDateTime()));
                    conn.Close();
                }

                stopWatch.Stop();
                return await Task.FromResult(DbOperationResult.Success(1, stopWatch.Elapsed));
            }
            catch (Exception e)
            {
                stopWatch.Stop();
                return await Task.FromResult(DbOperationResult.Fail(e.Message, stopWatch.Elapsed));
            }
        }
    }
}
