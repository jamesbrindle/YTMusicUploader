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
        /// HttpWebRequest POST request to send to YouTube delete a YT music track fro the users uploads
        /// </summary>
        /// <param name="cookieValue">Cookie from a previous YouTube Music sign in via this application (stored in the database)</param>
        /// <param name="entityId">YT Music entity ID to delete</param>
        /// <returns>True if successfully authenticated, false otherwise</returns>
        public static bool DeleteAlbumOrTrackFromYTMusic(string cookieValue, string entityId, out string errorMessage)
        {
            errorMessage = string.Empty;

            try
            {
                var request = (HttpWebRequest)WebRequest.Create(
                                                        Global.YouTubeBaseUrl +
                                                        "music/delete_privately_owned_entity" +
                                                        Global.YouTubeMusicParams);

                request = AddStandardHeaders(request, cookieValue);

                request.ContentType = "application/json; charset=UTF-8";
                request.Headers["X-Goog-AuthUser"] = "0";
                request.Headers["x-origin"] = "https://music.youtube.com";
                request.Headers["X-Goog-Visitor-Id"] = Global.GoogleVisitorId;
                request.Headers["Authorization"] = GetAuthorisation(GetSAPISIDFromCookie(cookieValue));

                var context = JsonConvert.DeserializeObject<DeleteFromYTMusicRequestContext>(
                                              SafeFileStream.ReadAllText(
                                                                  Path.Combine(
                                                                          Global.WorkingDirectory,
                                                                          @"AppData\delete_song_context.json")));

                context.entityId = entityId;
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
