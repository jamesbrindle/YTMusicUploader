using Dapper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using YTMusicUploader.Providers.Models;

namespace YTMusicUploader.Providers.Repos
{
    public class MusicFileRepo : DataAccess
    {
        public MusicFile Load(int id)
        {
            using (var conn = DbConnection())
            {
                conn.Open();
                var musicFile = conn.Query<MusicFile>(
                        @"SELECT 
                            Id, 
                            Path, 
                            LastUpload, 
                            Error,
                            ErrorReason
                        FROM MusicFiles
                        WHERE Id = @Id",
                        new { id }).FirstOrDefault();
                return musicFile;
            }
        }

        public MusicFile Load(string path)
        {
            using (var conn = DbConnection())
            {
                conn.Open();
                var musicFile = conn.Query<MusicFile>(
                        @"SELECT 
                            Id, 
                            Path, 
                            LastUpload, 
                            Error,
                            ErrorReason
                        FROM MusicFiles
                        WHERE Path = @Path",
                        new { path }).FirstOrDefault();
                return musicFile;
            }
        }

        public List<MusicFile> LoadAll(bool includeErrorFiles = true, bool lastUploadAscending = false)
        {
            using (var conn = DbConnection())
            {
                conn.Open();
                var musicFiles = conn.Query<MusicFile>(string.Format(
                        @"SELECT 
                            Id, 
                            Path, 
                            LastUpload, 
                            Error,
                            ErrorReason
                        FROM MusicFiles
                        {0} {1}",
                        includeErrorFiles ? "" : " WHERE Error = 0",
                        lastUploadAscending ? " ORDER BY LastUpload" : ""));
                return musicFiles.ToList();
            }
        }

        public int GetMusicFileIdFromPath(string path)
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
                    return (int)musicFileId;
                return -1;
            }
        }

        public DbOperationResult Insert(MusicFile musicFile)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            try
            {
                int existingMusicFileId = GetMusicFileIdFromPath(musicFile.Path);
                if (existingMusicFileId == -1)
                {
                    using (var conn = DbConnection())
                    {
                        conn.Open();
                        musicFile.Id = (int)conn.Query<long>(
                            @"INSERT INTO MusicFiles (
                                Path, 
                                LastUpload, 
                                Error,
                                ErrorReason) VALUES (
                                    @Path, 
                                    @LastUpload, 
                                    @Error,
                                    @ErrorReason);
                            SELECT last_insert_rowid()",
                            musicFile).First();
                    }
                }
                else
                {
                    stopWatch.Stop();
                    return DbOperationResult.Success(existingMusicFileId, stopWatch.Elapsed);
                }

                stopWatch.Stop();
                return DbOperationResult.Success(musicFile.Id, stopWatch.Elapsed);
            }
            catch (Exception e)
            {
                stopWatch.Stop();
                return DbOperationResult.Fail(e.Message, stopWatch.Elapsed);
            }
        }

        public DbOperationResult BulkInsert(List<MusicFile> musicFiles)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            try
            {
                using (var conn = DbConnection())
                {
                    conn.Open();
                    Parallel.ForEach(musicFiles, (musicFile) =>
                    {
                        var existingMusicFile = conn.Query<MusicFile>(
                                @"SELECT Id
                                FROM MusicFiles
                                WHERE Path = @Path
                                LIMIT 1",
                                new { musicFile.Path }).FirstOrDefault();

                        if (musicFile == null)
                        {
                            musicFile.Id = (int)conn.Query<long>(
                                @"INSERT INTO MusicFiles (
                                    Path, 
                                    LastUpload, 
                                    Error,
                                    ErrorReason) VALUES (
                                        @Path, 
                                        @LastUpload, 
                                        @Error,
                                        @ErrorReason);
                                SELECT last_insert_rowid()",
                                musicFile).First();
                        }
                    });
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

        public DbOperationResult Update(MusicFile musicFile)
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
                            SET LastUpload = @LastUpdate, 
                                Error = @Error,
                                ErrorReason = @ErrorReason
                        WHERE Id = @Id);
                        SELECT last_insert_rowid()",
                        musicFile).First();
                }

                stopWatch.Stop();
                return DbOperationResult.Success(musicFile.Id, stopWatch.Elapsed);
            }
            catch (Exception e)
            {
                stopWatch.Stop();
                return DbOperationResult.Fail(e.Message, stopWatch.Elapsed);
            }
        }

        public DbOperationResult Delete(MusicFile musicFile)
        {
            return Delete(musicFile.Id);
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
                            @"DELETE FROM MusicFiles
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
                            @"DELETE FROM MusicFiles 
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

        public DbOperationResult BulkDelete(List<MusicFile> musicFiles)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            try
            {
                using (var conn = DbConnection())
                {
                    conn.Open();

                    Parallel.ForEach(musicFiles, (musicFile) =>
                    {
                        conn.Execute(
                                @"DELETE FROM MusicFiles 
                                WHERE Id = @Id",
                                new { musicFile.Id });
                    });
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

        public DbOperationResult DeleteWatchFolder(string path)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            try
            {
                using (var conn = DbConnection())
                {
                    conn.Open();
                    conn.Execute(
                            @"DELETE FROM MusicFiles 
                            WHERE Path LIKE @Path + '%'",
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
