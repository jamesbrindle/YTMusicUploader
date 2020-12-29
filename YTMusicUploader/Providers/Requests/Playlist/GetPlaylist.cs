using JBToolkit.StreamHelpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Net;
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
        public partial class Playlist
        {
            /// <summary>
            /// HttpWebRequest POST request - Recursively fetches all the songs of an artist from YouTube Music's 'Upload' section
            /// </summary>
            /// <param name="cookieValue">Cookie from a previous YouTube Music sign in via this application (stored in the database)</param>
            /// <param name="browseId">YouTube Music's navigation ID for an individual artist, retreived from 'GetArtists' request</param>
            /// <param name="songs">Input ArtistCache 'Songs' object (should be empty when initialising - this is a recursive method)</param>
            /// <param name="continuationToken">Token from YouTube Music indicated more results to fetch, and the token to get them
            /// (shoult be empty when initialising - this is a recursive method)</param>
            /// <returns>ArtistCache object</returns>
            public static Playlist GetPlaylist(
            string cookieValue,
            string browseId,
            PlaylistSongCollection playlistSongCollection = null,
            string continuationToken = null)
            {
                var playlist = new Playlist();

                if (playlistSongCollection == null)
                    playlistSongCollection = new PlaylistSongCollection();

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
                            playlistSongCollection = GetInitalPlaylistSongs(playlistSongCollection, result, out string continuation);
                            if (!string.IsNullOrEmpty(continuation))
                                return GetPlaylist(cookieValue, browseId, playlistSongCollection, continuation);
                        }
                        else
                        {
                            playlistSongCollection = GetContinuationPlaylistSongs(playlistSongCollection, result, out string continuation);
                            if (!string.IsNullOrEmpty(continuation))
                                return GetPlaylist(cookieValue, browseId, playlistSongCollection, continuation);
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
                PlaylistSongCollection playlistSoongCollection, 
                string result, 
                out string continuation)
            {
                continuation = null;
                return null;
            }

            private static PlaylistSongCollection GetContinuationPlaylistSongs(
                PlaylistSongCollection playlistSoongCollection, 
                string result, 
                out string continuation)
            {
                continuation = null;
                return null;
            }
        }
    }
}
