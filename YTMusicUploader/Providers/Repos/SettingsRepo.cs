using Dapper;
using JBToolkit.Threads;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using YTMusicUploader.Providers.DataModels;

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
        public async Task<Settings> Load()
        {
            try
            {
                return await Load_R();
            }
            catch
            {
                ThreadHelper.SafeSleep(50);
                return await Load_R();
            }
        }

        private async Task<Settings> Load_R()
        {
            using (var conn = DbConnection(true))
            {
                conn.Open();
                var settings = conn.Query<Settings>(
                        @"SELECT 
                              Id, 
                              StartWithWindows, 
                              ThrottleSpeed, 
                              AuthenticationCookie,
                              SendLogsToSource,
                              UploadPlaylists,
                              LastPlaylistUpload,
                              CurrentSessionPlaylistUploadCount
                          FROM Settings").FirstOrDefault();
                conn.Close();

                return await Task.FromResult(settings);
            }
        }

        /// <summary>
        /// Updates the application settings data in the database
        /// </summary>
        /// <param name="settings">Settings model object</param>
        /// <returns>DbOperationResult - Showing success or fail, with messages and stats</returns>
        public async Task<DbOperationResult> Update(Settings settings)
        {
            try
            {
                return await Update_R(settings);
            }
            catch
            {
                ThreadHelper.SafeSleep(50);
                return await Update_R(settings);
            }
        }

        private async Task<DbOperationResult> Update_R(Settings settings)
        {
            var stopWatch = new Stopwatch();
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
                                     AuthenticationCookie = @AuthenticationCookie,
                                     SendLogsToSource = @SendLogsToSource,
                                     UploadPlaylists = @UploadPlaylists,
                                     LastPlaylistUpload = @LastPlaylistUpload,
                                     CurrentSessionPlaylistUploadCount = @CurrentSessionPlaylistUploadCount
                              WHERE Id = @Id",
                            settings);
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
