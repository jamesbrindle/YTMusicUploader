namespace YTMusicUploader.Providers.RequestModels
{
    public class MusicManageTreeNodeModel
    {
        public enum NodeTypeEnum
        {
            Root,
            Artist,
            Album,
            Song
        }

        public NodeTypeEnum NodeType { get; set; }
        public string ArtistTitle { get; set; }
        public string AlbumTitle { get; set; }
        public string SongTitle { get; set; }
        public string Duration { get; set; }
        public string Uploaded { get; set; }
        public string DatabaseExistence { get; set; }
        public string EntityOrBrowseId { get; set; }
        public string MbId { get; set; }
        public string CovertArtUrl { get; set; }
    }
}
