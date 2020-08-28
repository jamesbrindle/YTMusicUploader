using JBToolkit.StreamHelpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using YTMusicUploader.Providers.RequestModels;

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
        public static List<ArtistCache.Song> GetArtistSongs(
            string cookieValue,
            string browseId,
            List<ArtistCache.Song> songs = null,
            string continuationToken = null)
        {
            if (songs == null)
                songs = new List<ArtistCache.Song>();

            try
            {
                var request = (HttpWebRequest)WebRequest.Create(Global.YouTubeBaseUrl +
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
                    using (var brotli = new Brotli.BrotliStream(
                                                        response.GetResponseStream(),
                                                        System.IO.Compression.CompressionMode.Decompress,
                                                        true))
                    {
                        var streamReader = new StreamReader(brotli);
                        result = streamReader.ReadToEnd();
                    }

                    if (string.IsNullOrEmpty(continuationToken))
                    {
                        songs = GetInitalArtistSongs(songs, result, out string continuation);
                        if (!string.IsNullOrEmpty(continuation))
                            return GetArtistSongs(cookieValue, browseId, songs, continuation);
                    }
                    else
                    {
                        songs = GetContinuationArtistSongs(songs, result, out string continuation);
                        if (!string.IsNullOrEmpty(continuation))
                            return GetArtistSongs(cookieValue, browseId, songs, continuation);
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

            return songs;
        }

        private static List<ArtistCache.Song> GetInitalArtistSongs(
            List<ArtistCache.Song> songs,
            string httpResponseResults,
            out string continuation)
        {
            continuation = string.Empty;
            var jo = JObject.Parse(httpResponseResults);
            var musicShelfRendererTokens = jo.Descendants()
                                                      .Where(t => t.Type == JTokenType.Property && ((JProperty)t).Name == "musicShelfRenderer")
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
                    if (i != 0)
                    {
                        try
                        {
                            songs.Add(new ArtistCache.Song
                            {
                                Title = content.musicResponsiveListItemRenderer
                                               .flexColumns[0]
                                               .musicResponsiveListItemFlexColumnRenderer
                                               .text
                                               .runs[0]
                                               .text,

                                EntityId = GetEntityID(content.musicResponsiveListItemRenderer
                                                              .menu
                                                              .menuRenderer)
                            });
                        }
                        catch { }
                    }

                    i++;
                }
            }

            return songs;
        }

        private static List<ArtistCache.Song> GetContinuationArtistSongs(
            List<ArtistCache.Song> songs,
            string httpResponseResults,
            out string continuation)
        {
            continuation = string.Empty;
            var jo = JObject.Parse(httpResponseResults);
            var musicShelfRendererTokens = jo.Descendants()
                                                      .Where(t => t.Type == JTokenType.Property && ((JProperty)t).Name == "musicShelfContinuation")
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
                        songs.Add(new ArtistCache.Song
                        {
                            Title = content.musicResponsiveListItemRenderer
                                           .flexColumns[0]
                                           .musicResponsiveListItemFlexColumnRenderer
                                           .text
                                           .runs[0]
                                           .text,

                            EntityId = GetEntityID(content.musicResponsiveListItemRenderer
                                                          .menu
                                                          .menuRenderer)
                        });
                    }
                    catch { }
                }
            }

            return songs;
        }

        private static string GetEntityID(BrowseArtistResultsContext.Menurenderer menuRenderer)
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
    }
}
