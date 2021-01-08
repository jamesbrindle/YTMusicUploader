using System;
using System.IO;
using YTMusicUploader.Providers.DataModels;
using YTMusicUploader.Providers.Playlist.Content;
using YTMusicUploader.Providers.Playlist.Models;
using YTMusicUploader.Providers.Repos;

namespace YTMusicUploader.Providers.Playlist
{
    /// <summary>
    /// For reading music playlist files (.m3u, .m3u8, .wpl, .pls, .zpl)
    /// 
    /// Thanks to: tmk907: 
    ///     https://github.com/tmk907/PlaylistsNET
    /// </summary>
    public class PlaylistReader
    {
        /// <summary>
        /// Read a playlist file from a give path and returns a PlaylistFile (Data Model) complete with absolute paths
        /// well as reading the meta 
        /// </summary>
        /// <param name="path">Path to playlist file</param>
        /// <returns>PlaylistFile (Data Model)</returns>
        public static PlaylistFile ReadPlaylistFile(string path)
        {
            var playlistFile = new PlaylistFile();
            string playlistExtension = Path.GetExtension(path).ToLower();

            var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using (var sr = new StreamReader(fs))
            {
                switch (playlistExtension)
                {
                    case ".wpl":
                        var content_wpl = new WplContent();
                        var playlist_wpl = content_wpl.GetFromStream(sr.BaseStream);
                        playlistFile.Title = playlist_wpl.Title.Trim();
                        break;
                    case ".zpl":
                        var content_zpl = new ZplContent();
                        var playlist_zpl = content_zpl.GetFromStream(sr.BaseStream);
                        playlistFile.Title = playlist_zpl.Title.Trim();
                        break;
                    default:
                        playlistFile.Title = Path.GetFileNameWithoutExtension(path).Trim();
                        break;
                }

                if (string.IsNullOrEmpty(playlistFile.Title))
                    playlistFile.Title = Path.GetFileNameWithoutExtension(path).Trim();

                sr.BaseStream.Position = 0;
                var parser = PlaylistParserFactory.GetPlaylistParser(playlistExtension);
                var playlist = parser.GetFromStream(sr.BaseStream);
                var paths = playlist.GetTracksPaths();

                if ((paths == null || paths.Count == 0) && playlistExtension.In(".m3u", ".m3u8"))
                    paths = M3uPlaylist.GetPlaylistFromCorruptM3u(path);

                playlistFile.Path = path;
                playlistFile.LastModifiedDate = new FileInfo(path).LastWriteTime;

                var musicFileRepo = new MusicFileRepo();

                foreach (string musicFilePath in paths)
                {
                    string absolutePath = Utils.IsAbsolutePath(musicFilePath)
                                                ? musicFilePath
                                                : Utils.MakeAbsolutePath(Path.GetDirectoryName(path), musicFilePath);

                    if (absolutePath.StartsWith("file://"))
                        absolutePath = new Uri(absolutePath).LocalPath;

                    if (Path.GetExtension(absolutePath).ToLower().In(Global.SupportedMusicFiles) &&
                        File.Exists(absolutePath))
                    {
                        playlistFile.PlaylistItems.Add(new PlaylistFile.PlaylistFileItem
                        {
                            Path = absolutePath
                        });
                    }
                }
            }

            return playlistFile;
        }
    }
}
