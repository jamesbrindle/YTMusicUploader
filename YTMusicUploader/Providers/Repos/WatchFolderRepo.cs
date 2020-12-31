using Dapper;
using JBToolkit.Threads;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using YTMusicUploader.Providers.DataModels;

namespace YTMusicUploader.Providers.Repos
{
    /// <summary>
    /// Library watch folders database repository access
    /// </summary>
    public class WatchFolderRepo : DataAccess
    {
        /// <summary>
        /// Loads a list of library watch folders from the database
        /// </summary>
        /// <returns>List of WatchFolder model objects</returns>
        public async Task<List<WatchFolder>> Load()
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

        private async Task<List<WatchFolder>> Load_R()
        {
            using (var conn = DbConnection(true))
            {
                conn.Open();
                var watchFolders = conn.Query<WatchFolder>(
                        @"SELECT 
                              Id, 
                              Path
                          FROM WatchFolders
                          ORDER BY Path").ToList();
                conn.Close();

                return await Task.FromResult(watchFolders);
            }
        }

        /// <summary>
        /// Returns the database ID of the library watch folder from a given full directory path
        /// </summary>
        /// <param name="path">Directory path of the watched library</param>
        /// <returns>Database ID integer</returns>
        public async Task<int> GetWatchFolderIdFromPath(string path)
        {
            try
            {
                return await GetWatchFolderIdFromPath_R(path);
            }
            catch
            {
                ThreadHelper.SafeSleep(50);
                return await GetWatchFolderIdFromPath_R(path);
            }
        }

        private async Task<int> GetWatchFolderIdFromPath_R(string path)
        {
            using (var conn = DbConnection(true))
            {
                conn.Open();
                int? watchFolderId = conn.ExecuteScalar<int?>(
                        @"SELECT Id  
                          FROM WatchFolders
                          WHERE Path = @Path
                          LIMIT 1",
                        new { path });
                conn.Close();

                if (watchFolderId != null)
                    return await Task.FromResult((int)watchFolderId);
                return -1;
            }
        }

        /// <summary>
        /// Inserts a library Watch Folder entry into the database from the fields of a WatchFolder model object
        /// </summary>
        /// <param name="watchFolder">The given WatchFolder object</param>
        /// <returns>DbOperationResult - Showing success or fail, with messages and stats</returns>
        public async Task<DbOperationResult> Insert(WatchFolder watchFolder)
        {
            try
            {
                return await Insert_R(watchFolder);
            }
            catch
            {
                ThreadHelper.SafeSleep(50);
                return await Insert_R(watchFolder);
            }
        }

        private async Task<DbOperationResult> Insert_R(WatchFolder watchFolder)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            try
            {
                int existingMusicFileId = GetWatchFolderIdFromPath(watchFolder.Path).Result;
                if (existingMusicFileId == -1)
                {
                    using (var conn = DbConnection())
                    {
                        conn.Open();
                        watchFolder.Id = (int)conn.Query<long>(
                            @"INSERT 
                                INTO WatchFolders (
                                        Path) 
                                VALUES (@Path);
                              SELECT last_insert_rowid()",
                            watchFolder).First();
                        conn.Close();
                    }
                }
                else
                {
                    stopWatch.Stop();
                    return await Task.FromResult(DbOperationResult.Success(existingMusicFileId, stopWatch.Elapsed));
                }

                stopWatch.Stop();
                return await Task.FromResult(DbOperationResult.Success(watchFolder.Id, stopWatch.Elapsed));
            }
            catch (Exception e)
            {
                stopWatch.Stop();
                return await Task.FromResult(DbOperationResult.Fail(e.Message, stopWatch.Elapsed));
            }
        }

        /// <summary>
        /// Deletes a library Watch Folder entry from the database of a given WatchFolder model object
        /// </summary>
        /// <param name="watchFolder">The given WatchFolder object</param>
        /// <returns>DbOperationResult - Showing success or fail, with messages and stats</returns>
        public async Task<DbOperationResult> Delete(WatchFolder watchFolder)
        {
            try
            {
                return await Delete_R(watchFolder);
            }
            catch
            {
                ThreadHelper.SafeSleep(50);
                return await Delete_R(watchFolder);
            }
        }

        private async Task<DbOperationResult> Delete_R(WatchFolder watchFolder)
        {
            return await Delete(watchFolder.Id);
        }

        /// <summary>
        /// Deletes a library Watch Folder entry from the database of a given WatchFolder database ID
        /// </summary>
        /// <param name="watchFolder">The given WatchFolder database ID</param>
        /// <returns>DbOperationResult - Showing success or fail, with messages and stats</returns>
        public async Task<DbOperationResult> Delete(int id)
        {
            try
            {
                return await Delete_R(id);
            }
            catch
            {
                ThreadHelper.SafeSleep(50);
                return await Delete_R(id);
            }
        }

        private async Task<DbOperationResult> Delete_R(int id)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            try
            {
                using (var conn = DbConnection())
                {
                    conn.Open();
                    conn.Execute(
                            @"DELETE FROM WatchFolders
                              WHERE Id = @Id",
                            new { id });
                    conn.Close();
                }

                stopWatch.Stop();
                return await Task.FromResult(DbOperationResult.Success(-1, stopWatch.Elapsed));
            }
            catch (Exception e)
            {
                stopWatch.Stop();
                return await Task.FromResult(DbOperationResult.Fail(e.Message, stopWatch.Elapsed));
            }
        }

        /// <summary>
        /// Deletes a library Watch Folder entry from the database of a given WatchFolder database full directory path
        /// </summary>
        /// <param name="watchFolder">The given WatchFolder full directory path</param>
        /// <returns>DbOperationResult - Showing success or fail, with messages and stats</returns>
        public async Task<DbOperationResult> Delete(string path)
        {
            try
            {
                return await Delete_R(path);
            }
            catch
            {
                ThreadHelper.SafeSleep(50);
                return await Delete_R(path);
            }
        }

        private async Task<DbOperationResult> Delete_R(string path)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            try
            {
                using (var conn = DbConnection())
                {
                    conn.Open();
                    conn.Execute(
                            @"DELETE FROM WatchFolders 
                              WHERE Path = @Path",
                            new { path });
                    conn.Close();
                }

                stopWatch.Stop();
                return await Task.FromResult(DbOperationResult.Success(-1, stopWatch.Elapsed));
            }
            catch (Exception e)
            {
                stopWatch.Stop();
                return await Task.FromResult(DbOperationResult.Fail(e.Message, stopWatch.Elapsed));
            }
        }
    }
}
