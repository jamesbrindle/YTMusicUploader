using JBToolkit.StreamHelpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
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
        /// HttpWebRequest POST request - Recursively fetches all the artists from YouTube Music's 'Upload' section
        /// </summary>
        /// <param name="cookieValue">Cookie from a previous YouTube Music sign in via this application (stored in the database)</param>
        /// <param name="artistCache">Input ArtistCache object (should be empty when initialising - this is a recursive method)</param>
        /// <param name="continuationToken">Token from YouTube Music indicated more results to fetch, and the token to get them
        /// (shoult be empty when initialising - this is a recursive method)</param>
        /// <returns>ArtistCache object</returns>
        public static ArtistCache GetArtists(
            string cookieValue,
            ArtistCache artistCache = null,
            string continuationToken = null)
        {
            if (artistCache == null)
                artistCache = new ArtistCache();

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

                byte[] postBytes = GetPostBytes(
                                        SafeFileStream.ReadAllText(
                                                Path.Combine(Global.WorkingDirectory, @"AppData\get_artists_context.json")));

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
                        artistCache = GetInitialArtists(artistCache, result, out string continuation);
                        if (!string.IsNullOrEmpty(continuation))
                            return GetArtists(cookieValue, artistCache, continuation);
                    }
                    else
                    {
                        artistCache = GetArtistsContinuation(artistCache, result, out string continuation);
                        if (!string.IsNullOrEmpty(continuation))
                            return GetArtists(cookieValue, artistCache, continuation);
                    }
                }
            }
            catch (Exception e)
            {
                var _ = e;
#if DEBUG
                Console.Out.WriteLine("GetArtists: " + e.Message);
#endif
            }

            return artistCache;
        }

        private static ArtistCache GetInitialArtists(
            ArtistCache artistCache,
            string httpResponseResults,
            out string continuation)
        {
            continuation = string.Empty;
            var jo = JObject.Parse(httpResponseResults);
            var musicShelfRendererTokens = jo.Descendants().Where(t => t.Type == JTokenType.Property && ((JProperty)t).Name == "musicShelfRenderer")
                                                           .Select(p => ((JProperty)p).Value).ToList();

            foreach (JToken token in musicShelfRendererTokens)
            {
                var msr = token.ToObject<BrowseArtistsResultsContext.Musicshelfrenderer>();

                if (msr.continuations != null &&
                    msr.continuations.Length > 0 &&
                    msr.continuations[0].nextContinuationData != null &&
                    msr.continuations[0].nextContinuationData.continuation != null)
                {
                    continuation = msr.continuations[0].nextContinuationData.continuation;
                }

                foreach (var item in msr.contents)
                {
                    foreach (var run in item.musicResponsiveListItemRenderer
                                            .flexColumns[0]
                                            .musicResponsiveListItemFlexColumnRenderer
                                            .text
                                            .runs)
                    {
                        artistCache.Artists.Add(new ArtistCache.Artist
                        {
                            BrowseId = item.musicResponsiveListItemRenderer.navigationEndpoint.browseEndpoint.browseId,
                            ArtistName = run.text,
                        });
                    }
                }
            }

            return artistCache;
        }

        private static ArtistCache GetArtistsContinuation(
            ArtistCache artistCache,
            string httpResponseResults,
            out string continuation)
        {
            var browseArtistsResults = JsonConvert.DeserializeObject<BrowseArtistsResultsContinuationContext>(httpResponseResults);
            continuation = string.Empty;

            var musicShelfRenderer = browseArtistsResults.continuationContents.musicShelfContinuation;

            if (musicShelfRenderer.continuations != null &&
                musicShelfRenderer.continuations.Length > 0 &&
                musicShelfRenderer.continuations[0].nextContinuationData != null &&
                musicShelfRenderer.continuations[0].nextContinuationData.continuation != null)
            {
                continuation = musicShelfRenderer.continuations[0].nextContinuationData.continuation;
            }

            foreach (var item in musicShelfRenderer.contents)
            {
                foreach (var run in item.musicResponsiveListItemRenderer
                                        .flexColumns[0]
                                        .musicResponsiveListItemFlexColumnRenderer
                                        .text
                                        .runs)
                {
                    artistCache.Artists.Add(new ArtistCache.Artist
                    {
                        BrowseId = item.musicResponsiveListItemRenderer.navigationEndpoint.browseEndpoint.browseId,
                        ArtistName = run.text
                    });
                }
            }

            return artistCache;
        }
    }
}
