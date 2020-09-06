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
    /// Music File database repository access.
    /// </summary>
    public class MusicFileRepo : DataAccess
    {
        /// <summary>
        /// Load single MusicFile object by ID from the database
        /// </summary>
        /// <returns>MusicFile object</returns>
        public async Task<MusicFile> Load(int id)
        {
            using (var conn = DbConnection())
            {
                conn.Open();
                var musicFile = conn.Query<MusicFile>(
                        @"SELECT 
                              Id, 
                              Path,
                              MbId,
                              ReleaseMbId,
                              EntityId,
                              Hash,
                              LastUpload, 
                              Error,
                              ErrorReason
                          FROM MusicFiles
                          WHERE Id = @Id
                          AND (Removed IS NULL OR Removed != 1)",
                        new { id }).FirstOrDefault();
                return await Task.FromResult(musicFile);
            }
        }

        /// <summary>
        /// Load single MusicFile object by file path from the database
        /// </summary>
        /// <returns>MusicFile object</returns>
        public async Task<MusicFile> LoadFromPath(string path)
        {
            using (var conn = DbConnection())
            {
                conn.Open();
                var musicFile = conn.Query<MusicFile>(
                        @"SELECT 
                              Id, 
                              Path, 
                              MbId,
                              ReleaseMbId,
                              EntityId,
                              Hash,
                              LastUpload, 
                              Error,
                              ErrorReason
                          FROM MusicFiles
                          WHERE Path = @Path
                          AND (Removed IS NULL OR Removed != 1)",
                        new { path }).FirstOrDefault();
                return await Task.FromResult(musicFile);
            }
        }

        /// Load single MusicFile object by file by YouTube entity Id
        /// </summary>
        /// <returns>MusicFile object</returns>
        public async Task<MusicFile> LoadFromEntityId(string entityId)
        {
            using (var conn = DbConnection())
            {
                conn.Open();
                var musicFile = conn.Query<MusicFile>(
                        @"SELECT 
                              Id, 
                              Path, 
                              MbId,
                              ReleaseMbId,
                              EntityId,
                              Hash,
                              LastUpload, 
                              Error,
                              ErrorReason
                          FROM MusicFiles
                          WHERE EntityId = @EntityId
                          ORDER BY Removed",
                        new { entityId }).FirstOrDefault();
                return await Task.FromResult(musicFile);
            }
        }

        /// <summary>
        /// Load single MusicFile object by hash and from the database that doesn't match
        /// the given path (i.e. a duplicate hash)
        /// </summary>
        /// <param name="hash">Hash to look for</param>
        /// <param name="path">Path of music file we're comparing (path to exlude)</param>
        /// <returns>MusicFile object</returns>
        public Task<MusicFile> GetDuplicate(string hash, string path)
        {
            using (var conn = DbConnection())
            {
                conn.Open();
                var musicFile = conn.Query<MusicFile>(
                        @"SELECT
                              Id, 
                              Path, 
                              MbId,
                              ReleaseMbId,
                              EntityId,
                              Hash,
                              LastUpload, 
                              Error,
                              ErrorReason
                          FROM MusicFiles
                          WHERE Hash = @Hash
                          AND Path != @Path
                          LIMIT 1",
                        new { hash, path }).FirstOrDefault();
                return Task.FromResult(musicFile);
            }
        }

        /// <summary>
        /// Gets a random music file from the database that's been uploaded but has a missing MbId
        /// for the purposes of retreiving it from MusicBrainz when the application is in 'idle' state
        /// </summary>
        /// <returns>MusicFile object</returns>
        public Task<MusicFile> GetRandmonMusicFileWithMissingMbId()
        {
            using (var conn = DbConnection())
            {
                conn.Open();
                var musicFile = conn.Query<MusicFile>(
                        @"SELECT
                              Id, 
                              Path, 
                              MbId,
                              ReleaseMbId,
                              EntityId,
                              Hash,
                              LastUpload, 
                              Error,
                              ErrorReason
                          FROM MusicFiles
                          WHERE (Error = 0 OR Error IS NULL)
                          AND (MbId = '' OR MbId IS NULL)
                          AND LastUpload > '0001-01-01 00:00:00'
                          ORDER BY RANDOM()
                          LIMIT 1").FirstOrDefault();
                return Task.FromResult(musicFile);
            }
        }

        /// <summary>
        /// Gets a random music file from the database that's been uploaded but has a missing the YouTube Music
        /// entityId for the purposes of retreiving it when the application is in 'idle' state
        /// </summary>
        /// <returns>MusicFile object</returns>
        public Task<MusicFile> GetRandmonMusicFileWithMissingEntityId()
        {
            using (var conn = DbConnection())
            {
                conn.Open();
                var musicFile = conn.Query<MusicFile>(
                        @"SELECT
                              Id, 
                              Path, 
                              MbId,
                              ReleaseMbId,
                              EntityId,
                              Hash,
                              LastUpload, 
                              Error,
                              ErrorReason
                          FROM MusicFiles
                          WHERE (Error = 0 OR Error IS NULL)
                          AND (EntityId = '' OR EntityId IS NULL)
                          AND LastUpload > '0001-01-01 00:00:00'
                          ORDER BY RANDOM()
                          LIMIT 1").FirstOrDefault();
                return Task.FromResult(musicFile);
            }
        }

        /// <summary>
        /// Loads a list of all MusicFiles objects with optional filter or order by criteria
        /// </summary>
        /// <param name="includeErrorFiles">Include music files that have error in an upload</param>
        /// <param name="lastUploadAscending">Order by 'LastUpload' (date) in descending order</param>
        /// <param name="ignoreRecentlyUploaded">Don't include entries with a 'LastUpload' date less than a month old</param>
        /// <returns>List of MusicFileObjects</returns>
        public async Task<List<MusicFile>> LoadAll(
            bool includeErrorFiles = true,
            bool lastUploadAscending = false,
            bool ignoreRecentlyUploaded = false)
        {
            using (var conn = DbConnection())
            {
                string cmd = string.Format(
                                @"SELECT 
                                       Id, 
                                       Path, 
                                       MbId,
                                       ReleaseMbId,
                                       EntityId,
                                       Hash,
                                       LastUpload, 
                                       Error,
                                       ErrorReason
                                  FROM MusicFiles
                                  WHERE (Removed IS NULL OR Removed != 1)
                                  {0} {1} {2}",
                                includeErrorFiles
                                    ? ""
                                    : "AND Error = 0",
                                ignoreRecentlyUploaded
                                    ? "AND (LastUpload < '" + DateTime.Now.AddDays(Global.RecheckForUploadedSongsInDays * -1).ToString("yyyy-MM-dd HH:mm:ss") + "'" +
                                       (includeErrorFiles
                                           ? " OR Error = 1"
                                           : "") + ")"
                                    : "",
                                lastUploadAscending ? "ORDER BY LastUpload, IFNULL(Error, 0) ASC" : "");

                conn.Open();
                var musicFiles = conn.Query<MusicFile>(cmd).ToList();
                return await Task.FromResult(musicFiles);
            }
        }

        /// <summary>
        /// Loads a list of MusicFile objects with upload errors
        /// </summary>
        /// <returns>List of MusicFileObjects</returns>
        public Task<List<MusicFile>> LoadIssues()
        {
            using (var conn = DbConnection())
            {
                string cmd = string.Format(
                                @"SELECT 
                                       Id, 
                                       Path, 
                                       MbId,
                                       ReleaseMbId,
                                       EntityId,
                                       Hash,
                                       LastUpload, 
                                       Error,
                                       ErrorReason
                                  FROM MusicFiles
                                  WHERE (Removed IS NULL OR Removed != 1)
                                  AND Error = 1
                                  ORDER BY Id ASC");

                conn.Open();
                var musicFiles = conn.Query<MusicFile>(cmd).ToList();
                return Task.FromResult(musicFiles);
            }
        }

        /// <summary>
        /// Loads a list of MusicFile objects with a successful upload
        /// </summary>
        /// <returns>List of MusicFileObjects</returns>
        public async Task<List<MusicFile>> LoadUploaded()
        {
            using (var conn = DbConnection())
            {
                string cmd = string.Format(
                                @"SELECT 
                                       Id, 
                                       Path, 
                                       MbId,
                                       ReleaseMbId,
                                       EntityId,
                                       Hash,
                                       LastUpload, 
                                       Error,
                                       ErrorReason
                                  FROM MusicFiles
                                  WHERE (Removed IS NULL OR Removed != 1)
                                  AND (Error = 0 OR Error IS NULL)
                                  AND LastUpload IS NOT NULL 
                                  AND LastUpload != '0001-01-01 00:00:00'
                                  ORDER BY LastUpload DESC");

                conn.Open();
                var musicFiles = conn.Query<MusicFile>(cmd).ToList();
                return await Task.FromResult(musicFiles);
            }
        }

        /// <summary>
        /// Count all non-removed Music File entries
        /// </summary>
        /// <returns>Integer count</returns>
        public async Task<int> CountAll()
        {
            using (var conn = DbConnection())
            {
                conn.Open();
                var count = conn.Query<int>(
                        @"SELECT COUNT(Id) 
                          FROM MusicFiles
                          WHERE (Removed IS NULL OR Removed != 1)").FirstOrDefault();
                return await Task.FromResult(count);
            }
        }

        /// <summary>
        /// Count all non-removed Music File entries that have upload errors
        /// </summary>
        /// <returns>Integer count</returns>
        public async Task<int> CountIssues()
        {
            using (var conn = DbConnection())
            {
                conn.Open();
                var count = conn.Query<int>(
                        @"SELECT COUNT(Id) 
                          FROM MusicFiles
                          WHERE Error = 1
                          AND (Removed IS NULL OR Removed != 1)").FirstOrDefault();
                return await Task.FromResult(count);
            }
        }

        /// <summary>
        /// Count all non-removed Music File entries with upload success
        /// </summary>
        /// <returns>Integer count</returns>
        public async Task<int> CountUploaded()
        {
            using (var conn = DbConnection())
            {
                conn.Open();
                var count = conn.Query<int>(
                        @"SELECT COUNT(Id) 
                          FROM MusicFiles
                          WHERE LastUpload IS NOT NULL AND LastUpload != '0001-01-01 00:00:00'
                          AND (Error IS NULL OR Error = 0)
                          AND LastUpload != ''
                          AND (Removed IS NULL OR Removed != 1)").FirstOrDefault();
                return await Task.FromResult(count);
            }
        }

        /// <summary>
        /// Returns the database ID of the music file by a give file path
        /// </summary>
        /// <param name="path">Full inital path of file</param>
        /// <returns>Integer database ID</returns>
        public async Task<int> GetMusicFileIdFromPath(string path)
        {
            using (var conn = DbConnection())
            {
                conn.Open();
                var musicFileId = conn.ExecuteScalar<int?>(
                        @"SELECT Id  
                          FROM MusicFiles
                          WHERE Path = @Path
                          LIMIT 1",
                        new { path });

                if (musicFileId != null)
                    return await Task.FromResult((int)musicFileId);
                return -1;
            }
        }

        /// <summary>
        /// Inserts a Music File entry into the database. If the entry already exists, but is flagged as 'removed',
        /// then the removed flag will be set to false
        /// </summary>
        /// <param name="musicFile">MusicFile object</param>
        /// <returns>DbOperationResult - Showing success or fail, with messages and stats</returns>
        public async Task<DbOperationResult> Insert(MusicFile musicFile)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            try
            {
                int existingMusicFileId = await GetMusicFileIdFromPath(musicFile.Path);
                if (existingMusicFileId == -1)
                {
                    using (var conn = DbConnection())
                    {
                        conn.Open();
                        musicFile.Id = (int)conn.Query<long>(
                            @"INSERT 
                                INTO MusicFiles (
                                        Path, 
                                        Hash,
                                        MbId,
                                        ReleaseMbId,
                                        EntityId,
                                        LastUpload, 
                                        Error,
                                        ErrorReason) 
                                VALUES (@Path, 
                                        @Hash,
                                        @MbId,
                                        @LastUpload, 
                                        @Error,
                                        @ErrorReason);
                              SELECT last_insert_rowid()",
                            musicFile).First();
                    }
                }
                else
                    await RestoreMusicFile(existingMusicFileId);

                stopWatch.Stop();
                return await Task.FromResult(DbOperationResult.Success(musicFile.Id, stopWatch.Elapsed));
            }
            catch (Exception e)
            {
                stopWatch.Stop();
                return await Task.FromResult(DbOperationResult.Fail(e.Message, stopWatch.Elapsed));
            }
        }

        /// <summary>
        /// Sets the 'removed' flag to false
        /// </summary>
        /// <param name="id">Database ID of music file</param>
        public async Task RestoreMusicFile(int id)
        {
            using (var conn = DbConnection())
            {
                conn.Open();
                conn.Execute(
                        @"UPDATE MusicFiles
                            SET Removed = 0
                          WHERE Id = @Id",
                        new { id });
            }

            await Task.Run(() => { });
        }

        /// <summary>
        /// Reset all music file entry uploaded states
        /// </summary>
        public async Task ResetAllMusicFileUploadedStates()
        {
            using (var conn = DbConnection())
            {
                conn.Open();
                conn.Execute(
                        @"UPDATE MusicFiles
                            SET LastUpload = '0001-01-01 00:00:00'");
            }

            await Task.Run(() => { });
        }

        /// <summary>
        /// Updates the Music File entry in the database with the fields of the given MusicFile object
        /// </summary>
        /// <param name="musicFile">Given MusicFile obejct to update with</param>
        /// <returns>DbOperationResult - Showing success or fail, with messages and stats</returns>
        public async Task<DbOperationResult> Update(MusicFile musicFile)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            try
            {
                using (var conn = DbConnection())
                {
                    conn.Open();
                    musicFile.Id = (int)conn.Query<long>(
                        @"UPDATE MusicFiles
                             SET Path = @Path,
                                 Hash = @Hash,
                                 MbId = @MbId,
                                 ReleaseMbId = @ReleaseMbId,
                                 EntityId = @EntityId,
                                 LastUpload = @LastUpload, 
                                 Error = @Error,
                                 ErrorReason = @ErrorReason,
                                 Removed = @Removed
                          WHERE Id = @Id;
                          SELECT last_insert_rowid()",
                        musicFile).First();
                }

                stopWatch.Stop();
                return await Task.FromResult(DbOperationResult.Success(musicFile.Id, stopWatch.Elapsed));
            }
            catch (Exception e)
            {
                stopWatch.Stop();
                return await Task.FromResult(DbOperationResult.Fail(e.Message, stopWatch.Elapsed));
            }
        }

        /// <summary>
        /// Delete or destroyed a Music File entry from the database (delete in the case of a Music File is to set the 'removed'
        /// flag, whereas destroy is to completely delete it from the database
        /// </summary>
        /// <param name="musicFile">Given MusicFile object</param>
        /// <param name="destroy">If true, deletes from the database. If false, just sets the 'Removed' flag</param>
        /// <returns>DbOperationResult - Showing success or fail, with messages and stats</returns>
        public async Task<DbOperationResult> Delete(MusicFile musicFile, bool destroy = false)
        {
            return await Delete(musicFile.Id, destroy);
        }

        /// <summary>
        /// Delete or destroyed a Music File entry from the database (delete in the case of a Music File is to set the 'removed'
        /// flag, whereas destroy is to completely delete it from the database
        /// </summary>
        /// <param name="id">Database ID of Music File entry</param>
        /// <param name="destroy">If true, deletes from the database. If false, just sets the 'Removed' flag</param>
        /// <returns>DbOperationResult - Showing success or fail, with messages and stats</returns>
        public async Task<DbOperationResult> Delete(int id, bool destroy = false)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            try
            {
                using (var conn = DbConnection())
                {
                    conn.Open();

                    if (destroy)
                    {
                        conn.Execute(
                                @"DELETE 
                                  FROM MusicFiles
                                  WHERE Id = @Id",
                                new { id });
                    }
                    else
                    {
                        conn.Execute(
                               @"UPDATE MusicFiles
                                    SET Removed = 1
                                 WHERE Id = @Id",
                               new { id });
                    }
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
        /// Delete or destroyed a Music File entry from the database (delete in the case of a Music File is to set the 'removed'
        /// flag, whereas destroy is to completely delete it from the database
        /// </summary>
        /// <param name="path">Initial full file path of Music File entry</param>
        /// <param name="destroy">If true, deletes from the database. If false, just sets the 'Removed' flag</param>
        /// <returns>DbOperationResult - Showing success or fail, with messages and stats</returns>
        public async Task<DbOperationResult> Delete(string path)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            try
            {
                using (var conn = DbConnection())
                {
                    conn.Open();
                    conn.Execute(
                            @"UPDATE MusicFiles
                                SET Removed = 1
                              WHERE Path = @Path",
                            new { path });
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
        /// Performs a bulk delete of Music File entries who's path starts with a certain file path. I.e.
        /// When bulk deleting because of the removal of a library watch folder
        /// </summary>
        /// <param name="path">Beginning folder path to filter for</param>
        /// <returns>DbOperationResult - Showing success or fail, with messages and stats</returns>
        public async Task<DbOperationResult> DeleteWatchFolder(string path)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            try
            {
                using (var conn = DbConnection())
                {
                    conn.Open();
                    conn.Execute(
                            @"UPDATE MusicFiles 
                                SET Removed = 1
                              WHERE Path LIKE @Path + '%'",
                            new { path });
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
        /// Deletes all Music File entries from the database
        /// </summary>
        /// <returns>DbOperationResult - Showing success or fail, with messages and stats</returns>
        public async Task<DbOperationResult> DeleteAll()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            try
            {
                using (var conn = DbConnection())
                {
                    conn.Open();
                    conn.Execute(
                            @"UPDATE MusicFiles
                                SET Removed = 1");
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
