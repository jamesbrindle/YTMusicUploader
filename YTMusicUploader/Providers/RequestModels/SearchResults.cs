namespace YTMusicUploader.Providers.RequestModels
{
    /// <summary>
    /// Deserialised HttpWebRequest response body received from YouTube Music after a search
    /// </summary>
    public class SearchResult
    {
        public Responsecontext responseContext { get; set; }
        public string trackingParams { get; set; }
        public Contents contents { get; set; }

        public class Responsecontext
        {
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

        public class Contents
        {
            public Tabbedsearchresultsrenderer tabbedSearchResultsRenderer { get; set; }
        }

        public class Tabbedsearchresultsrenderer
        {
            public Tab[] tabs { get; set; }
        }

        public class Tab
        {
            public Tabrenderer tabRenderer { get; set; }
        }

        public class Tabrenderer
        {
            public Endpoint endpoint { get; set; }
            public string title { get; set; }
            public bool selected { get; set; }
            public string trackingParams { get; set; }
            public Content content { get; set; }
        }

        public class Endpoint
        {
            public string clickTrackingParams { get; set; }
            public Searchendpoint searchEndpoint { get; set; }
        }

        public class Searchendpoint
        {
            public string query { get; set; }
            public string _params { get; set; }
        }

        public class Content
        {
            public Sectionlistrenderer sectionListRenderer { get; set; }
        }

        public class Sectionlistrenderer
        {
            public Content1[] contents { get; set; }
            public string trackingParams { get; set; }
        }

        public class Content1
        {
            public Musicshelfrenderer musicShelfRenderer { get; set; }
        }

        public class Musicshelfrenderer
        {
            public Content2[] contents { get; set; }
            public string trackingParams { get; set; }
            public Shelfdivider shelfDivider { get; set; }
        }

        public class Shelfdivider
        {
            public Musicshelfdividerrenderer musicShelfDividerRenderer { get; set; }
        }

        public class Musicshelfdividerrenderer
        {
            public bool hidden { get; set; }
        }

        public class Content2
        {
            public Musicresponsivelistitemrenderer musicResponsiveListItemRenderer { get; set; }
        }

        public class Musicresponsivelistitemrenderer
        {
            public string trackingParams { get; set; }
            public Thumbnail thumbnail { get; set; }
            public Overlay overlay { get; set; }
            public Flexcolumn[] flexColumns { get; set; }
            public Menu menu { get; set; }
            public string flexColumnDisplayStyle { get; set; }
            public Navigationendpoint1 navigationEndpoint { get; set; }
            public string itemHeight { get; set; }
            public Fixedcolumn[] fixedColumns { get; set; }
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

        public class Overlay
        {
            public Musicitemthumbnailoverlayrenderer musicItemThumbnailOverlayRenderer { get; set; }
        }

        public class Musicitemthumbnailoverlayrenderer
        {
            public Background background { get; set; }
            public Content3 content { get; set; }
            public string contentPosition { get; set; }
            public string displayStyle { get; set; }
        }

        public class Background
        {
            public Verticalgradient verticalGradient { get; set; }
        }

        public class Verticalgradient
        {
            public string[] gradientLayerColors { get; set; }
        }

        public class Content3
        {
            public Musicplaybuttonrenderer musicPlayButtonRenderer { get; set; }
        }

        public class Musicplaybuttonrenderer
        {
            public Playnavigationendpoint playNavigationEndpoint { get; set; }
            public string trackingParams { get; set; }
            public Playicon playIcon { get; set; }
            public Pauseicon pauseIcon { get; set; }
            public long iconColor { get; set; }
            public int backgroundColor { get; set; }
            public int activeBackgroundColor { get; set; }
            public long loadingIndicatorColor { get; set; }
            public Playingicon playingIcon { get; set; }
            public int iconLoadingColor { get; set; }
            public int activeScaleFactor { get; set; }
            public string buttonSize { get; set; }
            public string rippleTarget { get; set; }
            public Accessibilityplaydata accessibilityPlayData { get; set; }
            public Accessibilitypausedata accessibilityPauseData { get; set; }
        }

        public class Playnavigationendpoint
        {
            public string clickTrackingParams { get; set; }
            public Watchplaylistendpoint watchPlaylistEndpoint { get; set; }
            public Watchendpoint watchEndpoint { get; set; }
        }

        public class Watchplaylistendpoint
        {
            public string playlistId { get; set; }
        }

        public class Watchendpoint
        {
            public string videoId { get; set; }
            public string playlistId { get; set; }
            public string _params { get; set; }
            public Watchendpointmusicsupportedconfigs watchEndpointMusicSupportedConfigs { get; set; }
        }

        public class Watchendpointmusicsupportedconfigs
        {
            public Watchendpointmusicconfig watchEndpointMusicConfig { get; set; }
        }

        public class Watchendpointmusicconfig
        {
            public string musicVideoType { get; set; }
        }

        public class Playicon
        {
            public string iconType { get; set; }
        }

        public class Pauseicon
        {
            public string iconType { get; set; }
        }

        public class Playingicon
        {
            public string iconType { get; set; }
        }

        public class Accessibilityplaydata
        {
            public Accessibilitydata accessibilityData { get; set; }
        }

        public class Accessibilitydata
        {
            public string label { get; set; }
        }

        public class Accessibilitypausedata
        {
            public Accessibilitydata1 accessibilityData { get; set; }
        }

        public class Accessibilitydata1
        {
            public string label { get; set; }
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
            public Toplevelbutton[] topLevelButtons { get; set; }
        }

        public class Accessibility
        {
            public Accessibilitydata2 accessibilityData { get; set; }
        }

        public class Accessibilitydata2
        {
            public string label { get; set; }
        }

        public class Item
        {
            public Menunavigationitemrenderer menuNavigationItemRenderer { get; set; }
            public Menuserviceitemrenderer menuServiceItemRenderer { get; set; }
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
            public Watchplaylistendpoint1 watchPlaylistEndpoint { get; set; }
            public Addtoplaylistendpoint addToPlaylistEndpoint { get; set; }
            public Browseendpoint browseEndpoint { get; set; }
            public Confirmdialogendpoint confirmDialogEndpoint { get; set; }
        }

        public class Watchplaylistendpoint1
        {
            public string playlistId { get; set; }
            public string _params { get; set; }
        }

        public class Addtoplaylistendpoint
        {
            public string playlistId { get; set; }
            public string videoId { get; set; }
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

        public class Confirmdialogendpoint
        {
            public Content4 content { get; set; }
        }

        public class Content4
        {
            public Confirmdialogrenderer confirmDialogRenderer { get; set; }
        }

        public class Confirmdialogrenderer
        {
            public Title title { get; set; }
            public string trackingParams { get; set; }
            public Dialogmessage[] dialogMessages { get; set; }
            public Confirmbutton confirmButton { get; set; }
            public Cancelbutton cancelButton { get; set; }
        }

        public class Title
        {
            public Run1[] runs { get; set; }
        }

        public class Run1
        {
            public string text { get; set; }
        }

        public class Confirmbutton
        {
            public Buttonrenderer buttonRenderer { get; set; }
        }

        public class Buttonrenderer
        {
            public string style { get; set; }
            public string size { get; set; }
            public bool isDisabled { get; set; }
            public Text1 text { get; set; }
            public string trackingParams { get; set; }
            public Command command { get; set; }
        }

        public class Text1
        {
            public Run2[] runs { get; set; }
        }

        public class Run2
        {
            public string text { get; set; }
        }

        public class Command
        {
            public string clickTrackingParams { get; set; }
            public Musicdeleteprivatelyownedentitycommand musicDeletePrivatelyOwnedEntityCommand { get; set; }
        }

        public class Musicdeleteprivatelyownedentitycommand
        {
            public string entityId { get; set; }
        }

        public class Cancelbutton
        {
            public Buttonrenderer1 buttonRenderer { get; set; }
        }

        public class Buttonrenderer1
        {
            public string style { get; set; }
            public string size { get; set; }
            public bool isDisabled { get; set; }
            public Text2 text { get; set; }
            public string trackingParams { get; set; }
        }

        public class Text2
        {
            public Run3[] runs { get; set; }
        }

        public class Run3
        {
            public string text { get; set; }
        }

        public class Dialogmessage
        {
            public Run4[] runs { get; set; }
        }

        public class Run4
        {
            public string text { get; set; }
        }

        public class Menuserviceitemrenderer
        {
            public Text3 text { get; set; }
            public Icon1 icon { get; set; }
            public Serviceendpoint serviceEndpoint { get; set; }
            public string trackingParams { get; set; }
        }

        public class Text3
        {
            public Run5[] runs { get; set; }
        }

        public class Run5
        {
            public string text { get; set; }
        }

        public class Icon1
        {
            public string iconType { get; set; }
        }

        public class Serviceendpoint
        {
            public string clickTrackingParams { get; set; }
            public Queueaddendpoint queueAddEndpoint { get; set; }
        }

        public class Queueaddendpoint
        {
            public Queuetarget queueTarget { get; set; }
            public string queueInsertPosition { get; set; }
            public Command1[] commands { get; set; }
        }

        public class Queuetarget
        {
            public string playlistId { get; set; }
            public string videoId { get; set; }
        }

        public class Command1
        {
            public string clickTrackingParams { get; set; }
            public Addtotoastaction addToToastAction { get; set; }
        }

        public class Addtotoastaction
        {
            public Item1 item { get; set; }
        }

        public class Item1
        {
            public Notificationtextrenderer notificationTextRenderer { get; set; }
        }

        public class Notificationtextrenderer
        {
            public Successresponsetext successResponseText { get; set; }
            public string trackingParams { get; set; }
        }

        public class Successresponsetext
        {
            public Run6[] runs { get; set; }
        }

        public class Run6
        {
            public string text { get; set; }
        }

        public class Toplevelbutton
        {
            public Likebuttonrenderer likeButtonRenderer { get; set; }
        }

        public class Likebuttonrenderer
        {
            public Target target { get; set; }
            public string likeStatus { get; set; }
            public string trackingParams { get; set; }
            public bool likesAllowed { get; set; }
            public Serviceendpoint1[] serviceEndpoints { get; set; }
        }

        public class Target
        {
            public string videoId { get; set; }
        }

        public class Serviceendpoint1
        {
            public string clickTrackingParams { get; set; }
            public Likeendpoint likeEndpoint { get; set; }
        }

        public class Likeendpoint
        {
            public string status { get; set; }
            public Target1 target { get; set; }
        }

        public class Target1
        {
            public string videoId { get; set; }
        }

        public class Navigationendpoint1
        {
            public string clickTrackingParams { get; set; }
            public Browseendpoint1 browseEndpoint { get; set; }
        }

        public class Browseendpoint1
        {
            public string browseId { get; set; }
            public Browseendpointcontextsupportedconfigs1 browseEndpointContextSupportedConfigs { get; set; }
        }

        public class Browseendpointcontextsupportedconfigs1
        {
            public Browseendpointcontextmusicconfig1 browseEndpointContextMusicConfig { get; set; }
        }

        public class Browseendpointcontextmusicconfig1
        {
            public string pageType { get; set; }
        }

        public class Flexcolumn
        {
            public Musicresponsivelistitemflexcolumnrenderer musicResponsiveListItemFlexColumnRenderer { get; set; }
        }

        public class Musicresponsivelistitemflexcolumnrenderer
        {
            public Text4 text { get; set; }
            public string displayPriority { get; set; }
        }

        public class Text4
        {
            public Run7[] runs { get; set; }
        }

        public class Run7
        {
            public string text { get; set; }
            public Navigationendpoint2 navigationEndpoint { get; set; }
        }

        public class Navigationendpoint2
        {
            public string clickTrackingParams { get; set; }
            public Browseendpoint2 browseEndpoint { get; set; }
            public Watchendpoint1 watchEndpoint { get; set; }
        }

        public class Browseendpoint2
        {
            public string browseId { get; set; }
            public Browseendpointcontextsupportedconfigs2 browseEndpointContextSupportedConfigs { get; set; }
        }

        public class Browseendpointcontextsupportedconfigs2
        {
            public Browseendpointcontextmusicconfig2 browseEndpointContextMusicConfig { get; set; }
        }

        public class Browseendpointcontextmusicconfig2
        {
            public string pageType { get; set; }
        }

        public class Watchendpoint1
        {
            public string videoId { get; set; }
            public string playlistId { get; set; }
            public Watchendpointmusicsupportedconfigs1 watchEndpointMusicSupportedConfigs { get; set; }
        }

        public class Watchendpointmusicsupportedconfigs1
        {
            public Watchendpointmusicconfig1 watchEndpointMusicConfig { get; set; }
        }

        public class Watchendpointmusicconfig1
        {
            public string musicVideoType { get; set; }
        }

        public class Fixedcolumn
        {
            public Musicresponsivelistitemfixedcolumnrenderer musicResponsiveListItemFixedColumnRenderer { get; set; }
        }

        public class Musicresponsivelistitemfixedcolumnrenderer
        {
            public Text5 text { get; set; }
            public string displayPriority { get; set; }
            public string size { get; set; }
        }

        public class Text5
        {
            public Run8[] runs { get; set; }
        }

        public class Run8
        {
            public string text { get; set; }
        }
    }
}
