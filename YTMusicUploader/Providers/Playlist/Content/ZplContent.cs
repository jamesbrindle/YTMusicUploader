using System;
using System.IO;
using System.Text;
using System.Xml.Linq;
using YTMusicUploader.Providers.Playlist.Models;

namespace YTMusicUploader.Providers.Playlist.Content
{
    /// <summary>
    /// Music playlist library implementation
    /// </summary>
    /// <remarks>
    /// See https://github.com/tmk907/PlaylistsNET
    /// </remarks>
    public class ZplContent : IPlaylistParser<ZplPlaylist>, IPlaylistWriter<ZplPlaylist>
    {
        public string ToText(ZplPlaylist playlist)
        {
            var sb = new StringBuilder();
            var seq = CreateSeqWithMedia(playlist);
            var body = new XElement("body");
            body.Add(seq);
            var head = new XElement("head");
            if (!String.IsNullOrEmpty(playlist.Author))
            {
                var author = new XElement("author", playlist.Author);
                head.Add(author);
            }
            if (!String.IsNullOrEmpty(playlist.Guid))
            {
                var guid = new XElement("guid", playlist.Guid);
                head.Add(guid);
            }
            if (!String.IsNullOrEmpty(playlist.Generator))
            {
                head.Add(CreateMeta("Generator", playlist.Generator));
            }
            if (playlist.ItemCount > 0)
            {
                head.Add(CreateMeta("ItemCount", playlist.ItemCount.ToString()));
            }
            if (playlist.TotalDuration > TimeSpan.Zero)
            {
                head.Add(CreateMeta("totalDuration", ((int)playlist.TotalDuration.TotalMilliseconds).ToString()));
            }
            var title = new XElement("title", playlist.Title);
            head.Add(title);
            var smil = new XElement("smil");
            smil.Add(head);
            smil.Add(body);
            var doc = new XDocument();
            doc.Add(smil);
            sb.AppendLine(@"<?zpl version=""2.0""?>");
            sb.Append(doc.ToString());
            return sb.ToString();
        }

        public ZplPlaylist GetFromStream(Stream stream)
        {
            var streamReader = new StreamReader(stream);
            return GetFromString(streamReader.ReadToEnd());
        }

        public ZplPlaylist GetFromString(string playlistString)
        {
            var playlist = new ZplPlaylist();

            var doc = XDocument.Parse(playlistString);
            var mainDocument = doc.Element("smil");
            var head = mainDocument.Element("head");
            playlist.Author = (string)head.Element("author") ?? "";
            playlist.Guid = (string)head.Element("guid") ?? "";
            playlist.Title = (string)head.Element("title") ?? "";
            var metaElements = head.Elements("meta");
            foreach (var metaElement in metaElements)
            {
                string name = Utils.UnEscape(metaElement.Attribute("name")?.Value);
                string content = Utils.UnEscape(metaElement.Attribute("content")?.Value);
                switch (name)
                {
                    case "Generator":
                        playlist.Generator = content;
                        break;
                    case "ItemCount":
                        int count;
                        Int32.TryParse(content, out count);
                        playlist.ItemCount = count;
                        break;
                    case "totalDuration":
                        int miliseconds;
                        Int32.TryParse(content, out miliseconds);
                        playlist.TotalDuration = TimeSpan.FromMilliseconds(miliseconds);
                        break;
                    default:
                        break;
                }
            }
            var mediaElements = mainDocument.Elements("body").Elements("seq").Elements("media");
            foreach (var media in mediaElements)
            {
                string src = Utils.UnEscape(media.Attribute("src")?.Value);
                string trackTitle = Utils.UnEscape(media.Attribute("trackTitle")?.Value);
                string trackArtist = Utils.UnEscape(media.Attribute("trackArtist")?.Value);
                string albumTitle = Utils.UnEscape(media.Attribute("albumTitle")?.Value);
                string albumArtist = Utils.UnEscape(media.Attribute("albumArtist")?.Value);
                int.TryParse(Utils.UnEscape(media.Attribute("duration")?.Value), out int miliseconds);
                _ = TimeSpan.FromMilliseconds(miliseconds);
                playlist.PlaylistEntries.Add(new ZplPlaylistEntry()
                {
                    AlbumArtist = albumArtist,
                    AlbumTitle = albumTitle,
                    Path = src,
                    TrackArtist = trackArtist,
                    TrackTitle = trackTitle
                });
            }

            return playlist;
        }

        public string Update(ZplPlaylist playlist, Stream stream)
        {
            var doc = XDocument.Load(stream);
            var mainDocument = doc.Element("smil");
            var head = mainDocument.Element("head");
            var title = head.Element("title");
            title.ReplaceWith(new XElement("title", playlist.Title));
            if (!String.IsNullOrEmpty(playlist.Guid))
            {
                var guid = head.Element("guid");
                guid.ReplaceWith(new XElement("guid", playlist.Guid));
            }
            if (!String.IsNullOrEmpty(playlist.Author))
            {
                var author = head.Element("author");
                author.ReplaceWith(new XElement("author", playlist.Author));
            }
            var meta = head.Elements("meta");
            foreach (var metaElement in meta)
            {
                string name = Utils.UnEscape(metaElement.Attribute("name")?.Value);
                _ = Utils.UnEscape(metaElement.Attribute("content")?.Value);
                switch (name)
                {
                    case "Generator":
                        if (!String.IsNullOrEmpty(playlist.Generator))
                        {
                            metaElement.SetAttributeValue("content", playlist.Generator);
                        }
                        break;
                    case "ItemCount":
                        if (playlist.ItemCount > 0)
                        {
                            metaElement.SetAttributeValue("content", playlist.ItemCount);
                        }
                        break;
                    case "totalDuration":
                        if (playlist.TotalDuration > TimeSpan.Zero)
                        {
                            metaElement.SetAttributeValue("content", (int)playlist.TotalDuration.TotalMilliseconds);
                        }
                        break;
                    default:
                        break;
                }
            }
            var seq = mainDocument.Elements("body").Elements("seq");
            XElement seqWithMedia = null;
            foreach (var s in seq)
            {
                var m3 = s.Elements("media");
                int i = 0;
                foreach (var a in m3) { i++; }
                if (i > 0)
                {
                    seqWithMedia = s;
                    break;
                }
            }
            if (seqWithMedia != null)
            {
                var newSeq = CreateSeqWithMedia(playlist);
                seqWithMedia.ReplaceWith(newSeq);
            }

            return doc.ToString();
        }

        private XElement CreateSeqWithMedia(ZplPlaylist playlist)
        {
            var seq = new XElement("seq");
            foreach (var entry in playlist.PlaylistEntries)
            {
                var media = new XElement("media");
                var src = new XAttribute("src", entry.Path);
                media.Add(src);
                if (!String.IsNullOrEmpty(entry.AlbumArtist))
                {
                    var att = new XAttribute("albumTitle", entry.AlbumTitle);
                    media.Add(att);
                }
                if (!String.IsNullOrEmpty(entry.AlbumArtist))
                {
                    var att = new XAttribute("albumArtist", entry.AlbumArtist);
                    media.Add(att);
                }
                if (!String.IsNullOrEmpty(entry.TrackTitle))
                {
                    var att = new XAttribute("trackTitle", entry.TrackTitle);
                    media.Add(att);
                }
                if (!String.IsNullOrEmpty(entry.TrackArtist))
                {
                    var att = new XAttribute("trackArtist", entry.TrackArtist);
                    media.Add(att);
                }
                if (entry.Duration != null && entry.Duration != TimeSpan.Zero)
                {
                    var att = new XAttribute("duration", (int)entry.Duration.TotalMilliseconds);
                    media.Add(att);
                }
                seq.Add(media);
            }
            return seq;
        }

        private XElement CreateMeta(string name, string content)
        {
            var meta = new XElement("meta");
            var attName = new XAttribute("name", name);
            var attContent = new XAttribute("content", content);
            meta.Add(attName);
            meta.Add(attContent);
            return meta;
        }
    }
}
