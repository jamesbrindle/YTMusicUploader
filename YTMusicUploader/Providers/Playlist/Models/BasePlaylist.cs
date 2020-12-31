using System.Collections.Generic;

namespace YTMusicUploader.Providers.Playlist.Models
{
    /// <summary>
    /// Music playlist library implementation
    /// </summary>
    /// <remarks>
    /// See https://github.com/tmk907/PlaylistsNET
    /// </remarks>
    public interface IBasePlaylist
    {
        string Path { get; set; }
        string FileName { get; set; }
        List<string> GetTracksPaths();
    }

    /// <summary>
    /// Music playlist library implementation
    /// </summary>
    /// <remarks>
    /// See https://github.com/tmk907/PlaylistsNET
    /// </remarks>
    public class BasePlaylist<T> : IBasePlaylist where T : BasePlaylistEntry
    {
        public List<T> PlaylistEntries { get; set; }
        public string Path { get; set; }
        public string FileName { get; set; }

        public List<string> GetTracksPaths()
        {
            var paths = new List<string>();
            foreach (var track in PlaylistEntries)
            {
                paths.Add(track.Path);
            }
            return paths;
        }

        public BasePlaylist()
        {
            PlaylistEntries = new List<T>();
        }
    }
}