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
            /// HttpWebRequest POST request to send to YTM which fetches a list (collection) of Playlists (without playlist tracks).
            /// Use the 'Requests.Playlists.GetPlaylist (singular)' method to get an individual playlist complete with track listing.
            /// </summary>
            /// <param name="cookieValue">Cookie from a previous YouTube Music sign in via this application (stored in the database)</param>
            /// <returns>OnlinePlaylistCollection object (list of playlists without tracks)</returns>
            public static OnlinePlaylistCollection GetPlaylists(
                string cookieValue,
                string continuationToken = null,
                OnlinePlaylistCollection playListCol = null)
            {
                if (playListCol == null)
                    playListCol = new OnlinePlaylistCollection();

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

                    byte[] postBytes = GetPostBytes(
                                            SafeFileStream.ReadAllText(
                                                    Path.Combine(Global.WorkingDirectory, @"AppData\get_playlists_context.json")));

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

                            if (string.IsNullOrEmpty(continuationToken))
                            {
                                var playListsResultContext = JsonConvert.DeserializeObject<BrowsePlaylistsResultsContext>(result);
                                var playListResults = playListsResultContext.contents
                                                                            .singleColumnBrowseResultsRenderer
                                                                            .tabs[0]
                                                                            .tabRenderer
                                                                            .content
                                                                            .sectionListRenderer
                                                                            .contents[1]
                                                                            .itemSectionRenderer
                                                                            .contents[0]
                                                                            .gridRenderer
                                                                            .items;

                                foreach (var item in playListResults)
                                {
                                    if (item.musicTwoRowItemRenderer.title.runs[0].text != "New playlist" &&
                                        item.musicTwoRowItemRenderer.title.runs[0].text != "Your likes")
                                    {
                                        try
                                        {
                                            playListCol.Add(new OnlinePlaylist
                                            {
                                                Title = item.musicTwoRowItemRenderer.title.runs[0].text,
                                                Subtitle = item.musicTwoRowItemRenderer.subtitle.runs[0].text +
                                                       item.musicTwoRowItemRenderer.subtitle.runs[1].text +
                                                       item.musicTwoRowItemRenderer.subtitle.runs[2].text,
                                                BrowseId = item.musicTwoRowItemRenderer.navigationEndpoint.browseEndpoint.browseId,
                                                CoverArtUrl = item.musicTwoRowItemRenderer.thumbnailRenderer.musicThumbnailRenderer.thumbnail.thumbnails[0].url
                                            });
                                        }
                                        catch (Exception e)
                                        {
                                            Logger.Log(e, "GetPlaylists - Error fetching a playlist", Log.LogTypeEnum.Error);
                                        }
                                    }
                                }

                                string continuation = string.Empty;
                                var jo = JObject.Parse(result);
                                var musicShelfRendererTokens = jo.Descendants().Where(t => t.Type == JTokenType.Property && ((JProperty)t).Name == "itemSectionRenderer")
                                                                               .Select(p => ((JProperty)p).Value)
                                                                               .ToList();

                                foreach (var token in musicShelfRendererTokens)
                                {
                                    Console.Out.WriteLine(token);

                                    var msr = token.ToObject<BrowsePlaylistsResultsContext.Itemsectionrenderer>();
                                    if (msr != null &&
                                        msr.contents[0].gridRenderer.continuations != null &&
                                        msr.contents[0].gridRenderer.continuations.Length > 0 &&
                                        msr.contents[0].gridRenderer.continuations[0].nextContinuationData != null &&
                                        msr.contents[0].gridRenderer.continuations[0].nextContinuationData.continuation != null)
                                    {
                                        continuation = msr.contents[0].gridRenderer.continuations[0].nextContinuationData.continuation;
                                    }
                                }

                                if (!string.IsNullOrEmpty(continuation))
                                    return GetPlaylists(cookieValue, continuation, playListCol);
                            }
                            else
                            {
                                var playListsResultContext = JsonConvert.DeserializeObject<BrowsePlaylistsResultsContinuationContext>(result);
                                var playListResults = playListsResultContext.continuationContents
                                                                            .gridContinuation
                                                                            .items;

                                foreach (var item in playListResults)
                                {
                                    if (item.musicTwoRowItemRenderer.title.runs[0].text != "New playlist" &&
                                        item.musicTwoRowItemRenderer.title.runs[0].text != "Your likes")
                                    {
                                        try
                                        {
                                            playListCol.Add(new OnlinePlaylist
                                            {
                                                Title = item.musicTwoRowItemRenderer.title.runs[0].text,
                                                Subtitle = item.musicTwoRowItemRenderer.subtitle.runs[0].text +
                                                       item.musicTwoRowItemRenderer.subtitle.runs[1].text +
                                                       item.musicTwoRowItemRenderer.subtitle.runs[2].text,
                                                BrowseId = item.musicTwoRowItemRenderer.navigationEndpoint.browseEndpoint.browseId,
                                                CoverArtUrl = item.musicTwoRowItemRenderer.thumbnailRenderer.musicThumbnailRenderer.thumbnail.thumbnails[0].url
                                            });
                                        }
                                        catch (Exception e)
                                        {
                                            Logger.Log(e, "GetPlaylists - Error fetching a playlist", Log.LogTypeEnum.Error);
                                        }
                                    }
                                }

                                string continuation = string.Empty;
                                var jo = JObject.Parse(result);
                                var musicShelfRendererTokens = jo.Descendants().Where(t => t.Type == JTokenType.Property && ((JProperty)t).Name == "continuationContents")
                                                                               .Select(p => ((JProperty)p).Value)
                                                                               .ToList();

                                foreach (var token in musicShelfRendererTokens)
                                {
                                    Console.Out.WriteLine(token);

                                    var msr = token.ToObject<BrowsePlaylistsResultsContinuationContext.Continuationcontents>();
                                    if (msr != null &&
                                        msr.gridContinuation != null &&
                                        msr.gridContinuation.continuations != null &&
                                        msr.gridContinuation.continuations.Length > 0 &&
                                        msr.gridContinuation.continuations[0].nextContinuationData != null &&
                                        msr.gridContinuation.continuations[0].nextContinuationData.continuation != null)
                                    {
                                        continuation = msr.gridContinuation.continuations[0].nextContinuationData.continuation;
                                    }
                                }

                                if (!string.IsNullOrEmpty(continuation))
                                    return GetPlaylists(cookieValue, continuation, playListCol);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    var _ = e;
#if DEBUG
                    Console.Out.WriteLine("GetPlaylists: " + e.Message);
#endif
                }

                return playListCol;
            }
        }
    }
}
