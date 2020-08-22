using Dapper;
using System;
using System.Diagnostics;
using System.Linq;
using YTMusicUploader.Providers.Models;

namespace YTMusicUploader.Providers.Repos
{
    /// <summary>
    /// Application settings database repository access. 
    /// </summary>
    public class SettingsRepo : DataAccess
    {
        /// <summary>
        /// Loads the application settings data from the database
        /// </summary>
        /// <returns>Setting model object</returns>
        public Settings Load()
        {
            using (var conn = DbConnection())
            {
                conn.Open();
                var settings = conn.Query<Settings>(
                        @"SELECT 
                              Id, 
                              StartWithWindows, 
                              ThrottleSpeed, 
                              AuthenticationCookie
                          FROM Settings").FirstOrDefault();
                return settings;
            }
        }

        /// <summary>
        /// Updates the application settings data in the database
        /// </summary>
        /// <param name="settings">Settings model object</param>
        /// <returns>DbOperationResult - Showing success or fail, with messages and stats</returns>
        public DbOperationResult Update(Settings settings)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            try
            {
                using (var conn = DbConnection())
                {
                    conn.Open();
                    conn.Execute(
                            @"UPDATE Settings 
                                 SET StartWithWindows = @StartWithWindows, 
                                     ThrottleSpeed = @ThrottleSpeed,  
                                     AuthenticationCookie = @AuthenticationCookie
                              WHERE Id = @Id",
                            settings);

                }

                stopWatch.Stop();
                return DbOperationResult.Success(1, stopWatch.Elapsed);
            }
            catch (Exception e)
            {
                stopWatch.Stop();
                return DbOperationResult.Fail(e.Message, stopWatch.Elapsed);
            }
        }
    }
}
