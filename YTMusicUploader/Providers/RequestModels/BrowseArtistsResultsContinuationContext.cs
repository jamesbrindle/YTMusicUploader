namespace YTMusicUploader.Providers.RequestModels
{
    /// <summary>
    /// Deserialised HttpWebRequest response body received from YouTube Music after a search
    /// </summary>
    public class BrowseArtistsResultsContinuationContext
    {
        public Responsecontext responseContext { get; set; }
        public string trackingParams { get; set; }
        public Continuationcontents continuationContents { get; set; }

        public class Responsecontext
        {
            public string visitorData { get; set; }
            public int maxAgeSeconds { get; set; }
            public Servicetrackingparam[] serviceTrackingParams { get; set; }
        }

        public class Servicetrackingparam
        {
            public string service { get; set; }
            public Param[] _params { get; set; }
        }

        public class Param
        {
            public string key { get; set; }
            public string value { get; set; }
        }

        public class Continuationcontents
        {
            public Musicshelfcontinuation musicShelfContinuation { get; set; }
        }

        public class Musicshelfcontinuation
        {
            public Content[] contents { get; set; }
            public string trackingParams { get; set; }
            public Continuation[] continuations { get; set; }
            public Shelfdivider shelfDivider { get; set; }
            public bool autoReloadWhenEmpty { get; set; }
        }

        public class Shelfdivider
        {
            public Musicshelfdividerrenderer musicShelfDividerRenderer { get; set; }
        }

        public class Musicshelfdividerrenderer
        {
            public bool hidden { get; set; }
        }

        public class Content
        {
            public Musicresponsivelistitemrenderer musicResponsiveListItemRenderer { get; set; }
        }

        public class Musicresponsivelistitemrenderer
        {
            public string trackingParams { get; set; }
            public Thumbnail thumbnail { get; set; }
            public Flexcolumn[] flexColumns { get; set; }
            public Menu menu { get; set; }
            public string flexColumnDisplayStyle { get; set; }
            public Navigationendpoint1 navigationEndpoint { get; set; }
            public string itemHeight { get; set; }
        }

        public class Thumbnail
        {
            public Musicthumbnailrenderer musicThumbnailRenderer { get; set; }
        }

        public class Musicthumbnailrenderer
        {
            public Thumbnail1 thumbnail { get; set; }
            public string thumbnailCrop { get; set; }
            public string thumbnailScale { get; set; }
            public string trackingParams { get; set; }
        }

        public class Thumbnail1
        {
            public Thumbnail2[] thumbnails { get; set; }
        }

        public class Thumbnail2
        {
            public string url { get; set; }
            public int width { get; set; }
            public int height { get; set; }
        }

        public class Menu
        {
            public Menurenderer menuRenderer { get; set; }
        }

        public class Menurenderer
        {
            public Item[] items { get; set; }
            public string trackingParams { get; set; }
            public Accessibility accessibility { get; set; }
        }

        public class Accessibility
        {
            public Accessibilitydata accessibilityData { get; set; }
        }

        public class Accessibilitydata
        {
            public string label { get; set; }
        }

        public class Item
        {
            public Menunavigationitemrenderer menuNavigationItemRenderer { get; set; }
        }

        public class Menunavigationitemrenderer
        {
            public Text text { get; set; }
            public Icon icon { get; set; }
            public Navigationendpoint navigationEndpoint { get; set; }
            public string trackingParams { get; set; }
        }

        public class Text
        {
            public Run[] runs { get; set; }
        }

        public class Run
        {
            public string text { get; set; }
        }

        public class Icon
        {
            public string iconType { get; set; }
        }

        public class Navigationendpoint
        {
            public string clickTrackingParams { get; set; }
            public Watchplaylistendpoint watchPlaylistEndpoint { get; set; }
        }

        public class Watchplaylistendpoint
        {
            public string playlistId { get; set; }
            public string _params { get; set; }
        }

        public class Navigationendpoint1
        {
            public string clickTrackingParams { get; set; }
            public Browseendpoint browseEndpoint { get; set; }
        }

        public class Browseendpoint
        {
            public string browseId { get; set; }
            public Browseendpointcontextsupportedconfigs browseEndpointContextSupportedConfigs { get; set; }
        }

        public class Browseendpointcontextsupportedconfigs
        {
            public Browseendpointcontextmusicconfig browseEndpointContextMusicConfig { get; set; }
        }

        public class Browseendpointcontextmusicconfig
        {
            public string pageType { get; set; }
        }

        public class Flexcolumn
        {
            public Musicresponsivelistitemflexcolumnrenderer musicResponsiveListItemFlexColumnRenderer { get; set; }
        }

        public class Musicresponsivelistitemflexcolumnrenderer
        {
            public Text1 text { get; set; }
            public string displayPriority { get; set; }
        }

        public class Text1
        {
            public Run1[] runs { get; set; }
        }

        public class Run1
        {
            public string text { get; set; }
        }

        public class Continuation
        {
            public Nextcontinuationdata nextContinuationData { get; set; }
        }

        public class Nextcontinuationdata
        {
            public string continuation { get; set; }
            public string clickTrackingParams { get; set; }
        }
    }
}