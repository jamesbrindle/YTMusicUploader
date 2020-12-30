using Dapper;
using JBToolkit.Threads;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YTMusicUploader.Providers.DataModels;

namespace YTMusicUploader.Providers.Repos
{
    public class PlaylistFileRepo : DataAccess
    {
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
                              IFNULL(LastModifiedDate, '0001-01-01 00:00:00') [LastModifiedDate]
                              IFNULL(LastUpload, '0001-01-01 00:00:00') [LastUpload]
                          FROM PlaylistFiles
                          WHERE Path = @Path",
                        new { path }).FirstOrDefault();
                conn.Close();
                return await Task.FromResult(playlistFile);
            }
        }

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
                              IFNULL(LastModifiedDate, '0001-01-01 00:00:00') [LastModifiedDate]
                              IFNULL(LastUpload, '0001-01-01 00:00:00') [LastUpload]
                          FROM PlaylistFiles
                          WHERE PlaylistId = @playlistId",
                        new { playlistId }).FirstOrDefault();
                conn.Close();
                return await Task.FromResult(playlistFile);
            }
        }

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
            Stopwatch stopWatch = new Stopwatch();
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
        /// Updates the Music File entry in the database with the fields of the given MusicFile object
        /// </summary>
        /// <param name="musicFile">Given MusicFile obejct to update with</param>
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
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            try
            {
                using (var conn = DbConnection())
                {
                    conn.Open();
                    playlistFile.Id = (int)conn.Query<long>(
                        @"UPDATE PlaylistFie
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
            Stopwatch stopWatch = new Stopwatch();
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
            Stopwatch stopWatch = new Stopwatch();
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
            Stopwatch stopWatch = new Stopwatch();
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
            Stopwatch stopWatch = new Stopwatch();
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
