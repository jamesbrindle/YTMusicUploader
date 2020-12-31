using JBToolkit.StreamHelpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Net;
using YTMusicUploader.Providers.DataModels;
using YTMusicUploader.Providers.RequestModels;
using static YTMusicUploader.Providers.RequestModels.ArtistCache;

namespace YTMusicUploader.Providers
{
    /// <summary>
    /// YouTube Music API Request Methods
    /// 
    /// Thanks to: sigma67: 
    ///     https://ytmusicapi.readthedocs.io/en/latest/ 
    ///     https://github.com/sigma67/ytmusicapi
    /// </summary>
    public partial class Requests
    {
        /// <summary>
        /// YouTube Music API request methods specifically for playlist manipulation
        /// </summary>
        public partial class Playlists
        {
            /// <summary>
            /// HttpWebRequest POST request to send to YTM, which gets a playlist given a playlist or browse id 
            /// (You can get this from the 'Requests.Playlists.GetPlaylists (plural)') request method. And then recurisvely
            /// fetches all tracks listed in the playlist
            /// </summary>
            /// <param name="cookieValue">Cookie from a previous YouTube Music sign in via this application (stored in the database)</param>
            /// <param name="browseId">Playlist id or browse id to retreive the playlist for</param>
            /// <param name="playlist">Shuuld be null initially. This object is used for recursive purposes</param>
            /// <param name="continuationToken">Should be null initially. This object is used for recursive purposes.</param>
            /// <returns>OnlinePlaylist object</returns>
            public static OnlinePlaylist GetPlaylist(
            string cookieValue,
            string browseId,
            OnlinePlaylist playlist = null,
            string continuationToken = null)
            {
                if (playlist == null)
                    playlist = new OnlinePlaylist();

                if (playlist.Songs == null)
                    playlist.Songs = new PlaylistSongCollection();

                try
                {
                    var request = (HttpWebRequest)WebRequest.Create(Global.YTMusicBaseUrl +
                                                                    "browse" +
                                                                    (string.IsNullOrEmpty(continuationToken)
                                                                                    ? ""
                                                                                    : "?ctoken=" + continuationToken +
                                                                                      "&continuation=" + continuationToken) +
                                                                    (string.IsNullOrEmpty(continuationToken)
                                                                                    ? Global.YTMusicParams
                                                                                    : Global.YTMusicParams.Replace('?', '&')));
                    request = AddStandardHeaders(request, cookieValue);

                    request.ContentType = "application/json; charset=UTF-8";
                    request.Headers["X-Goog-AuthUser"] = "0";
                    request.Headers["x-origin"] = "https://music.youtube.com";
                    request.Headers["X-Goog-Visitor-Id"] = Global.GoogleVisitorId;
                    request.Headers["Authorization"] = GetAuthorisation(GetSAPISIDFromCookie(cookieValue));

                    var context = JsonConvert.DeserializeObject<BrowseArtistRequestContext>(
                                    SafeFileStream.ReadAllText(
                                                        Path.Combine(
                                                                Global.WorkingDirectory,
                                                                @"AppData\get_playlist_context.json")));

                    context.browseId = string.Format("{0}", browseId);
                    byte[] postBytes = GetPostBytes(JsonConvert.SerializeObject(context));
                    request.ContentLength = postBytes.Length;

                    using (var requestStream = request.GetRequestStream())
                    {
                        requestStream.Write(postBytes, 0, postBytes.Length);
                        requestStream.Close();
                    }

                    postBytes = null;
                    using (var response = (HttpWebResponse)request.GetResponse())
                    {
                        string result;
                        using (var brotli = new Brotli.BrotliStream(response.GetResponseStream(),
                                                                    System.IO.Compression.CompressionMode.Decompress,
                                                                    true))
                        {
                            var streamReader = new StreamReader(brotli);
                            result = streamReader.ReadToEnd();
                        }

                        if (string.IsNullOrEmpty(continuationToken))
                        {
                            var playListData = JsonConvert.DeserializeObject<BrowsePlaylistResultsContext>(result);
                            playlist.BrowseId = browseId;
                            playlist.Title = playListData.header
                                                         .musicEditablePlaylistDetailHeaderRenderer
                                                         .header
                                                         .musicDetailHeaderRenderer
                                                         .title
                                                         .runs[0]
                                                         .text;

                            playlist.Subtitle = playListData.header
                                                            .musicEditablePlaylistDetailHeaderRenderer
                                                            .header
                                                            .musicDetailHeaderRenderer
                                                            .subtitle.runs[0].text +
                                                playListData.header
                                                            .musicEditablePlaylistDetailHeaderRenderer
                                                            .header
                                                            .musicDetailHeaderRenderer
                                                            .subtitle.runs[1].text +
                                                playListData.header
                                                            .musicEditablePlaylistDetailHeaderRenderer
                                                            .header
                                                            .musicDetailHeaderRenderer
                                                            .subtitle.runs[2].text;

                            playlist.Description = playListData.header
                                                        .musicEditablePlaylistDetailHeaderRenderer
                                                        .editHeader
                                                        .musicPlaylistEditHeaderRenderer
                                                        .description != null
                                                            ? playListData.header
                                                                          .musicEditablePlaylistDetailHeaderRenderer
                                                                          .editHeader
                                                                          .musicPlaylistEditHeaderRenderer
                                                                          .description
                                                                          .runs[0]
                                                                          .text
                                                            : "";

                            playlist.Duration = playListData.header
                                                               .musicEditablePlaylistDetailHeaderRenderer
                                                               .header
                                                               .musicDetailHeaderRenderer
                                                               .secondSubtitle
                                                               .runs[2]
                                                               .text;

                            playlist.CoverArtUrl = playListData.header
                                                               .musicEditablePlaylistDetailHeaderRenderer
                                                               .header
                                                               .musicDetailHeaderRenderer
                                                               .thumbnail
                                                               .croppedSquareThumbnailRenderer
                                                               .thumbnail
                                                               .thumbnails[0].url;

                            try
                            {
                                playlist.PrivacyStatus = (OnlinePlaylist.PrivacyStatusEmum)Enum.Parse(
                                                            typeof(OnlinePlaylist.PrivacyStatusEmum),
                                                            playListData.header
                                                                    .musicEditablePlaylistDetailHeaderRenderer
                                                                    .editHeader
                                                                    .musicPlaylistEditHeaderRenderer
                                                                    .privacy,
                                                            true);
                            }
                            catch
                            {
                                playlist.PrivacyStatus = OnlinePlaylist.PrivacyStatusEmum.Private;
                            }

                            playlist.Songs = GetInitalPlaylistSongs(playlist.Songs, result, out string continuation);
                            if (!string.IsNullOrEmpty(continuation))
                                return GetPlaylist(cookieValue, browseId, playlist, continuation);
                        }
                        else
                        {
                            playlist.Songs = GetContinuationPlaylistSongs(playlist.Songs, result, out string continuation);
                            if (!string.IsNullOrEmpty(continuation))
                                return GetPlaylist(cookieValue, browseId, playlist, continuation);
                        }
                    }
                }
                catch (Exception e)
                {
                    var _ = e;
#if DEBUG
                    Console.Out.WriteLine("GetArtistSongs: " + e.Message);
#endif
                }

                return playlist;
            }

            private static PlaylistSongCollection GetInitalPlaylistSongs(
                PlaylistSongCollection playlistSongCollection,
                string result,
                out string continuation)
            {
                continuation = string.Empty;
                var jo = JObject.Parse(result);
                var musicShelfRendererTokens = jo.Descendants().Where(t => t.Type == JTokenType.Property && ((JProperty)t).Name == "musicPlaylistShelfRenderer")
                                                               .Select(p => ((JProperty)p).Value).ToList();

                foreach (var token in musicShelfRendererTokens)
                {
                    var msr = token.ToObject<BrowsePlaylistResultsContext.Musicplaylistshelfrenderer>();
                    if (msr.continuations != null &&
                        msr.continuations.Length > 0 &&
                        msr.continuations[0].nextContinuationData != null &&
                        msr.continuations[0].nextContinuationData.continuation != null)
                    {
                        continuation = msr.continuations[0].nextContinuationData.continuation;
                    }

                    int i = 0;
                    foreach (var content in msr.contents)
                    {
                        if (content.musicResponsiveListItemRenderer.fixedColumns != null)
                        {
                            try
                            {
                                string coverArtUrl = content.musicResponsiveListItemRenderer
                                                            .thumbnail
                                                            .musicThumbnailRenderer
                                                            .thumbnail
                                                            .thumbnails[0].url;

                                var song = new PlaylistSong
                                {
                                    Title = content.musicResponsiveListItemRenderer
                                                   .flexColumns[0]
                                                   .musicResponsiveListItemFlexColumnRenderer
                                                   .text
                                                   .runs[0]
                                                   .text,

                                    ArtistTitle = content.musicResponsiveListItemRenderer
                                                   .flexColumns[1]
                                                   .musicResponsiveListItemFlexColumnRenderer
                                                   .text
                                                   .runs != null
                                                        ? content.musicResponsiveListItemRenderer
                                                           .flexColumns[1]
                                                           .musicResponsiveListItemFlexColumnRenderer
                                                           .text
                                                           .runs[0]
                                                           .text
                                                        : "",

                                    AlbumTitle = content.musicResponsiveListItemRenderer
                                                   .flexColumns[2]
                                                   .musicResponsiveListItemFlexColumnRenderer
                                                   .text
                                                   .runs != null
                                                        ? content.musicResponsiveListItemRenderer
                                                           .flexColumns[2]
                                                           .musicResponsiveListItemFlexColumnRenderer
                                                           .text
                                                           .runs[0]
                                                           .text
                                                        : "",

                                    Duration = content.musicResponsiveListItemRenderer
                                                      .fixedColumns[0].musicResponsiveListItemFixedColumnRenderer
                                                      .text
                                                      .runs[0]
                                                      .text,

                                    VideoId = GetVideoEntityId(content.musicResponsiveListItemRenderer
                                                          .menu
                                                          .menuRenderer),

                                    SetVideoId = GetSetVideoEntityId(content.musicResponsiveListItemRenderer
                                                          .menu
                                                          .menuRenderer),

                                    CoverArtUrl = coverArtUrl
                                };

                                playlistSongCollection.Add(song);
                            }
                            catch (Exception e)
                            {
                                var _ = e;
#if DEBUG
                                Console.Out.WriteLine(e.Message);
#endif
                                Logger.Log(e, "GetInitalPlaylistSongs - Error fetching playlist items", Log.LogTypeEnum.Error);
                            }
                        }

                        i++;
                    }
                }

                return playlistSongCollection;
            }

            private static PlaylistSongCollection GetContinuationPlaylistSongs(
                PlaylistSongCollection playlistSongCollection,
                string result,
                out string continuation)
            {
                continuation = string.Empty;
                var jo = JObject.Parse(result);
                var musicShelfRendererTokens = jo.Descendants().Where(t => t.Type == JTokenType.Property && ((JProperty)t).Name == "musicPlaylistShelfContinuation")
                                                               .Select(p => ((JProperty)p).Value).ToList();

                foreach (var token in musicShelfRendererTokens)
                {
                    var msr = token.ToObject<BrowsePlaylistResultsContext.Musicplaylistshelfrenderer>();
                    if (msr.continuations != null &&
                        msr.continuations.Length > 0 &&
                        msr.continuations[0].nextContinuationData != null &&
                        msr.continuations[0].nextContinuationData.continuation != null)
                    {
                        continuation = msr.continuations[0].nextContinuationData.continuation;
                    }

                    int i = 0;
                    foreach (var content in msr.contents)
                    {
                        if (content.musicResponsiveListItemRenderer.fixedColumns != null)
                        {
                            try
                            {
                                string coverArtUrl = content.musicResponsiveListItemRenderer
                                                            .thumbnail
                                                            .musicThumbnailRenderer
                                                            .thumbnail
                                                            .thumbnails[0].url;

                                var song = new PlaylistSong
                                {
                                    Title = content.musicResponsiveListItemRenderer
                                                   .flexColumns[0]
                                                   .musicResponsiveListItemFlexColumnRenderer
                                                   .text
                                                   .runs[0]
                                                   .text,

                                    ArtistTitle = content.musicResponsiveListItemRenderer
                                                   .flexColumns[1]
                                                   .musicResponsiveListItemFlexColumnRenderer
                                                   .text
                                                   .runs != null
                                                        ? content.musicResponsiveListItemRenderer
                                                           .flexColumns[1]
                                                           .musicResponsiveListItemFlexColumnRenderer
                                                           .text
                                                           .runs[0]
                                                           .text
                                                        : "",

                                    AlbumTitle = content.musicResponsiveListItemRenderer
                                                   .flexColumns[2]
                                                   .musicResponsiveListItemFlexColumnRenderer
                                                   .text
                                                   .runs != null
                                                        ? content.musicResponsiveListItemRenderer
                                                           .flexColumns[2]
                                                           .musicResponsiveListItemFlexColumnRenderer
                                                           .text
                                                           .runs[0]
                                                           .text
                                                        : "",

                                    Duration = content.musicResponsiveListItemRenderer
                                                      .fixedColumns[0].musicResponsiveListItemFixedColumnRenderer
                                                      .text
                                                      .runs[0]
                                                      .text,

                                    VideoId = GetVideoEntityId(content.musicResponsiveListItemRenderer
                                                          .menu
                                                          .menuRenderer),

                                    CoverArtUrl = coverArtUrl
                                };

                                playlistSongCollection.Add(song);
                            }
                            catch (Exception e)
                            {
                                var _ = e;
#if DEBUG
                                Console.Out.WriteLine(e.Message);
#endif
                                Logger.Log(e, "GetContinuationPlaylistSongs - Error fetching playlist items", Log.LogTypeEnum.Error);
                            }
                        }

                        i++;
                    }
                }

                return playlistSongCollection;
            }
        }

        private static string GetVideoEntityId(BrowsePlaylistResultsContext.Menurenderer menuRenderer)
        {
            foreach (var item in menuRenderer.items)
            {
                if (item.menuServiceItemRenderer != null)
                {
                    try
                    {
                        if (item.menuServiceItemRenderer.text.runs[0].text.ToLower() == "remove from playlist")
                            return item.menuServiceItemRenderer
                                       .serviceEndpoint
                                       .playlistEditEndpoint
                                       .actions[0].removedVideoId;
                    }
                    catch { }
                }
            }

            return string.Empty;
        }

        private static string GetSetVideoEntityId(BrowsePlaylistResultsContext.Menurenderer menuRenderer)
        {
            foreach (var item in menuRenderer.items)
            {
                if (item.menuServiceItemRenderer != null)
                {
                    try
                    {
                        if (item.menuServiceItemRenderer.text.runs[0].text.ToLower() == "remove from playlist")
                            return item.menuServiceItemRenderer
                                       .serviceEndpoint
                                       .playlistEditEndpoint
                                       .actions[0].setVideoId;
                    }
                    catch { }
                }
            }

            return string.Empty;
        }
    }
}
