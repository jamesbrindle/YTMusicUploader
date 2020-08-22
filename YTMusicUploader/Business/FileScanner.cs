using JBToolkit.Windows;
using System;
using System.Collections.Generic;
using System.IO;
using YTMusicUploader.Providers;
using YTMusicUploader.Providers.Models;
using System.Data.SQLite;
using Dapper;

namespace YTMusicUploader.Business
{
    public class FileScanner
    {
        private MainForm MainForm { get; set; }
        public List<FileData> NewFiles { get; set; } = new List<FileData>();
        public HashSet<string> NewFilesHash { get; set; } = new HashSet<string>();
        public HashSet<string> DiscoveredFilesHash { get; set; } = new HashSet<string>();
        public List<MusicFile> MusicFilesToDelete { get; set; } = new List<MusicFile>();
        public List<MusicFile> CurrentMusicFiles { get; set; }
        public HashSet<string> CurrentMusicFilesHash { get; set; } = new HashSet<string>();

        public FileScanner(MainForm mainForm)
        {
            MainForm = mainForm;
        }

        public void Process()
        {
            MainForm.SetStatusMessage("Looking for new files...");

            CurrentMusicFiles = MainForm.MusicFileRepo.LoadAll();
            foreach (var musicFile in CurrentMusicFiles)
                CurrentMusicFilesHash.Add(musicFile.Path);

            if (MainForm.WatchFolders.Count == 0)
                MainForm.MusicFileRepo.DeleteAll();

            //
            // Get files to add - Cross reference with the DB
            //
            foreach (var watchFolder in MainForm.WatchFolders)
            {
                if (MainForm.Aborting)
                {
                    MainForm.SetStatusMessage("Idle");
                    return;
                }

                foreach (var file in FastDirectoryEnumerator.EnumerateFiles(
                                                                        watchFolder.Path,
                                                                        "*.*",
                                                                        SearchOption.AllDirectories))
                {
                    if (MainForm.Aborting)
                    {
                        MainForm.SetStatusMessage("Idle");
                        return;
                    }

                    if (!CurrentMusicFilesHash.Contains(file.Path))
                    {
                        if (Path.GetExtension(file.Name.ToLower()).In(Global.SupportedFiles))
                        {
                            NewFiles.Add(file);
                            NewFilesHash.Add(file.Path);
                        }
                    }

                    if (Path.GetExtension(file.Name.ToLower()).In(Global.SupportedFiles))
                        DiscoveredFilesHash.Add(file.Path);
                }
            }

#if DEBUG
            Console.Out.WriteLine("File scan complete: " + NewFiles.Count + " files found.");
#endif
            //
            // Get files to delete - Cross reference with the DB
            //
            foreach (var musicFile in CurrentMusicFiles)
            {
                if (MainForm.Aborting)
                {
                    MainForm.SetStatusMessage("Idle");
                    return;
                }

                if (!DiscoveredFilesHash.Contains(musicFile.Path))
                    MusicFilesToDelete.Add(musicFile);
            }

            using (var conn = new SQLiteConnection("Data Source=" + DataAccess.DBLocation))
            {
                conn.Open();
                int count = 0;
                foreach (var file in NewFiles)
                {
                    if (MainForm.Aborting)
                    {
                        MainForm.SetStatusMessage("Idle");
                        return;
                    }

                    count++;
                    if (count > MainForm.InitialFilesCount)
                        if (count % 100 == 0)
                            MainForm.SetDiscoveredFilesLabel(count.ToString());

                    AddToDB(conn, new MusicFile(file.Path));
                }

                if (MainForm.Aborting)
                {
                    MainForm.SetStatusMessage("Idle");
                    return;
                }
#if DEBUG
                Console.Out.WriteLine("Insert new music files complete: " + count + " files inserted");
#endif
                count = 0;
                foreach (var musicFile in MusicFilesToDelete)
                {
                    if (MainForm.Aborting)
                    {
                        MainForm.SetStatusMessage("Idle");
                        return;
                    }

                    count++;
                    RemoveFromDB(conn, musicFile.Path);

                };

                MainForm.SetDiscoveredFilesLabel(MainForm.MusicFileRepo.CountAll().ToString());

#if DEBUG
                Console.Out.WriteLine("Delete removed music files complete: " + count + " files inserted");
#endif
            }

            MainForm.SetStatusMessage(MainForm.ConnectedToYTMusic ? "Ready" : "Not running");
        }

        private void AddToDB(SQLiteConnection conn, MusicFile musicFile)
        {
            // Not using the standard Repos namespace so we don't have to keep creating and
            // opening new connections

            try
            {
                conn.Execute(
                          @"INSERT INTO MusicFiles (
			                            Path, 
			                            LastUpload, 
			                            Error,
			                            ErrorReason)
                            SELECT @Path,
	                               @LastUpload,
	                               @Error,
	                               @ErrorReason
                            WHERE NOT EXISTS (SELECT Id FROM MusicFiles WHERE Path = @Path LIMIT 1)",
                          musicFile);
            }
            catch { }
        }

        private void RemoveFromDB(SQLiteConnection conn, string path)
        {
            // Not using the standard Repos namespace so we don't have to keep creating and
            // opening new connections

            try
            {
                conn.Execute(
                          @"DELETE FROM MusicFiles
                            WHERE Path = @Path",
                          new { path });
            }
            catch { }
        }

        public void Reset()
        {
            NewFiles = new List<FileData>();
            NewFilesHash = new HashSet<string>();
            DiscoveredFilesHash = new HashSet<string>();
            MusicFilesToDelete = new List<MusicFile>();
            CurrentMusicFilesHash = new HashSet<string>();
        }
    }
}
