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
            /// HttpWebRequest POST request to send to YTM to remove a playlist item (track) from an existing playlist
            /// </summary>
            /// <param name="cookieValue">Cookie from a previous YouTube Music sign in via this application (stored in the database)</param>
            /// <param name="playlistId">Playlist id to remove a track from</param>
            /// <param name="videoEntityId">VideoId (unique track entityId) of the track to remove</param>
            /// <param name="setVideoEntityId">SetVideoId (alt unbique track entityId) of the track to remove</param>
            /// <param name="errorMessage">(Output) Error message if error encountered while sending the request</param>
            /// <returns>True if the request was successful, false otherwise</returns>
            public static bool RemovePlaylistItems(
                string cookieValue,
                string playlistId,
                string videoEntityId,
                string setVideoEntityId,
                out string errorMessage)
            {
                errorMessage = string.Empty;

                if (playlistId.StartsWith("VL"))
                    playlistId = playlistId.Substring(2, playlistId.Length - 2);

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

                    var context = JsonConvert.DeserializeObject<DeletePlaylistItemRequestContext>(
                                                  SafeFileStream.ReadAllText(
                                                                      Path.Combine(
                                                                              Global.WorkingDirectory,
                                                                              @"AppData\delete_playlist_item_context.json")));

                    context.playlistId = playlistId;
                    context.actions[0].setVideoId = setVideoEntityId;
                    context.actions[0].removedVideoId = videoEntityId;

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
                        {
                            errorMessage = "Error: " + result;
                            return false;
                        }
                    }
                }
                catch (Exception e)
                {
                    errorMessage = "Error: " + e.Message;
                    return false;
                }

                return true;
            }
        }
    }
}
