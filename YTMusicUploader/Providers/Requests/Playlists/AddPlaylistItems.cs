using JBToolkit.StreamHelpers;
using Newtonsoft.Json;
using System;
using System.IO;
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
        /// YouTube Music API request methods specifically for playlist manipulation
        /// </summary>
        public partial class Playlists
        {
            /// <summary>
            /// HttpWebRequest POST request to send to add a playlist item (track) to an existing YouTube Music playlist
            /// </summary>
            /// <param name="cookieValue">Cookie from a previous YouTube Music sign in via this application (stored in the database)</param>
            /// <param name="playlistId">YT Music playlistid (or browseId) to add to</param>
            /// <param name="videoId">This is the unique track id to add</param>
            /// <returns>True if successfully, false otherwise</returns>
            public static bool AddPlaylistItem(string cookieValue, string playlistId, string videoId, out Exception ex)
            {
                ex = null;
                string originalRequest = string.Empty;

                try
                {
                    var request = (HttpWebRequest)WebRequest.Create(
                                                            Global.YTMusicBaseUrl +
                                                            "browse/edit_playlist" +
                                                            Global.YTMusicParams);

                    request = AddStandardHeaders(request, cookieValue);

                    request.ContentType = "application/json; charset=UTF-8";
                    request.Headers["X-Goog-AuthUser"] = "0";
                    request.Headers["x-origin"] = "https://music.youtube.com";
                    request.Headers["X-Goog-Visitor-Id"] = Global.GoogleVisitorId;
                    request.Headers["Authorization"] = GetAuthorisation(GetSAPISIDFromCookie(cookieValue));

                    var context = JsonConvert.DeserializeObject<AddPlaylistItemContext>(
                                                  SafeFileStream.ReadAllText(
                                                                      Path.Combine(
                                                                              Global.WorkingDirectory,
                                                                              @"AppData\add_playlist_item_context.json")));

                    if (playlistId.StartsWith("VL"))
                        playlistId = playlistId.Substring(2, playlistId.Length - 2);

                    context.playlistId = playlistId;
                    context.actions[0].addedVideoId = videoId;

                    var delta = TimeZoneInfo.Local.GetUtcOffset(DateTime.Now);
                    double utcMinuteOffset = delta.TotalMinutes;
                    context.context.client.utcOffsetMinutes = (int)utcMinuteOffset;

                    byte[] postBytes = GetPostBytes(JsonConvert.SerializeObject(context));

                    try
                    {
                        originalRequest = JsonConvert.SerializeObject(context);
                    }
                    catch { }

                    request.ContentLength = postBytes.Length;
                    using (var requestStream = request.GetRequestStream())
                    {
                        requestStream.Write(postBytes, 0, postBytes.Length);
                        requestStream.Close();
                    }

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
                            throw new Exception("Error: " + result);
                    }
                }
                catch
                {
                    return false;
                }

                return true;
            }
        }
    }
}
