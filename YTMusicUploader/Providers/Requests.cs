using System;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace YTMusicUploader.Providers
{
    public partial class Requests
    {
        public static string YouTubeBaseUrl
        {
            get
            {
                return "https://music.youtube.com/youtubei/v1/";
            }
        }

        public static string UploadUrl
        {
            get
            {
                return "https://upload.youtube.com/upload/usermusic/http?authuser=0";
            }
        }

        public static string Params
        {
            get
            {
                return "?alt=json&key=AIzaSyC9XL3ZjWddXya6X74dJoCTL-WEYFDNX30";
            }
        }

        public static HttpWebRequest AddStandardHeaders(HttpWebRequest webRequest, string cookieValue)
        {
            webRequest.Accept = "*/*";
            webRequest.Headers["Accept-Encoding"] = "gzip, deflate, br";
            webRequest.UserAgent = "Mozilla / 5.0(Windows NT 10.0; Win64; x64; rv: 72.0) Gecko / 20100101 Firefox / 72.0";            
            webRequest.Headers["Cookie"] = cookieValue;
            webRequest.Method = "POST";

            return webRequest;
        }

        public static byte[] GetPostBytes(string stringToEncode)
        {
            byte[] bytes = Encoding.Default.GetBytes(stringToEncode);
            string body = Encoding.UTF8.GetString(bytes);
            return Encoding.UTF8.GetBytes(body);
        }

        private static string GetSAPISIDFromCookie(string cookieValue)
        {
            string[] parts = Regex.Split(cookieValue, "SAPISID=");
            string[] parts2 = parts[1].Split(';');
            return parts2[0];
        }

        private static string GetAuthorisation(string auth)
        {
            int unixTimestamp = (int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            var hash = new SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(unixTimestamp + " " + auth));
            var sb = new StringBuilder(hash.Length * 2);

            foreach (byte b in hash)
                sb.Append(b.ToString("x2"));

            return "SAPISIDHASH " + unixTimestamp + "_" + sb.ToString();
        }
    }
}
