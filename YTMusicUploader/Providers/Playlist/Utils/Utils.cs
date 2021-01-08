using System;
using System.IO;

namespace YTMusicUploader.Providers.Playlist
{
    /// <summary>
    /// Music playlist library implementation (static utilities class)
    /// </summary>
    /// <remarks>
    /// See https://github.com/tmk907/PlaylistsNET
    /// </remarks>
    public class Utils
    {
        public static string MakeAbsolutePath(string folderPath, string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath)) return filePath;

            if (IsStream(filePath)) return filePath;
            if (IsAbsolutePath(filePath)) return filePath;

            if (filePath.StartsWith("\\\\"))
                return filePath;

            if (filePath[0] == '/' || filePath[0] == '\\') //relative path and starts with / or \
            {
                filePath = filePath.Substring(1);
            }
            try
            {
                if (IsStream(folderPath))
                {
                    if (!folderPath.EndsWith("/"))
                    {
                        folderPath += "/";
                    }
                    string path = Path.Combine(folderPath, filePath);
                    return path;
                }
                else
                {

                    string path = Path.Combine(folderPath, filePath);
                    path = Path.GetFullPath(path);
                    return path;
                }
            }
            catch (ArgumentException)
            {
                return filePath;
            }
            catch (PathTooLongException)
            {
                return filePath;
            }
            catch (NotSupportedException)
            {
                return filePath;
            }
        }

        public static string MakeRelativePath(string folderPath, string fileAbsolutePath)
        {
            if (string.IsNullOrEmpty(folderPath)) throw new ArgumentNullException("folderPath");
            if (string.IsNullOrEmpty(fileAbsolutePath)) throw new ArgumentNullException("filePath");

            if (!folderPath.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                folderPath += Path.DirectorySeparatorChar;
            }

            var folderUri = new Uri(folderPath);
            var fileAbsoluteUri = new Uri(fileAbsolutePath);

            if (folderUri.Scheme != fileAbsoluteUri.Scheme) { return fileAbsolutePath; } // path can't be made relative.

            var relativeUri = folderUri.MakeRelativeUri(fileAbsoluteUri);
            string relativePath = Uri.UnescapeDataString(relativeUri.ToString());

            if (fileAbsoluteUri.Scheme.Equals("file", StringComparison.CurrentCultureIgnoreCase))
            {
                relativePath = relativePath.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
            }

            return relativePath;
        }

        public static bool IsAbsolutePath(string path)
        {
            if (path.Length > 3)
            {
                if (path[1] == ':' && (path[2] == '\\' || path[2] == '/')) return true;
            }
            return false;
        }

        public static bool IsRelativePath(string path)
        {
            if (path.StartsWith(@"/") ||
                path.StartsWith(@"./") ||
                path.StartsWith(@"../") ||
                path.StartsWith(@"\") ||
                path.StartsWith(@".\") ||
                path.StartsWith(@"..\")) return true;
            return false;
        }

        public static bool IsStream(string path)
        {
            return path.Contains(@"://");
        }

        public static string UnEscape(string content)
        {
            if (content == null) return content;
            return content.Replace("&amp;", "&").Replace("&apos;", "'").Replace("&quot;", "\"").Replace("&gt;", ">").Replace("&lt;", "<");
        }

        public static string Escape(string content)
        {
            if (content == null) return null;
            return content.Replace("&", "&amp;").Replace("'", "&apos;").Replace("\"", "&quot;").Replace(">", "&gt;").Replace("<", "&lt;");
        }
    }
}
