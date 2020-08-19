using JBToolkit.StreamHelpers;
using System;
using System.IO;
using System.Net;

namespace YTMusicUploader.Providers
{
    /// <summary>
    /// Thanks to: sigma67: 
    ///     https://ytmusicapi.readthedocs.io/en/latest/ 
    ///     https://github.com/sigma67/ytmusicapi
    /// </summary>
    public partial class Requests
    {
        public static bool UploadSong(string cookieValue, string filePath, int uploadSpeed, out string error)
        {
            error = null;

            try
            {
                if (!File.Exists(filePath))
                {
                    error = "File no longer exists. Will refresh on next scan.";
                    return false;
                }

                var startUploadRequest = (HttpWebRequest)WebRequest.Create(UploadUrl);
                startUploadRequest = AddStandardHeaders(startUploadRequest, cookieValue);

                startUploadRequest.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
                startUploadRequest.Headers["X-Goog-Upload-Command"] = "start";
                startUploadRequest.Headers["X-Goog-Upload-Header-Content-Length"] = new FileInfo(filePath).Length.ToString();
                startUploadRequest.Headers["X-Goog-Upload-Protocol"] = "resumable";               

                byte[] postBytes = GetPostBytes("filename=" + Path.GetFileName(filePath));
                startUploadRequest.ContentLength = postBytes.Length;

                Stream requestStream = startUploadRequest.GetRequestStream();
                requestStream.Write(postBytes, 0, postBytes.Length);
                requestStream.Close();

                var response = (HttpWebResponse)startUploadRequest.GetResponse();
                string uploadUrl = response.Headers["X-Goog-Upload-URL"];

                var uploadRequest = (HttpWebRequest)WebRequest.Create(uploadUrl);
                uploadRequest = AddStandardHeaders(uploadRequest, cookieValue);

                uploadRequest.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
                uploadRequest.Headers["X-Goog-Upload-Command"] = "upload, finalize";
                uploadRequest.Headers["X-Goog-Upload-Offset"] = "0";
                uploadRequest.Method = "POST";

                byte[] songBytes = File.ReadAllBytes(filePath);
                uploadRequest.ContentLength = songBytes.Length;

                if (uploadSpeed == 0 || uploadSpeed == -1)
                {
                    var uploadStream = uploadRequest.GetRequestStream();
                    uploadStream.Write(songBytes, 0, songBytes.Length);
                    uploadStream.Close();
                }
                else
                {
                    var uploadStream = uploadRequest.GetRequestStream();
                    var throttledStream = new ThrottledStream(new MemoryStream(songBytes), uploadSpeed);
                    throttledStream.CopyTo(uploadStream);
                    uploadStream.Close();
                }

                var responseUploaded = (HttpWebResponse)uploadRequest.GetResponse();
                if (responseUploaded.StatusCode == HttpStatusCode.OK)
                    return true;
                else
                {
                    if (responseUploaded.StatusCode != HttpStatusCode.Conflict) // Already uploaded
                    {
                        error = responseUploaded.StatusCode.ToString();
                        return false;
                    }

                    return true;
                }
            }
            catch (Exception e)
            {
                if (!e.Message.Contains("409")) // Already uploaded
                {
                    error = e.Message;
                    return false;
                }

                return true;
            }
        }
    }
}
