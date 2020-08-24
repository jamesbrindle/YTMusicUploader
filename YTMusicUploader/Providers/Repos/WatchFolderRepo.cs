using Dapper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
        public List<WatchFolder> Load()
        {
            using (var conn = DbConnection())
            {
                conn.Open();
                var watchFolders = conn.Query<WatchFolder>(
                        @"SELECT 
                              Id, 
                              Path
                          FROM WatchFolders
                          ORDER BY Path").ToList();
                return watchFolders;
            }
        }

        /// <summary>
        /// Returns the database ID of the library watch folder from a given full directory path
        /// </summary>
        /// <param name="path">Directory path of the watched library</param>
        /// <returns>Database ID integer</returns>
        public int GetWatchFolderIdFromPath(string path)
        {
            using (var conn = DbConnection())
            {
                conn.Open();
                var watchFolderId = conn.ExecuteScalar<int?>(
                        @"SELECT Id  
                          FROM WatchFolders
                          WHERE Path = @Path
                          LIMIT 1",
                        new { path });

                if (watchFolderId != null)
                    return (int)watchFolderId;
                return -1;
            }
        }

        /// <summary>
        /// Inserts a library Watch Folder entry into the database from the fields of a WatchFolder model object
        /// </summary>
        /// <param name="watchFolder">The given WatchFolder object</param>
        /// <returns>DbOperationResult - Showing success or fail, with messages and stats</returns>
        public DbOperationResult Insert(WatchFolder watchFolder)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            try
            {
                int existingMusicFileId = GetWatchFolderIdFromPath(watchFolder.Path);
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
                    }
                }
                else
                {
                    stopWatch.Stop();
                    return DbOperationResult.Success(existingMusicFileId, stopWatch.Elapsed);
                }

                stopWatch.Stop();
                return DbOperationResult.Success(watchFolder.Id, stopWatch.Elapsed);
            }
            catch (Exception e)
            {
                stopWatch.Stop();
                return DbOperationResult.Fail(e.Message, stopWatch.Elapsed);
            }
        }

        /// <summary>
        /// Deletes a library Watch Folder entry from the database of a given WatchFolder model object
        /// </summary>
        /// <param name="watchFolder">The given WatchFolder object</param>
        /// <returns>DbOperationResult - Showing success or fail, with messages and stats</returns>
        public DbOperationResult Delete(WatchFolder watchFolder)
        {

            return Delete(watchFolder.Id);
        }

        /// <summary>
        /// Deletes a library Watch Folder entry from the database of a given WatchFolder database ID
        /// </summary>
        /// <param name="watchFolder">The given WatchFolder database ID</param>
        /// <returns>DbOperationResult - Showing success or fail, with messages and stats</returns>
        public DbOperationResult Delete(int id)
        {
            Stopwatch stopWatch = new Stopwatch();
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
                }

                stopWatch.Stop();
                return DbOperationResult.Success(-1, stopWatch.Elapsed);
            }
            catch (Exception e)
            {
                stopWatch.Stop();
                return DbOperationResult.Fail(e.Message, stopWatch.Elapsed);
            }
        }

        /// <summary>
        /// Deletes a library Watch Folder entry from the database of a given WatchFolder database full directory path
        /// </summary>
        /// <param name="watchFolder">The given WatchFolder full directory path</param>
        /// <returns>DbOperationResult - Showing success or fail, with messages and stats</returns>
        public DbOperationResult Delete(string path)
        {
            Stopwatch stopWatch = new Stopwatch();
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
                }

                stopWatch.Stop();
                return DbOperationResult.Success(-1, stopWatch.Elapsed);
            }
            catch (Exception e)
            {
                stopWatch.Stop();
                return DbOperationResult.Fail(e.Message, stopWatch.Elapsed);
            }
        }
    }
}
