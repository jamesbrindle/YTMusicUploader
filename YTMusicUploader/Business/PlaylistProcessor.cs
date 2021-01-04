using JBToolkit.FuzzyLogic;
using JBToolkit.Network;
using JBToolkit.Threads;
using JBToolkit.Windows;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using YTMusicUploader.Providers;
using YTMusicUploader.Providers.DataModels;
using static YTMusicUploader.Providers.RequestModels.ArtistCache;

namespace YTMusicUploader.Business
{
    /// <summary>
    /// Responsible for scanning library playlist files and either creating them on YT Music or adding to them on YT Music
    /// </summary>
    public class PlaylistProcessor
    {
        private MainForm MainForm { get; set; }
        public List<PlaylistFile> PlaylistFiles { get; set; }
        public OnlinePlaylistCollection OnlinePlaylists { get; set; }
        public bool Stopped { get; set; } = false;

        public PlaylistProcessor(MainForm mainForm)
        {
            MainForm = mainForm;
        }

        /// <summary>
        /// Execute the playlist management process
        /// </summary>
        public void Process(bool forceRefreshPlaylists = false)
        {
            Stopped = false;
            SetStatus("Processing playlist files", "Processing playlist files");

            try
            {
                PlaylistFiles = MainForm.PlaylistFileRepo.LoadAll().Result;
                if (forceRefreshPlaylists)
                    Requests.ArtistCache.Playlists = Requests.Playlists.GetPlaylists(MainForm.Settings.AuthenticationCookie);

                OnlinePlaylists = Requests.ArtistCache.Playlists;

                if (PlaylistFiles != null)
                {
                    foreach (var playlistFile in PlaylistFiles)
                    {   
                        ConnectionCheckWait();
                        if (MainFormAborting())
                            return;

                        if (playlistFile.LastModifiedDate != playlistFile.LastUpload ||
                          (File.Exists(playlistFile.Path) && playlistFile.LastModifiedDate != new FileInfo(playlistFile.Path).LastWriteTime) ||
                          (playlistFile.PlaylistId != null && MatchOnlinePlaylist(playlistFile.PlaylistId, true) == null))
                        {
                            try
                            {
                                if (MainFormAborting())
                                    return;

                                var updatedPlaylistFile = Providers.Playlist.PlaylistReader.ReadPlaylistFile(playlistFile.Path);
                                playlistFile.Title = updatedPlaylistFile.Title;
                                OnlinePlaylist onlinePlaylist = null;

                                ConnectionCheckWait();
                                if (MainFormAborting())
                                    return;

                                if (!string.IsNullOrEmpty(playlistFile.PlaylistId) && MatchOnlinePlaylist(playlistFile.PlaylistId, true) != null)
                                    onlinePlaylist = Requests.Playlists.GetPlaylist(MainForm.Settings.AuthenticationCookie, playlistFile.PlaylistId);
                                else
                                {
                                    onlinePlaylist = MatchOnlinePlaylist(playlistFile.Title, false);
                                    if (onlinePlaylist != null)
                                    {
                                        // Get full playlist details
                                        onlinePlaylist = Requests.Playlists.GetPlaylist(MainForm.Settings.AuthenticationCookie, onlinePlaylist.BrowseId);
                                        playlistFile.PlaylistId = onlinePlaylist.BrowseId;
                                    }
                                }

                                if (MainFormAborting())
                                    return;

                                foreach (var playlistItem in updatedPlaylistFile.PlaylistItems)
                                {
                                    if (MainFormAborting())
                                        return;

                                    // We can only create or update a playlist if we have the YT Music video (entity) ID
                                    var musicFile = MainForm.MusicFileRepo.LoadFromPath(playlistItem.Path).Result;
                                    if (musicFile != null && !string.IsNullOrEmpty(musicFile.VideoId))
                                    {
                                        var playlistToAdd = playlistItem;
                                        playlistItem.VideoId = musicFile.VideoId;
                                        playlistFile.PlaylistItems.Add(playlistToAdd);
                                    }
                                    else
                                    {
                                        // Check the file hash instead, in case the playlist file path is somehow different
                                        // to the watch folder file path (i.e. network folder locations or symbolics links)

                                        string hash = DirectoryHelper.GetFileHash(playlistItem.Path).Result;
                                        musicFile = MainForm.MusicFileRepo.LoadFromHash(hash).Result;

                                        if (musicFile != null && !string.IsNullOrEmpty(musicFile.VideoId))
                                        {
                                            var playlistToAdd = playlistItem;
                                            playlistItem.VideoId = musicFile.VideoId;
                                            playlistFile.PlaylistItems.Add(playlistToAdd);
                                        }
                                    }
                                }

                                if (MainFormAborting())
                                    return;

                                if (onlinePlaylist != null)
                                    HandleOnlinePlaylistPresent(onlinePlaylist, playlistFile);
                                else
                                    HandleOnlinePlaylistNeedsCreating(playlistFile);
                            }
                            catch (Exception e)
                            {
                                Logger.Log(e, $"PlaylistProcessor - Error processing playlist: {playlistFile.Title}");
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                if (e.Message.ToLower().Contains("thread was being aborted") ||
                   (e.InnerException != null && e.InnerException.Message.ToLower().Contains("thread was being aborted")))
                {
                    // Non-detrimental - Ignore to not clog up the application log
                    // Logger.Log(e, "PlaylistProcessor", Log.LogTypeEnum.Warning);
                }
                else
                {
                    Logger.Log(e, "Fatal error in PlaylistProcessor", Log.LogTypeEnum.Critical);
                }
            }

            Stopped = true;
        }

        private void HandleOnlinePlaylistPresent(OnlinePlaylist onlinePlaylist, PlaylistFile playlistFile, bool forceRefreshPlaylists = false)
        {
            ConnectionCheckWait();
            if (MainFormAborting())
                return;

            if (onlinePlaylist.Songs.Count < 5000)
            {
                foreach (var onlinePlaylistItem in onlinePlaylist.Songs)
                {
                    if (playlistFile.PlaylistItems.Where(p => p.VideoId == onlinePlaylistItem.VideoId).Any())
                        playlistFile.PlaylistItems.Remove(playlistFile.PlaylistItems.Where(p => p.VideoId == onlinePlaylistItem.VideoId).FirstOrDefault());
                }

                playlistFile.Description = onlinePlaylist.Description;
                playlistFile.Title = onlinePlaylist.Title;

                // Max YouTube Music playlist size
                if (onlinePlaylist.Songs.Count + playlistFile.PlaylistItems.Count > 5000)
                {
                    for (int i = playlistFile.PlaylistItems.Count - 1; i >= 0; i--)
                    {
                        playlistFile.PlaylistItems.RemoveAt(i);
                        if (onlinePlaylist.Songs.Count + playlistFile.PlaylistItems.Count > 5000)
                            break;
                    }
                }

                foreach (var playlistItem in playlistFile.PlaylistItems)
                {
                    ConnectionCheckWait();
                    if (MainFormAborting())
                        return;

                    for (int i = 0; i < 7; i++)
                    {
                        if (Requests.Playlists.AddPlaylistItem(
                                MainForm.Settings.AuthenticationCookie,
                                playlistFile.PlaylistId,
                                playlistItem.VideoId,
                                out Exception e))
                        {
                            Logger.LogInfo("HandleOnlinePlaylistPresent", $"Adding item to online playlist: {playlistFile.Title}: Success");
                            break;
                        }
                        else
                        {
                            if (e.Message.Contains("404") && !forceRefreshPlaylists)
                            {
                                // Then playlist recently deleted. So create new one by running original 'Process()'
                                Process(true);
                                return;
                            }
                            else
                            {
                                if (i == 4)
                                {
                                    Logger.Log(e, $"Error adding item to online playlist: {playlistFile.Title}", Log.LogTypeEnum.Error);
                                    break;
                                }
                                else
                                    ThreadHelper.SafeSleep(3000);
                            }
                        }
                    }
                }
            }

            playlistFile.LastModifiedDate = new FileInfo(playlistFile.Path).LastWriteTime;
            playlistFile.LastUpload = playlistFile.LastModifiedDate;
            playlistFile.Save().Wait();
        }

        private void HandleOnlinePlaylistNeedsCreating(PlaylistFile playlistFile)
        {
            ConnectionCheckWait();
            if (MainFormAborting())
                return;

            if (string.IsNullOrEmpty(playlistFile.Description))
                playlistFile.Description = "Created by YT Music Uploader";

            // Max YouTube Music playlist size
            if (playlistFile.PlaylistItems.Count > 5000)
            {
                for (int i = playlistFile.PlaylistItems.Count - 1; i >= 0; i--)
                {
                    playlistFile.PlaylistItems.RemoveAt(i);
                    if (playlistFile.PlaylistItems.Count < 5001)
                        break;
                }
            }

            if (MainFormAborting())
                return;

            if (playlistFile.PlaylistItems != null && playlistFile.PlaylistItems.Count > 0) // Don't bother if playlist empty
            {
                for (int i = 0; i < 7; i++)
                {
                    if (Requests.Playlists.CreatePlaylist(
                           MainForm.Settings.AuthenticationCookie,
                           playlistFile.Title,
                           playlistFile.Description,
                           playlistFile.PlaylistItems.Select(m => m.VideoId).ToList(),
                           OnlinePlaylist.PrivacyStatusEmum.Private,
                           out string playlistId,
                           out string browseId,
                           out Exception e))
                    {
                        playlistFile.PlaylistId = browseId;
                        playlistFile.LastModifiedDate = new FileInfo(playlistFile.Path).LastWriteTime;
                        playlistFile.LastUpload = playlistFile.LastModifiedDate;
                        playlistFile.Save().Wait();

                        try
                        {
                            Requests.ArtistCache.Playlists = Requests.Playlists.GetPlaylists(MainForm.Settings.AuthenticationCookie);
                        }
                        catch { }

                        Logger.LogInfo("HandleOnlinePlaylistNeedsCreating", $"Created online playlist: {playlistFile.Title}: Success");
                        break;
                    }
                    else
                    {
                        if (i == 4)
                        {
                            Logger.Log(e, $"Error creating playlist: {playlistFile.Title}", Log.LogTypeEnum.Error);
                            break;
                        }
                        else
                            ThreadHelper.SafeSleep(3000);
                    }
                }
            }
        }

        private OnlinePlaylist MatchOnlinePlaylist(string playlistTitleOrId, bool isId)
        {
            if (OnlinePlaylists == null)
                return null;

            else
            {
                try
                {
                    if (isId)
                    {
                        return OnlinePlaylists.Where(p => p.BrowseId == playlistTitleOrId).FirstOrDefault();
                    }
                    else
                    {
                        foreach (var onlinePlaylist in OnlinePlaylists)
                        {
                            float playlistTitleSimilarity = Levenshtein.Similarity(playlistTitleOrId.UnQuote(), onlinePlaylist.Title.UnQuote());
                            if (playlistTitleSimilarity > 0.95)
                                return onlinePlaylist;
                        }

                        foreach (var onlinePlaylist in OnlinePlaylists)
                        {
                            float playlistTitleSimilarity = Levenshtein.Similarity(playlistTitleOrId.UnQuote(), onlinePlaylist.Title.UnQuote());
                            if (playlistTitleSimilarity > 0.9)
                                return onlinePlaylist;
                        }

                        foreach (var onlinePlaylist in OnlinePlaylists)
                        {
                            float playlistTitleSimilarity = Levenshtein.Similarity(playlistTitleOrId.UnQuote(), onlinePlaylist.Title.UnQuote());
                            if (playlistTitleSimilarity > 0.8)
                                return onlinePlaylist;
                        }

                        foreach (var onlinePlaylist in OnlinePlaylists)
                        {
                            float playlistTitleSimilarity = Levenshtein.Similarity(playlistTitleOrId.UnQuote(), onlinePlaylist.Title.UnQuote());
                            if (playlistTitleSimilarity > 0.75)
                                return onlinePlaylist;
                        }
                    }
                }
                catch { }
            }

            return null;
        }

        private void SetStatus(string statusText = null, string systemTrayIconText = null)
        {
            if (statusText != null)
                MainForm.SetStatusMessage(statusText);

            if (!string.IsNullOrEmpty(systemTrayIconText))
                MainForm.SetSystemTrayIconText(systemTrayIconText);
        }

        private void ConnectionCheckWait()
        {
            if (MainFormAborting())
                return;

            while (!NetworkHelper.InternetConnectionIsUp())
            {
                if (MainFormAborting())
                    return;

                ThreadHelper.SafeSleep(1000);
            }

            while (MainForm.ManagingYTMusicStatus == MainForm.ManagingYTMusicStatusEnum.Showing)
            {
                MainForm.SetPaused(true);
                ThreadHelper.SafeSleep(1000);
            }
        }

        private bool MainFormAborting()
        {
            if (MainForm.Aborting)
            {
                Stopped = true;
                MainForm.SetUploadingMessage("Restarting", "Restarting", null, true);
                return true;
            }

            return false;
        }
    }
}
