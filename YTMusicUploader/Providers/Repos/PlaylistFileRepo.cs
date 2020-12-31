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
    /// Playlist File database repository access.
    /// </summary>
    public class PlaylistFileRepo : DataAccess
    {
        /// <summary>
        /// Load all playlist file entries from the database
        /// </summary>
        /// <returns>PlayListFile list</returns>
        public async Task<List<PlaylistFile>> LoadAll()
        {
            try
            {
                return await LoadAll_R();
            }
            catch
            {
                ThreadHelper.SafeSleep(50);
                return await LoadAll_R();
            }
        }

        private async Task<List<PlaylistFile>> LoadAll_R()
        {
            using (var conn = DbConnection(true))
            {
                string cmd =
                        @"SELECT 
                              Id, 
                              Title,
                              Description,
                              PlaylistId,
                              Path,
                              IFNULL(LastModifiedDate, '0001-01-01 00:00:00') [LastModifiedDate],
                              IFNULL(LastUpload, '0001-01-01 00:00:00') [LastUpload]
                          FROM PlaylistFiles";
                conn.Open();
                var playlistFiles = conn.Query<PlaylistFile>(cmd).ToList();
                conn.Close();
                return await Task.FromResult(playlistFiles);
            }
        }

        /// <summary>
        /// Loads a PlayListFile entry from the database by a given path
        /// </summary>
        /// <param name="path">Path of playlist file</param>
        /// <returns>PlayListFile object</returns>
        public async Task<PlaylistFile> LoadFromPath(string path)
        {
            try
            {
                return await LoadFromPath_R(path);
            }
            catch
            {
                ThreadHelper.SafeSleep(50);
                return await LoadFromPath_R(path);
            }
        }

        private async Task<PlaylistFile> LoadFromPath_R(string path)
        {
            using (var conn = DbConnection(true))
            {
                conn.Open();
                var playlistFile = conn.Query<PlaylistFile>(
                        @"SELECT 
                              Id, 
                              Title,
                              Description,
                              PlaylistId,
                              Path,
                              IFNULL(LastModifiedDate, '0001-01-01 00:00:00') [LastModifiedDate],
                              IFNULL(LastUpload, '0001-01-01 00:00:00') [LastUpload]
                          FROM PlaylistFiles
                          WHERE Path = @Path",
                        new { path }).FirstOrDefault();
                conn.Close();
                return await Task.FromResult(playlistFile);
            }
        }

        /// <summary>
        /// Loads a PlayListFile entry from the database by a given playlistId (actuall browseId)
        /// </summary>
        /// <param name="playlistId">playlistId (actuull browseId) of playlist</param>
        /// <returns>PlayListFile object</returns>
        public async Task<PlaylistFile> LoadFromPlayListId(string playlistId)
        {
            try
            {
                return await LoadFromPlayListId_R(playlistId);
            }
            catch
            {
                ThreadHelper.SafeSleep(50);
                return await LoadFromPlayListId_R(playlistId);
            }
        }

        private async Task<PlaylistFile> LoadFromPlayListId_R(string playlistId)
        {
            using (var conn = DbConnection(true))
            {
                conn.Open();
                var playlistFile = conn.Query<PlaylistFile>(
                        @"SELECT 
                              Id, 
                              Title,
                              Description,
                              PlaylistId,
                              Path,
                              IFNULL(LastModifiedDate, '0001-01-01 00:00:00') [LastModifiedDate],
                              IFNULL(LastUpload, '0001-01-01 00:00:00') [LastUpload]
                          FROM PlaylistFiles
                          WHERE PlaylistId = @playlistId",
                        new { playlistId }).FirstOrDefault();
                conn.Close();
                return await Task.FromResult(playlistFile);
            }
        }

        /// <summary>
        /// Inserts a new PlayListFile entry into the database
        /// </summary>
        /// <param name="playlistFile">PlayListFile object</param>
        /// <returns>DbOperationResult - Showing success or fail, with messages and stats</returns>
        public async Task<DbOperationResult> Insert(PlaylistFile playlistFile)
        {
            try
            {
                return await Insert_R(playlistFile);
            }
            catch
            {
                ThreadHelper.SafeSleep(50);
                return await Insert_R(playlistFile);
            }
        }

        private async Task<DbOperationResult> Insert_R(PlaylistFile playlistFile)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            try
            {
                using (var conn = DbConnection())
                {
                    conn.Open();
                    playlistFile.Id = (int)conn.Query<long>(
                        @"INSERT 
                                INTO PlaylisFiles (
                                        Title, 
                                        Description,
                                        PlaylistId,
                                        Path,
                                        LastModifiedDate,
                                        LastUpload) 
                                VALUES @Title, 
                                       @Description,
                                       @PlaylistId,
                                       @Path,
                                       @LastModifiedDate,
                                       @LastUpload);
                              SELECT last_insert_rowid()",
                        playlistFile).First();
                    conn.Close();
                }

                stopWatch.Stop();
                return await Task.FromResult(DbOperationResult.Success(playlistFile.Id, stopWatch.Elapsed));
            }
            catch (Exception e)
            {
                stopWatch.Stop();
                return await Task.FromResult(DbOperationResult.Fail(e.Message, stopWatch.Elapsed));
            }
        }

        /// <summary>
        /// Updates a PlayListFile entry in the database
        /// </summary>
        /// <param name="playlistFile">PlaylistFile object</param>
        /// <returns>DbOperationResult - Showing success or fail, with messages and stats</returns>
        public async Task<DbOperationResult> Update(PlaylistFile playlistFile)
        {
            try
            {
                return await Update_R(playlistFile);
            }
            catch
            {
                ThreadHelper.SafeSleep(50);
                return await Update_R(playlistFile);
            }
        }

        private async Task<DbOperationResult> Update_R(PlaylistFile playlistFile)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            try
            {
                using (var conn = DbConnection())
                {
                    conn.Open();
                    playlistFile.Id = (int)conn.Query<long>(
                        @"UPDATE PlaylistFiles
                             SET Title = @Title,
                                 Description = @Description,
                                 PlaylistId = @PlaylistId,
                                 Path = @Path,
                                 LastModifiedDate = @LastModifiedDate,
                                 LastUpload = @LastUpload
                          WHERE Id = @Id;
                          SELECT last_insert_rowid()",
                        playlistFile).First();
                    conn.Close();
                }

                stopWatch.Stop();
                return await Task.FromResult(DbOperationResult.Success(playlistFile.Id, stopWatch.Elapsed));
            }
            catch (Exception e)
            {
                stopWatch.Stop();
                return await Task.FromResult(DbOperationResult.Fail(e.Message, stopWatch.Elapsed));
            }
        }

        /// <summary>
        /// Resets all PlayListFile entry states, forcing a re-processing of all scanned playlist files
        /// </summary>
        public Task ResetAllPlaylistUploadedStates()
        {
            try
            {
                return ResetAllPlaylistUploadedStates_R();
            }
            catch
            {
                ThreadHelper.SafeSleep(50);
                return ResetAllPlaylistUploadedStates_R();
            }
        }

        private async Task ResetAllPlaylistUploadedStates_R()
        {
            using (var conn = DbConnection())
            {
                conn.Open();
                conn.Execute(
                        @"UPDATE MusicFiles
                            SET LastUpload = '0001-01-01 00:00:00'");
                conn.Close();
            }

            await Task.Run(() => { });
        }

        /// <summary>
        /// Deletes a PlaylistFile entry from the database by a given file path
        /// </summary>
        /// <param name="path">Path of play list file</param>
        /// <returns>DbOperationResult - Showing success or fail, with messages and stats</returns>
        public async Task<DbOperationResult> DeleteFromPath(string path)
        {
            try
            {
                return await DeleteFromPath_R(path);
            }
            catch
            {
                ThreadHelper.SafeSleep(50);
                return await DeleteFromPath_R(path);
            }
        }

        private async Task<DbOperationResult> DeleteFromPath_R(string path)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            try
            {
                using (var conn = DbConnection())
                {
                    conn.Open();
                    conn.Execute(
                            @"DELETE FROM PlaylistFile
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

        /// <summary>
        /// Deletes a PlaylistFile entry from the database by a given playlist id (actually browseId)
        /// </summary>
        /// <param name="playlistId">PlaylistId (actually browseId) of playlist</param>
        /// <returns>DbOperationResult - Showing success or fail, with messages and stats</returns>
        public async Task<DbOperationResult> DeleteFromPlaylistId(string playlistId)
        {
            try
            {
                return await DeleteFromPlaylistId_R(playlistId);
            }
            catch
            {
                ThreadHelper.SafeSleep(50);
                return await DeleteFromPlaylistId_R(playlistId);
            }
        }

        private async Task<DbOperationResult> DeleteFromPlaylistId_R(string playlistId)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            try
            {
                using (var conn = DbConnection())
                {
                    conn.Open();
                    conn.Execute(
                            @"DELETE FROM PlaylistFiles
                              WHERE PlaylistId = @playlistId",
                            new { playlistId });
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
        /// Delete a PlayListFile entry from the database
        /// </summary>
        /// <param name="playlistFile">PlaylistFile object to delete</param>
        /// <returns>DbOperationResult - Showing success or fail, with messages and stats</returns>
        public async Task<DbOperationResult> Delete(PlaylistFile playlistFile)
        {
            try
            {
                return await Delete_R(playlistFile);
            }
            catch
            {
                ThreadHelper.SafeSleep(50);
                return await Delete_R(playlistFile);
            }
        }

        private async Task<DbOperationResult> Delete_R(PlaylistFile playlistFile)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            try
            {
                using (var conn = DbConnection())
                {
                    conn.Open();
                    conn.Execute(
                            @"DELETE FROM PlaylistFiles
                              WHERE Id = @Id",
                            new { playlistFile.Id });
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
        /// Deletes all PlayListFile entries from the database
        /// </summary>
        /// <returns>DbOperationResult - Showing success or fail, with messages and stats</returns>
        public async Task<DbOperationResult> DeleteAll()
        {
            try
            {
                return await DeleteAll_R();
            }
            catch
            {
                ThreadHelper.SafeSleep(50);
                return await DeleteAll_R();
            }
        }

        private async Task<DbOperationResult> DeleteAll_R()
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            try
            {
                using (var conn = DbConnection())
                {
                    conn.Open();
                    conn.Execute(@"DELETE FROM PlaylistFiles");
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
