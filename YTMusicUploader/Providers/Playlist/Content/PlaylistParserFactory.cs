using System;
using YTMusicUploader.Providers.Playlist.Models;

namespace YTMusicUploader.Providers.Playlist.Content
{
    /// <summary>
    /// Music playlist library implementation
    /// </summary>
    /// <remarks>
    /// See https://github.com/tmk907/PlaylistsNET
    /// </remarks>
    public enum PlaylistType
    {
        M3U,
        M3U8,
        HLSMaster,
        HlsMedia,
        PLS,
        WPL,
        ZPL
    }

    public class PlaylistParserFactory
    {
        public static IPlaylistParser<IBasePlaylist> GetPlaylistParser(string fileType)
        {
            fileType = fileType.Trim('.');
            try
            {
                var type = (PlaylistType)Enum.Parse(typeof(PlaylistType), fileType, true);
                return GetPlaylistParser(type);
            }
            catch (ArgumentException)
            {
                throw new ArgumentException($"Unsupported playlist extension: {fileType}");
            }
        }

        public static IPlaylistParser<IBasePlaylist> GetPlaylistParser(PlaylistType playlistType)
        {
            IPlaylistParser<IBasePlaylist> playlistParser;

            switch (playlistType)
            {
                case PlaylistType.M3U:
                case PlaylistType.M3U8:
                    playlistParser = new M3uContent();
                    break;
                case PlaylistType.HLSMaster:
                    playlistParser = new HlsMasterContent();
                    break;
                case PlaylistType.HlsMedia:
                    playlistParser = new HlsMediaContent();
                    break;
                case PlaylistType.PLS:
                    playlistParser = new PlsContent();
                    break;
                case PlaylistType.WPL:
                    playlistParser = new WplContent();
                    break;
                case PlaylistType.ZPL:
                    playlistParser = new ZplContent();
                    break;
                default:
                    throw new ArgumentException($"Unsupported playlist type: {playlistType}");
            }
            return playlistParser;
        }
    }
}
