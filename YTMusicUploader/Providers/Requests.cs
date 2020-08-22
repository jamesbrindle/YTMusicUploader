using System;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace YTMusicUploader.Providers
{
    /// <summary>
    /// YouTube music HttpWebRequest abstract class
    /// </summary>
    public partial class Requests
    {
        /// <summary>
        /// Main API URL for YouTube music
        /// </summary>
        public static string YouTubeBaseUrl
        {
            get
            {
                return "https://music.youtube.com/youtubei/v1/";
            }
        }

        /// <summary>
        /// Upload specific API URL
        /// </summary>
        public static string UploadUrl
        {
            get
            {
                return "https://upload.youtube.com/upload/usermusic/http?authuser=0";
            }
        }

        /// <summary>
        /// Main URL parameters for typical YouTube music API calls
        /// </summary>
        public static string Params
        {
            get
            {
                return "?alt=json&key=AIzaSyC9XL3ZjWddXya6X74dJoCTL-WEYFDNX30";
            }
        }

        /// <summary>
        /// Required headers for any YouTube music API request
        /// </summary>
        /// <param name="webRequest">HttpWebRequest to add the headers to</param>
        /// <param name="cookieValue">Cookie from a previous YouTube Music sign in via this application (stored in the database)</param>
        /// <returns>The same HttpWebReqest object with added default headers</returns>
        public static HttpWebRequest AddStandardHeaders(HttpWebRequest webRequest, string cookieValue)
        {
            webRequest.Accept = "*/*";
            webRequest.Headers["Accept-Encoding"] = "gzip, deflate, br";
            webRequest.UserAgent = "Mozilla / 5.0(Windows NT 10.0; Win64; x64; rv: 72.0) Gecko / 20100101 Firefox / 72.0";
            webRequest.Headers["Cookie"] = cookieValue;
            webRequest.Method = "POST";

            return webRequest;
        }

        /// <summary>
        /// Converts a string to a byte array for use in a HttpWebRequest upload stream (UTF8 encoded).
        /// </summary>
        /// <param name="stringToEncode">String to convert to a UTF8 encoded byte array</param>
        /// <returns>UTF8 encoded byte arra</returns>
        public static byte[] GetPostBytes(string stringToEncode)
        {
            byte[] bytes = Encoding.Default.GetBytes(stringToEncode);
            string body = Encoding.UTF8.GetString(bytes);
            return Encoding.UTF8.GetBytes(body);
        }

        /// <summary>
        /// Strips the SAPISID value from the cookie
        /// </summary>
        /// <param name="cookieValue">Cookie from a previous YouTube Music sign in via this application (stored in the database)</param>
        /// <returns>The SAPISID value from the the cookie</returns>
        private static string GetSAPISIDFromCookie(string cookieValue)
        {
            string[] parts = Regex.Split(cookieValue, "SAPISID=");
            string[] parts2 = parts[1].Split(';');
            return parts2[0];
        }

        /// <summary>
        /// Thanks to: Dave Thomas
        ///     https://stackoverflow.com/users/984724/dave-thomas
        ///     https://stackoverflow.com/a/32065323/5726546
        ///     
        /// For reverse engineering generating the SAPISIDHASH hash from the cookie value for use in the HttpWebRequest
        /// 'Authorization' header so we can be authenticated with YouTube music to utilise it's API.
        /// </summary>
        /// <param name="sapisid">SAPISID which is from the cookie value</param>
        /// <returns>SAPISID hash</returns>
        private static string GetAuthorisation(string sapisid)
        {
            int unixTimestamp = (int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            var hash = new SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(unixTimestamp + " " + sapisid + " https://music.youtube.com"));
            var sb = new StringBuilder(hash.Length * 2);

            foreach (byte b in hash)
                sb.Append(b.ToString("x2"));

            return "SAPISIDHASH " + unixTimestamp + "_" + sb.ToString();
        }
    }
}
