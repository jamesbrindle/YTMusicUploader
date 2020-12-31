using System.IO;
using YTMusicUploader.Providers.Playlist.Models;

namespace YTMusicUploader.Providers.Playlist.Content
{
    /// <summary>
    /// Music playlist library implementation
    /// </summary>
    /// <remarks>
    /// See https://github.com/tmk907/PlaylistsNET
    /// </remarks>
    public interface IPlaylistParser<out T> where T : IBasePlaylist
    {
        T GetFromStream(Stream stream);
        T GetFromString(string playlistString);
    }
}
