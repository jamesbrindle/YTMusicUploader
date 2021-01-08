using JBToolkit.StreamHelpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using YTMusicUploader.Providers.RequestModels;
using static YTMusicUploader.Providers.RequestModels.ArtistCache.OnlinePlaylist;

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
            /// HttpWebRequest POST request to send to YTM to create a new playlist
            /// </summary>
            /// <param name="cookieValue">Cookie from a previous YouTube Music sign in via this application (stored in the database)</param>
            /// <param name="title">Title to call the playlist</param>
            /// <param name="description">Description of the playlist (does not allow HTML tags)</param>
            /// <param name="videoIds">List of video ids (track ids) to create the playlist with</param>
            /// <param name="privacyStatus">PRIVATE, PUBLIC or UNLISTED - Default it PUBLIC</param>
            /// <param name="playlistId">(Output) playlist id once created</param>
            /// <param name="browseId">(Output) browse id once create (It's just the playlist ID prefixed with 'VL')</param>
            /// <param name="errorMessage">(Output)Any error message encountered during the request</param>
            /// <returns>True if request is successful, false otherwise</returns>
            public static bool CreatePlaylist(
                string cookieValue,
                string title,
                string description,
                List<string> videoIds,
                PrivacyStatusEmum privacyStatus,
                out string playlistId,
                out string browseId,
                out Exception ex)
            {
                ex = null;
                string originalRequest = string.Empty;
                browseId = string.Empty;
                playlistId = string.Empty;

                if (description == null)
                    description = string.Empty;

                try
                {
                    var request = (HttpWebRequest)WebRequest.Create(
                                                            Global.YTMusicBaseUrl +
                                                            "playlist/create" +
                                                            Global.YTMusicParams);

                    request = AddStandardHeaders(request, cookieValue);

                    request.ContentType = "application/json; charset=UTF-8";
                    request.Headers["X-Goog-AuthUser"] = "0";
                    request.Headers["x-origin"] = "https://music.youtube.com";
                    request.Headers["X-Goog-Visitor-Id"] = Global.GoogleVisitorId;
                    request.Headers["Authorization"] = GetAuthorisation(GetSAPISIDFromCookie(cookieValue));

                    var context = JsonConvert.DeserializeObject<CreatePlaylistRequestContext>(
                                                  SafeFileStream.ReadAllText(
                                                                      Path.Combine(
                                                                              Global.WorkingDirectory,
                                                                              @"AppData\create_playlist_context.json")));

                    context.title = title;
                    context.description = description;
                    context.privacyStatus = privacyStatus.ToString().ToUpper();
                    context.videoIds = videoIds.ToArray();

                    var delta = TimeZoneInfo.Local.GetUtcOffset(DateTime.Now);
                    double utcMinuteOffset = delta.TotalMinutes;
                    context.context.client.utcOffsetMinutes = (int)utcMinuteOffset;

                    try
                    {
                        originalRequest = JsonConvert.SerializeObject(context);
                    }
                    catch { }

                    byte[] postBytes = GetPostBytes(JsonConvert.SerializeObject(context));
                    request.ContentLength = postBytes.Length;

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

                        if (result.ToLower().Contains("error"))
                            throw new Exception("Error: " + result + ": Original Http Request: " + originalRequest);
                        else
                        {
                            var runObject = JObject.Parse(result);
                            var browseIds = runObject.Descendants()
                                                .Where(t => t.Type == JTokenType.Property && ((JProperty)t).Name == "browseId")
                                                .Select(p => ((JProperty)p).Value).ToList();

                            if (browseIds.Count > 0)
                                browseId = browseIds[0].ToString();

                            var playlistIds = runObject.Descendants()
                                                .Where(t => t.Type == JTokenType.Property && ((JProperty)t).Name == "playlistId")
                                                .Select(p => ((JProperty)p).Value).ToList();

                            if (playlistIds.Count > 0)
                                playlistId = playlistIds[0].ToString();
                        }
                    }
                }
                catch (Exception e)
                {
                    Logger.LogError("CreatePlaylist", "Error creating playlist: " + title, false, originalRequest);
                    ex = e;
                    return false;
                }

                return true;
            }
        }
    }
}