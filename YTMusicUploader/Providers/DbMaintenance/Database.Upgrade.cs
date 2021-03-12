using Dapper;
using System;
using System.Linq;

namespace YTMusicUploader.Providers
{
    public partial class Database
    {
        public class Upgrade
        {
            /// <summary>
            /// Runs on form load to ensure the database schema is at the latest version following an application upgrade
            /// </summary>
            public static void CheckAndRun()
            {
                using (var conn = DataAccess.DbConnection())
                {
                    conn.Open();

                    try
                    {
                        //
                        // Added Mbid Column to MusicFiles Table in v1.2
                        //

                        var columns = conn.Query<string>(
                                @"SELECT name 
                                  FROM PRAGMA_TABLE_INFO('MusicFiles')").ToList();

                        if (!columns.Contains("MbId"))
                        {
                            conn.Execute(
                                @"ALTER TABLE MusicFiles
                                  ADD COLUMN MbId TEXT");
                        }

                        //
                        // Added ReleaseMbId Column to MusicFiles Table in v1.3.6
                        //

                        columns = conn.Query<string>(
                                @"SELECT name 
                                  FROM PRAGMA_TABLE_INFO('MusicFiles')").ToList();

                        if (!columns.Contains("ReleaseMbId"))
                        {
                            conn.Execute(
                                @"ALTER TABLE MusicFiles
                                  ADD COLUMN ReleaseMbId TEXT");
                        }

                        //
                        // Added EntityId Column to MusicFiles Table in v1.3.6
                        //

                        columns = conn.Query<string>(
                                @"SELECT name 
                                  FROM PRAGMA_TABLE_INFO('MusicFiles')").ToList();

                        if (!columns.Contains("EntityId"))
                        {
                            conn.Execute(
                                @"ALTER TABLE MusicFiles
                                  ADD COLUMN EntityId TEXT");
                        }

                        //
                        // Added Logs Table in 1.4.9
                        // 

                        string result = conn.Query<string>(
                                        @"SELECT name FROM sqlite_master WHERE type='table' AND name='Logs';")
                                            .ToList()
                                            .FirstOrDefault();

                        if (string.IsNullOrEmpty(result))
                        {
                            conn.Execute(
                                @"CREATE TABLE ""LogType"" (
	                              ""Id""	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	                              ""Description""	TEXT NOT NULL
                        );");

                            conn.Execute(
                                @"INSERT INTO LogType (Id, Description) VALUES
                                  (1, 'Info'),
                                  (2, 'Error')");

                            conn.Execute(
                                @"CREATE TABLE ""Logs"" (
	                                ""Id""	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	                                ""Event""	TEXT NOT NULL,
	                                ""LogTypeId""	INTEGER NOT NULL,
	                                ""Machine""	TEXT NOT NULL,
	                                ""Source""	TEXT NOT NULL,
	                                ""Message""	TEXT NOT NULL,
	                                ""StackTrace""	TEXT,
	                            FOREIGN KEY(""LogTypeId"") REFERENCES ""LogType""(""Id"")
                        );");
                        }

                        columns = conn.Query<string>(
                                @"SELECT name 
                                  FROM PRAGMA_TABLE_INFO('Settings')").ToList();

                        if (!columns.Contains("SendLogsToSource"))
                        {
                            conn.Execute(
                                @"ALTER TABLE Settings
                                  ADD SendLogsToSource INTEGER DEFAULT 1");

                        }

                        //
                        // Added additional logs types in 1.5.1
                        // 

                        result = conn.Query<string>(
                                    @"SELECT Description
                                      FROM LogType
                                      WHERE Description = 'Warning'")
                                        .ToList()
                                        .FirstOrDefault();

                        if (result == null)
                        {
                            conn.Execute(
                               @"INSERT INTO LogType (Description)
                                 VALUES ('Warning'),
                                    ('Critical')");
                        }

                        //
                        // Added version to log in 1.5.2
                        // 

                        columns = conn.Query<string>(
                                @"SELECT name 
                                  FROM PRAGMA_TABLE_INFO('Logs')").ToList();

                        if (!columns.Contains("Version"))
                        {
                            conn.Execute(
                                @"ALTER TABLE Logs
                                  ADD COLUMN Version TEXT");
                        }

                        //
                        // Added failure attempts columns to MusicFile in 1.5.2
                        // 

                        columns = conn.Query<string>(
                               @"SELECT name 
                                 FROM PRAGMA_TABLE_INFO('MusicFiles')").ToList();

                        if (!columns.Contains("UploadAttempts"))
                        {
                            conn.Execute(
                                @"ALTER TABLE MusicFiles
                                 ADD COLUMN UploadAttempts INTEGER");

                            conn.Execute(
                                @"ALTER TABLE MusicFiles
                                 ADD COLUMN LastUploadError TEXT ");

                            conn.Execute(
                               @"UPDATE MusicFiles
                                 SET LastUploadError = '0001-01-01 00:00:00'");
                        }

                        //
                        // Corrected column 'Version' data type in 1.5.4 - Should be text
                        //

                        string versionDataType = conn.Query<string>(
                                "SELECT TYPEOF(Version) FROM Logs LIMIT 1").ToList()
                                                                           .FirstOrDefault();

                        if (versionDataType != null && versionDataType.ToLower() != "text")
                        {
                            conn.Execute(
                              @"DROP TABLE ""Logs"";
                                CREATE TABLE ""Logs"" (
                                    ""Id""    INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
                                    ""Event"" TEXT NOT NULL,
                                    ""LogTypeId"" INTEGER NOT NULL,
                                    ""Machine""   TEXT NOT NULL,
                                    ""Source""    TEXT NOT NULL,
                                    ""Message""   TEXT NOT NULL,
                                    ""Version""   TEXT,
                                    ""StackTrace""    TEXT     
                                ); ");
                        }

                        //
                        // Version 1.6.0 - Added song 'VideoId' column
                        //               - Added 'PlaylistFiles' table
                        //               - FILE RESCAN REQUIRED
                        //

                        columns = conn.Query<string>(
                                @"SELECT name 
                                  FROM PRAGMA_TABLE_INFO('MusicFiles')").ToList();

                        if (!columns.Contains("VideoId"))
                        {
                            conn.Execute(
                                @"ALTER TABLE MusicFiles
                                  ADD COLUMN VideoId TEXT");
                        }

                        bool playlistTableExists = conn.Query<string>(
                            @"SELECT name 
                              FROM sqlite_master WHERE type='table' AND name='PlaylistFiles';").ToList().Count > 0;

                        if (!playlistTableExists)
                        {
                            conn.Execute(
                                @"CREATE TABLE ""PlaylistFiles"" (
                                    ""Id""    INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                                    ""Title"" TEXT,
                                    ""Description""   TEXT,
                                    ""PlaylistId""    TEXT,
                                    ""Path""  TEXT NOT NULL,
                                    ""LastModifiedDate""  TEXT NOT NULL,
                                    ""LastUpload""    TEXT
                                 ); ");

                            conn.Execute(
                               @"UPDATE MusicFiles
                                SET LastUpload = '0001-01-01 00:00:00'");
                        }

                        //
                        // Version 1.6.5 - Added application 'Version' column to 'Settings' table so we can determine what
                        //                 DB upgrades are required, rather than looking for missing columns etc
                        //               - FILE RESCAN REQUIRED
                        //

                        columns = conn.Query<string>(
                                @"SELECT name 
                                  FROM PRAGMA_TABLE_INFO('Settings')").ToList();

                        if (!columns.Contains("Version"))
                        {
                            conn.Execute(
                                @"ALTER TABLE Settings
                                  ADD COLUMN Version TEXT");

                            conn.Execute(
                               @"UPDATE MusicFiles
                                 SET LastUpload = '0001-01-01 00:00:00',
                                     Error = 0,
                                     ErrorReason = NULL,
                                     UploadAttempts = NULL,
                                     EntityId = NULL,
                                     VideoId = NULL");
                        }

                        //
                        // Added option to enable or disable playlist upload in 1.7.0
                        // 

                        columns = conn.Query<string>(
                                @"SELECT name 
                                  FROM PRAGMA_TABLE_INFO('Settings')").ToList();

                        if (!columns.Contains("UploadPlaylists"))
                        {
                            conn.Execute(
                                @"ALTER TABLE Settings
                                  ADD COLUMN UploadPlaylists INTEGER DEFAULT 1");

                            conn.Execute(
                                @"UPDATE Settings
                                  SET UploadPlaylists = 1");

                            conn.Execute(
                                @"ALTER TABLE Settings
                                  ADD COLUMN LastPlaylistUpload TEXT");
                        }

                        // Set DB version to App version (MAKE SURE IT's THE LAST THING ON THIS METHOD)

                        conn.Execute(string.Format(
                                @"UPDATE Settings
                                  SET Version = '{0}'", Global.ApplicationVersion.ToLower().Replace("v", "")));
                    }
                    catch { }

                    conn.Close();
                }
            }
        }
    }
}
