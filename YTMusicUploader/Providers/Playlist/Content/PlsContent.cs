using System;
using System.IO;
using System.Linq;
using System.Text;
using YTMusicUploader.Providers.Playlist.Models;

namespace YTMusicUploader.Providers.Playlist.Content
{
    /// <summary>
    /// Music playlist library implementation
    /// </summary>
    /// <remarks>
    /// See https://github.com/tmk907/PlaylistsNET
    /// </remarks>
    public class PlsContent : IPlaylistParser<PlsPlaylist>, IPlaylistWriter<PlsPlaylist>
    {
        public string ToText(PlsPlaylist playlist)
        {
            var sb = new StringBuilder();
            int nr = 0;

            sb.AppendLine("[playlist]");
            sb.AppendLine();
            foreach (var entry in playlist.PlaylistEntries)
            {
                nr++;
                sb.AppendLine(ToFile(entry.Path, nr));
                if (!String.IsNullOrEmpty(entry.Title))
                {
                    sb.AppendLine(ToTitle(entry.Title, nr));
                }
                if (entry.Length != TimeSpan.Zero)
                {
                    sb.AppendLine(ToLength(entry.Length, nr));
                }
                sb.AppendLine();
            }
            sb.Append("NumberOfEntries=").Append(nr).AppendLine();
            sb.AppendLine();
            sb.Append("Version=2");

            return sb.ToString();
        }

        public PlsPlaylist GetFromStream(Stream stream)
        {
            var streamReader = new StreamReader(stream);
            return GetFromString(streamReader.ReadToEnd());
        }

        public PlsPlaylist GetFromString(string playlistString)
        {
            var playlist = new PlsPlaylist
            {
                Version = 2
            };

            var playlistLines = playlistString.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToList();

            // If there are no lines to parse, return the empty playlist
            if (playlistLines.Count == 0)
            {
                return playlist;
            }

            // Verify header is [playlist] and return if not
            if (playlistLines[0] != "[playlist]")
            {
                return playlist;
            }

            foreach (string line in playlistLines)
            {
                int nr = GetNr(line);
                if (line.StartsWith("File"))
                {
                    string path = GetPath(line);
                    var entry = playlist.PlaylistEntries.SingleOrDefault(e => e.Nr == nr);
                    if (entry == null)
                    {
                        playlist.PlaylistEntries.Add(new PlsPlaylistEntry()
                        {
                            Nr = nr,
                            Path = path
                        });
                    }
                    else
                    {
                        entry.Path = path;
                    }
                }
                else if (line.StartsWith("Title"))
                {
                    string title = GetTitle(line);
                    if (!String.IsNullOrEmpty(title))
                    {
                        var entry = playlist.PlaylistEntries.SingleOrDefault(e => e.Nr == nr);
                        if (entry == null)
                        {
                            playlist.PlaylistEntries.Add(new PlsPlaylistEntry()
                            {
                                Nr = nr,
                                Title = title
                            });
                        }
                        else
                        {
                            entry.Title = title;
                        }
                    }
                }
                else if (line.StartsWith("Length"))
                {
                    var length = GetLength(line);
                    if (length != null)
                    {
                        var entry = playlist.PlaylistEntries.SingleOrDefault(e => e.Nr == nr);
                        if (entry == null)
                        {
                            playlist.PlaylistEntries.Add(new PlsPlaylistEntry()
                            {
                                Nr = nr,
                                Length = length
                            });
                        }
                        else
                        {
                            entry.Length = length;
                        }
                    }
                }
            }
            playlist.PlaylistEntries = playlist.PlaylistEntries.OrderBy(e => e.Nr).ToList();
            return playlist;
        }

        private string ToFile(string path, int nr)
        {
            var sb = new StringBuilder();
            sb.Append("File").Append(nr).Append("=").Append(path);
            return sb.ToString();
        }

        private string ToTitle(string title, int nr)
        {
            var sb = new StringBuilder();
            sb.Append("Title").Append(nr).Append("=").Append(title);
            return sb.ToString();
        }

        private string ToLength(TimeSpan length, int nr)
        {
            var sb = new StringBuilder();
            sb.Append("Length").Append(nr).Append("=").Append((int)length.TotalSeconds);
            return sb.ToString();
        }

        private int GetNr(string line)
        {
            int nr = -1;
            if (line.StartsWith("File"))
            {
                try
                {
                    //0123456
                    //File1=
                    //File10=
                    nr = Int32.Parse(line.Substring(4, line.IndexOf('=') - 4));
                }
                catch { }
            }
            else if (line.StartsWith("Title"))
            {
                try
                {
                    //01234567
                    //Title1=
                    //Title10=
                    nr = Int32.Parse(line.Substring(5, line.IndexOf('=') - 5));
                }
                catch { }
            }
            else if (line.StartsWith("Length"))
            {
                try
                {
                    //012345678
                    //Length1=
                    //Length10=
                    nr = Int32.Parse(line.Substring(6, line.IndexOf('=') - 6));
                }
                catch { }
            }
            return nr;
        }

        private string GetPath(string line)
        {
            string path = null;
            try
            {
                path = line.Substring(line.IndexOf('=') + 1);
            }
            catch { }
            return path;
        }

        private string GetTitle(string line)
        {
            string title = null;
            try
            {
                title = line.Substring(line.IndexOf('=') + 1);
            }
            catch { }
            return title;
        }

        private TimeSpan GetLength(string line)
        {
            var length = TimeSpan.Zero;
            try
            {
                length = TimeSpan.FromSeconds(Int32.Parse(line.Substring(line.IndexOf('=') + 1)));
            }
            catch { }
            return length;
        }
    }
}
