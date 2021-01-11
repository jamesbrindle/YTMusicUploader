using System;
using System.IO;
using System.Windows;
using YTMusicUploader.Providers.Repos;

namespace YTMusicUploader.Providers
{
    public partial class Database
    {
        public class Maintenance
        {
            /// <summary>
            /// Checks if the database file is present in the users AppData path. If it's not 
            /// present it will copy over the template database file from the Program Files (or working directory) AppData folder
            /// </summary>
            public static void CheckAndCopyDatabaseFile()
            {
                if (!File.Exists(Global.DbLocation))
                {
                    if (!Directory.Exists(Global.AppDataLocation))
                        Directory.CreateDirectory(Global.AppDataLocation);

                    File.Copy(Path.Combine(Global.WorkingDirectory, @"AppData\ytuploader.db"), Global.DbLocation);
                }
                else
                {
                    if (File.Exists(Path.Combine(Global.AppDataLocation, "ytuploader.db-shm")))
                    {
                        try
                        {
                            File.Delete(Path.Combine(Global.AppDataLocation, "ytuploader.db-shm"));
                        }
                        catch { }
                    }

                    if (File.Exists(Path.Combine(Global.AppDataLocation, "ytuploader.db-wal")))
                    {
                        try
                        {
                            File.Delete(Path.Combine(Global.AppDataLocation, "ytuploader.db-wal"));
                        }
                        catch { }
                    }

                    Upgrade.CheckAndRun();
                }

                CheckDatabaseIntegrity();
            }

            /// <summary>
            /// Delete the app data database (user's database)
            /// </summary>
            public static void ResetDatabase()
            {
                if (File.Exists(Path.Combine(Global.AppDataLocation, "ytuploader.db-shm")))
                {
                    try
                    {
                        File.Delete(Path.Combine(Global.AppDataLocation, "ytuploader.db-shm"));
                    }
                    catch { }
                }

                if (File.Exists(Path.Combine(Global.AppDataLocation, "ytuploader.db-wal")))
                {
                    try
                    {
                        File.Delete(Path.Combine(Global.AppDataLocation, "ytuploader.db-wal"));
                    }
                    catch { }
                }

                if (File.Exists(Path.Combine(Global.AppDataLocation, "ytuploader.db")))
                {
                    try
                    {
                        File.Delete(Path.Combine(Global.AppDataLocation, "ytuploader.db"));
                    }
                    catch { }
                }

                if (!File.Exists(Global.DbLocation))
                {
                    if (!Directory.Exists(Global.AppDataLocation))
                        Directory.CreateDirectory(Global.AppDataLocation);

                    File.Copy(Path.Combine(Global.WorkingDirectory, @"AppData\ytuploader.db"), Global.DbLocation);
                }

                Upgrade.CheckAndRun();
            }


            public static void CheckDatabaseIntegrity()
            {
                try
                {
                    Logger.LogInfo("CheckDatabaseIntegrity", "Checking database integrity");

                    var _s = new SettingsRepo().Load().Result;
                    var _w = new WatchFolderRepo().Load().Result;
                    var _m = new MusicFileRepo().LoadAll(true).Result;
                    var _p = new PlaylistFileRepo().LoadAll().Result;
                    var _l = new LogsRepo().LoadSpecific("3").Result;

                    Logger.LogInfo("CheckDatabaseIntegrity", "Database integrity check complete - No issues");
                }
                catch (Exception e)
                {
                    if (
                        MessageBox.Show(
                            $"Unfortunately the database integrity check has failed ({e.Message}). YT Music Uploader cannot continue in this state. " +
                            $"If you click 'OK', YT Music Uploader will reset the database to its original state. You'll lose any uploaded file states but the program" +
                            $" should then work. Otherwise click cancel to attempt to rectify the database yourself located in: %localappdata%\\YTMusicUploader",
                            "Database Integrity Check Fail",
                            MessageBoxButton.OKCancel,
                            MessageBoxImage.Error) == MessageBoxResult.OK)
                    {
                        ResetDatabase();
                        Logger.LogInfo("CheckDatabaseIntegrity", "Database has been reset due to integrity check failure. Comfirmed by user.");
                    }
                }
            }
        }
    }
}
