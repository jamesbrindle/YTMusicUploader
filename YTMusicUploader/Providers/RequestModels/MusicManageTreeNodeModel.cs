namespace YTMusicUploader.Providers.RequestModels
{
    /// <summary>
    /// Model for Manage YouTube Music dialogue's TreeView
    /// </summary>
    public class MusicManageTreeNodeModel
    {
        public enum NodeTypeEnum
        {
            Root,
            Artist,
            Album,
            Song,
            Playlist,
            PlaylistItem
        }

        public NodeTypeEnum NodeType { get; set; }
        public string ArtistTitle { get; set; }
        public string AlbumTitle { get; set; }
        public string SongTitleOrDescription { get; set; }
        public string PlaylistTitle { get; set; }
        public string Duration { get; set; }
        public string Uploaded { get; set; }
        public string DatabaseExistence { get; set; }
        public string EntityOrBrowseId { get; set; }
        public string AltEntityId { get; set; }
        public string MbId { get; set; }
        public string CovertArtUrl { get; set; }
    }
}
