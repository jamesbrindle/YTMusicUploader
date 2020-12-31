using YTMusicUploader.Providers.Playlist.Models;

namespace YTMusicUploader.Providers.Playlist.Content
{
    /// <summary>
    /// Music playlist library implementation
    /// </summary>
    /// <remarks>
    /// See https://github.com/tmk907/PlaylistsNET
    /// </remarks>
    public class PlaylistToTextHelper
    {
        public static string ToText(IBasePlaylist playlist)
        {
            string text = "";

            switch (playlist)
            {
                case M3uPlaylist m3u:
                    var m3uWriter = new M3uContent();
                    text = m3uWriter.ToText(m3u);
                    break;
                case PlsPlaylist pls:
                    var plsWriter = new PlsContent();
                    text = plsWriter.ToText(pls);
                    break;
                case WplPlaylist wpl:
                    var wplWriter = new WplContent();
                    text = wplWriter.ToText(wpl);
                    break;
                case ZplPlaylist zpl:
                    var zplWriter = new ZplContent();
                    text = zplWriter.ToText(zpl);
                    break;
                default:
                    break;
            }

            return text;
        }
    }
}
