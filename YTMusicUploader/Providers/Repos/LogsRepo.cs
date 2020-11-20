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
            using (var conn = DbConnection())
            {
                string cmd = string.Format(
                                @"SELECT * 
                                  FROM Logs
                                  ORDER BY Id DESC");

                conn.Open();
                var logs = conn.Query<Log>(cmd).ToList();
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
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            try
            {
                using (var conn = DbConnection())
                {
                    conn.Open();

                    log.Id = (int)conn.Query<long>(
                        @"INSERT 
                                INTO Logs (
                                        Event, 
                                        LogTypeId,
                                        Machine,
                                        Source,
                                        Message, 
                                        StackTrace) 
                                VALUES (@Event,
                                        @LogTypeId,
                                        @Machine,
                                        @Source,
                                        @Message,
                                        @StackTrace);
                              SELECT last_insert_rowid()",
                        log).First();
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

        /// <summary>
        /// Sends the log off to the source for diagnostic data
        /// </summary>
        /// <param name="log">Log object to record</param>
        internal void RemoteAdd(Log log)
        {
            try
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
                                        StackTrace) 
                                VALUES (@Event,
                                        @LogTypeId,
                                        @Machine,
                                        @Source,
                                        @Message,
                                        @StackTrace);",
                        log);
                }
            }
            catch { }
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
