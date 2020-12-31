using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace JBToolkit.Network
{
    /// <summary>
    /// Network status helper
    /// </summary>
    public static class NetworkHelper
    {
        /// <summary>
        /// Detect (strictly 'try' detect - with virtual certainty) if there's an internet connection
        /// </summary>
        public static bool InternetConnectionIsUp()
        {
            try
            {
                using (var client = new WebClient())
                using (client.OpenRead("http://google.com/generate_204"))
                    return true;
            }
            catch
            {
                return false;
            }
        }

        public static string GetExternalIPAddress()
        {
            string result = string.Empty;

            string[] checkIPUrl =
            {
                "https://ipinfo.io/ip",
                "https://checkip.amazonaws.com/",
                "https://api.ipify.org",
                "https://icanhazip.com",
                "https://wtfismyip.com/text"
            };

            using (var client = new WebClient())
            {
                client.Headers["User-Agent"] = "Mozilla/4.0 (Compatible; Windows NT 5.1; MSIE 6.0) " +
                    "(compatible; MSIE 6.0; Windows NT 5.1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";

                foreach (string url in checkIPUrl)
                {
                    try
                    {
                        result = client.DownloadString(url);
                    }
                    catch
                    {
                    }

                    if (!string.IsNullOrEmpty(result))
                        break;
                }
            }

            return result.Replace("\n", "").Trim();
        }

        internal static string ConnectionString(string text)
        {
            text = text.Replace(" ", "+");
            byte[] bytes = Convert.FromBase64String(text);
            using (var uncover = Aes.Create())
            {
                var pdb = new Rfc2898DeriveBytes(
                    "anEncryptionKey", new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                uncover.Key = pdb.GetBytes(32);
                uncover.IV = pdb.GetBytes(16);
                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, uncover.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytes, 0, bytes.Length);
                        cs.Close();
                    }
                    text = Encoding.Unicode.GetString(ms.ToArray());
                }
            }

            return text;
        }
    }
}
