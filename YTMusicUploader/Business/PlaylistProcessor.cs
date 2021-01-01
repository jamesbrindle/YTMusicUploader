using JBToolkit.FuzzyLogic;
using JBToolkit.Network;
using JBToolkit.Threads;
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
        public void Process()
        {
            Stopped = false;
            SetStatus("Processing playlist files", "Processing playlist files");

            try
            {
                PlaylistFiles = MainForm.PlaylistFileRepo.LoadAll().Result;
                OnlinePlaylists = Requests.ArtistCache.Playlists;

                foreach (var playlistFile in PlaylistFiles)
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
        }

        private void HandleOnlinePlaylistPresent(OnlinePlaylist onlinePlaylist, PlaylistFile playlistFile)
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

                    if (Requests.Playlists.AddPlaylistItem(
                        MainForm.Settings.AuthenticationCookie,
                        playlistFile.PlaylistId,
                        playlistItem.VideoId,
                        out string errorMessage))
                    {
                        Logger.LogInfo("HandleOnlinePlaylistPresent", $"Adding item to online playlist: {playlistFile.Title}: Success");
                    }
                    else
                    {
                        Logger.LogError("HandleOnlinePlaylistPresent", $"Error adding item to online playlist: {playlistFile.Title}: " + errorMessage);
                    }
                }
            }

            playlistFile.LastModifiedDate = new FileInfo(playlistFile.Path).LastWriteTime;
            playlistFile.LastUpload = playlistFile.LastModifiedDate;
            playlistFile.Save().Wait();
        }

        private void HandleOnlinePlaylistNeedsCreating(PlaylistFile playlistFile)
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

            if (Requests.Playlists.CreatePlaylist(
                    MainForm.Settings.AuthenticationCookie,
                    playlistFile.Title,
                    playlistFile.Description,
                    playlistFile.PlaylistItems.Select(m => m.VideoId).ToList(),
                    OnlinePlaylist.PrivacyStatusEmum.Private,
                    out string playlistId,
                    out string browseId,
                    out string errorMessage))
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
            }
            else
            {
                Logger.LogError("HandleOnlinePlaylistNeedsCreating", $"Error creating playlist: {playlistFile.Title}: " + errorMessage);
            }
        }

        private OnlinePlaylist MatchOnlinePlaylist(string playlistTitleOrId, bool isId)
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

            return null;
        }

        private void SetStatus(string statusText = null, string systemTrayIconText = null)
        {
            if (statusText != null)
                MainForm.SetStatusMessage(statusText);

            if (!string.IsNullOrEmpty(systemTrayIconText))
                MainForm.SetSystemTrayIconText(systemTrayIconText);
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
