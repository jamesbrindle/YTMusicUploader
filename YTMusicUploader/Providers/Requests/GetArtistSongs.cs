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
        /// <summary>
        /// HttpWebRequest POST request - Recursively fetches all the songs of an artist from YouTube Music's 'Upload' section
        /// </summary>
        /// <param name="cookieValue">Cookie from a previous YouTube Music sign in via this application (stored in the database)</param>
        /// <param name="browseId">YouTube Music's navigation ID for an individual artist, retreived from 'GetArtists' request</param>
        /// <param name="songs">Input ArtistCache 'Songs' object (should be empty when initialising - this is a recursive method)</param>
        /// <param name="continuationToken">Token from YouTube Music indicated more results to fetch, and the token to get them
        /// (shoult be empty when initialising - this is a recursive method)</param>
        /// <returns>ArtistCache object</returns>
        public static AlbumSongCollection GetArtistSongs(
            string cookieValue,
            string browseId,
            AlbumSongCollection albumSongCollection = null,
            string continuationToken = null)
        {
            if (albumSongCollection == null)
                albumSongCollection = new AlbumSongCollection();

            if (albumSongCollection.Songs == null)
                albumSongCollection.Songs = new SongCollection();

            if (albumSongCollection.Albums == null)
                albumSongCollection.Albums = new AlbumCollection();

            try
            {
                var request = (HttpWebRequest)WebRequest.Create(Global.YouTubeMusicBaseUrl +
                                                                "browse" +
                                                                (string.IsNullOrEmpty(continuationToken)
                                                                                ? ""
                                                                                : "?ctoken=" + continuationToken +
                                                                                  "&continuation=" + continuationToken) +
                                                                (string.IsNullOrEmpty(continuationToken)
                                                                                ? Global.YouTubeMusicParams
                                                                                : Global.YouTubeMusicParams.Replace('?', '&')));
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
                                                            @"AppData\get_artist_context.json")));

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
                        albumSongCollection = GetInitalArtistSongs(albumSongCollection, result, out string continuation);
                        if (!string.IsNullOrEmpty(continuation))
                            return GetArtistSongs(cookieValue, browseId, albumSongCollection, continuation);
                    }
                    else
                    {
                        albumSongCollection = GetContinuationArtistSongs(albumSongCollection, result, out string continuation);
                        if (!string.IsNullOrEmpty(continuation))
                            return GetArtistSongs(cookieValue, browseId, albumSongCollection, continuation);
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

            return albumSongCollection;
        }

        private static AlbumSongCollection GetInitalArtistSongs(
            AlbumSongCollection albumSongCollection,
            string httpResponseResults,
            out string continuation)
        {
            continuation = string.Empty;
            var jo = JObject.Parse(httpResponseResults);
            var musicShelfRendererTokens = jo.Descendants().Where(t => t.Type == JTokenType.Property && ((JProperty)t).Name == "musicShelfRenderer")
                                                           .Select(p => ((JProperty)p).Value).ToList();

            foreach (JToken token in musicShelfRendererTokens)
            {
                var msr = token.ToObject<BrowseArtistResultsContext.Musicshelfrenderer>();
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

                            var song = new Song
                            {
                                Title = content.musicResponsiveListItemRenderer
                                               .flexColumns[0]
                                               .musicResponsiveListItemFlexColumnRenderer
                                               .text
                                               .runs[0]
                                               .text,

                                Duration = content.musicResponsiveListItemRenderer
                                                  .fixedColumns[0].musicResponsiveListItemFixedColumnRenderer
                                                  .text
                                                  .runs[0]
                                                  .text,

                                CoverArtUrl = coverArtUrl,

                                EntityId = GetTrackEntityID(content.musicResponsiveListItemRenderer
                                                                   .menu
                                                                   .menuRenderer)
                            };

                            bool isSingle = true;
                            string albumTitle = "[Singles]";
                            if (content.musicResponsiveListItemRenderer
                                       .flexColumns[2]
                                       .musicResponsiveListItemFlexColumnRenderer
                                       .text
                                       .runs != null)
                            {
                                isSingle = false;
                                albumTitle = content.musicResponsiveListItemRenderer
                                                    .flexColumns[2]
                                                    .musicResponsiveListItemFlexColumnRenderer
                                                    .text
                                                    .runs[0]
                                                    .text;
                            }

                            if (!albumSongCollection.Albums.AlbumHashSet.Contains(albumTitle))
                            {
                                albumSongCollection.Albums.AlbumHashSet.Add(albumTitle);
                                albumSongCollection.Albums.Add(new Alumb
                                {
                                    Title = albumTitle,
                                    CoverArtUrl = coverArtUrl,
                                    Songs = new SongCollection(),
                                    EntityId = isSingle ? "[Single]"
                                                        : GetAlbumEntityID(content.musicResponsiveListItemRenderer
                                                                                  .flexColumns[2]
                                                                                  .musicResponsiveListItemFlexColumnRenderer)
                                });
                            }

                            albumSongCollection.Songs.Add(song);
                            if (albumSongCollection.Albums.Where(m => m.Title == albumTitle).Any())
                                albumSongCollection.Albums.Where(m => m.Title == albumTitle).FirstOrDefault().Songs.Add(song);

                        }
                        catch (Exception e)
                        {
                            var _ = e;
#if DEBUG
                            Console.Out.WriteLine(e.Message);
#endif
                        }
                    }

                    i++;
                }
            }

            return albumSongCollection;
        }

        private static AlbumSongCollection GetContinuationArtistSongs(
            AlbumSongCollection albumSongCollection,
            string httpResponseResults,
            out string continuation)
        {
            continuation = string.Empty;
            var jo = JObject.Parse(httpResponseResults);
            var musicShelfRendererTokens = jo.Descendants().Where(t => t.Type == JTokenType.Property && ((JProperty)t).Name == "musicShelfContinuation")
                                                           .Select(p => ((JProperty)p).Value).ToList();

            var result = JsonConvert.DeserializeObject<BrowseArtistResultsContinuationContext>(httpResponseResults);

            foreach (JToken token in musicShelfRendererTokens)
            {
                var msr = token.ToObject<BrowseArtistResultsContext.Musicshelfrenderer>();
                if (msr.continuations != null &&
                    msr.continuations.Length > 0 &&
                    msr.continuations[0].nextContinuationData != null &&
                    msr.continuations[0].nextContinuationData.continuation != null)
                {
                    continuation = msr.continuations[0].nextContinuationData.continuation;
                }

                foreach (var content in msr.contents)
                {
                    try
                    {
                        string coverArtUrl = content.musicResponsiveListItemRenderer
                                                        .thumbnail
                                                        .musicThumbnailRenderer
                                                        .thumbnail
                                                        .thumbnails[0].url;

                        var song = new Song
                        {
                            Title = content.musicResponsiveListItemRenderer
                                           .flexColumns[0]
                                           .musicResponsiveListItemFlexColumnRenderer
                                           .text
                                           .runs[0]
                                           .text,

                            CoverArtUrl = coverArtUrl,

                            Duration = content.musicResponsiveListItemRenderer
                                              .fixedColumns[0]
                                              .musicResponsiveListItemFixedColumnRenderer
                                              .text
                                              .runs[0]
                                              .text,

                            EntityId = GetTrackEntityID(content.musicResponsiveListItemRenderer
                                                          .menu
                                                          .menuRenderer)
                        };

                        bool isSingle = true;
                        string albumTitle = "[Singles]";

                        if (content.musicResponsiveListItemRenderer
                                   .flexColumns[2]
                                   .musicResponsiveListItemFlexColumnRenderer
                                   .text
                                   .runs != null)
                        {
                            isSingle = false;
                            albumTitle = content.musicResponsiveListItemRenderer
                                                .flexColumns[2]
                                                .musicResponsiveListItemFlexColumnRenderer
                                                .text
                                                .runs[0]
                                                .text;
                        }

                        if (!albumSongCollection.Albums.AlbumHashSet.Contains(albumTitle))
                        {
                            albumSongCollection.Albums.AlbumHashSet.Add(albumTitle);
                            albumSongCollection.Albums.Add(new Alumb
                            {
                                Title = albumTitle,
                                CoverArtUrl = coverArtUrl,
                                Songs = new SongCollection(),
                                EntityId = isSingle ? "[Single]"
                                                    : GetAlbumEntityID(content.musicResponsiveListItemRenderer
                                                                              .flexColumns[2]
                                                                              .musicResponsiveListItemFlexColumnRenderer)
                            });
                        }

                        albumSongCollection.Songs.Add(song);
                        if (albumSongCollection.Albums.Where(m => m.Title == albumTitle).Any())
                            albumSongCollection.Albums.Where(m => m.Title == albumTitle).FirstOrDefault().Songs.Add(song);
                    }
                    catch (Exception e)
                    {
                        var _ = e;
#if DEBUG
                        Console.Out.WriteLine(e.Message);
#endif
                    }
                }
            }

            return albumSongCollection;
        }

        private static string GetTrackEntityID(BrowseArtistResultsContext.Menurenderer menuRenderer)
        {
            foreach (var item in menuRenderer.items)
            {
                if (item.menuNavigationItemRenderer != null)
                {
                    try
                    {
                        if (item.menuNavigationItemRenderer.text.runs[0].text.ToLower() == "delete song")
                            return item.menuNavigationItemRenderer
                                       .navigationEndpoint
                                       .confirmDialogEndpoint
                                       .content.confirmDialogRenderer
                                       .confirmButton.buttonRenderer
                                       .command
                                       .musicDeletePrivatelyOwnedEntityCommand
                                       .entityId;
                    }
                    catch { }
                }
            }

            return string.Empty;
        }

        private static string GetAlbumEntityID(BrowseArtistResultsContext.Musicresponsivelistitemflexcolumnrenderer menuRenderer)
        {
            return menuRenderer.text
                               .runs[0]
                               .navigationEndpoint
                               .browseEndpoint
                               .browseId
                               .Replace("FEmusic_library_privately_owned_release_detail", "");
        }
    }
}
