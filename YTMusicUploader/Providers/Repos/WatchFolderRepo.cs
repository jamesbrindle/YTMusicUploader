using Dapper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using YTMusicUploader.Providers.Models;

namespace YTMusicUploader.Providers.Repos
{
    public class WatchFolderRepo: DataAccess
    {
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
                            @"INSERT INTO WatchFolders (
                                Path) VALUES (
                                    @Path);
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

        public DbOperationResult Delete(WatchFolder watchFolder)
        {
            return Delete(watchFolder.Id);
        }

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
