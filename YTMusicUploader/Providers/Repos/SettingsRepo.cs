using Dapper;
using System;
using System.Data;
using System.Diagnostics;
using System.Linq;
using YTMusicUploader.Providers.Models;

namespace YTMusicUploader.Providers.Repos
{
    public class SettingsRepo : DataAccess
    {
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
