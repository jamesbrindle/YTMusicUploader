using System;
using System.Collections.Generic;

namespace YTMusicUploader.Providers.Playlist.Models
{
    /// <summary>
    /// Music playlist library implementation
    /// </summary>
    /// <remarks>
    /// See https://github.com/tmk907/PlaylistsNET
    /// </remarks>
    public class M3uPlaylist : BasePlaylist<M3uPlaylistEntry>
    {
        public M3uPlaylist()
        {
            Comments = new List<string>();
        }

        public bool IsExtended { get; set; }
        public List<string> Comments { get; set; }

        /// <summary>
        /// This handles playlists created in a particular version of VLC and saved as .mp3u. For some odd
        /// reason VLC isn't saving them correctly. In fact, you can't even open that playlist in VLC!
        /// 
        /// None-the-less, the playlist does include the file paths, therefore we can parse them.
        /// </summary>
        /// <param name="playlistPath">Full path to playlist file</param>
        /// <returns>Music file paths as a string list</returns>
        public static List<string> GetPlaylistFromCorruptM3u(string playlistPath)
        {
            var paths = new List<string>();

            try
            {
                string[] lines = JBToolkit.StreamHelpers.SafeFileStream.ReadAllLines(playlistPath);

                foreach (var line in lines)
                {
                    try
                    {
                        if (line.Contains("file://"))
                        {
                            string path = line.Substring(line.IndexOf("file://"), line.Length - line.IndexOf("file://"))
                                              .Replace("\r", "")
                                              .Replace("\n", "").Trim();

                            path = new Uri(path).LocalPath;
                            paths.Add(path);
                        }
                    }
                    catch { }
                }
            }
            catch { }

            return paths;
        }
    }
}
