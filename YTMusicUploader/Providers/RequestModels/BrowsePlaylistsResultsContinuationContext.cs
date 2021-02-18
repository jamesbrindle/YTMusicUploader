
namespace YTMusicUploader.Providers.RequestModels
{
    public class BrowsePlaylistsResultsContinuationContext
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
            public Gridcontinuation gridContinuation { get; set; }
        }

        public class Gridcontinuation
        {
            public Item[] items { get; set; }
            public Continuation[] continuations { get; set; }
            public string trackingParams { get; set; }
            public string itemSize { get; set; }
        }

        public class Item
        {
            public Musictworowitemrenderer musicTwoRowItemRenderer { get; set; }
        }

        public class Musictworowitemrenderer
        {
            public Thumbnailrenderer thumbnailRenderer { get; set; }
            public string aspectRatio { get; set; }
            public Title title { get; set; }
            public Subtitle subtitle { get; set; }
            public Navigationendpoint2 navigationEndpoint { get; set; }
            public string trackingParams { get; set; }
            public Menu menu { get; set; }
            public Thumbnailoverlay thumbnailOverlay { get; set; }
        }

        public class Thumbnailrenderer
        {
            public Musicthumbnailrenderer musicThumbnailRenderer { get; set; }
        }

        public class Musicthumbnailrenderer
        {
            public Thumbnail thumbnail { get; set; }
            public string thumbnailCrop { get; set; }
            public string thumbnailScale { get; set; }
            public string trackingParams { get; set; }
        }

        public class Thumbnail
        {
            public Thumbnail1[] thumbnails { get; set; }
        }

        public class Thumbnail1
        {
            public string url { get; set; }
            public int width { get; set; }
            public int height { get; set; }
        }

        public class Title
        {
            public Run[] runs { get; set; }
        }

        public class Run
        {
            public string text { get; set; }
            public Navigationendpoint navigationEndpoint { get; set; }
        }

        public class Navigationendpoint
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

        public class Subtitle
        {
            public Run1[] runs { get; set; }
        }

        public class Run1
        {
            public string text { get; set; }
            public Navigationendpoint1 navigationEndpoint { get; set; }
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

        public class Navigationendpoint2
        {
            public string clickTrackingParams { get; set; }
            public Browseendpoint2 browseEndpoint { get; set; }
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

        public class Menu
        {
            public Menurenderer menuRenderer { get; set; }
        }

        public class Menurenderer
        {
            public Item1[] items { get; set; }
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

        public class Item1
        {
            public Menunavigationitemrenderer menuNavigationItemRenderer { get; set; }
            public Menuserviceitemrenderer menuServiceItemRenderer { get; set; }
        }

        public class Menunavigationitemrenderer
        {
            public Text text { get; set; }
            public Icon icon { get; set; }
            public Navigationendpoint3 navigationEndpoint { get; set; }
            public string trackingParams { get; set; }
        }

        public class Text
        {
            public Run2[] runs { get; set; }
        }

        public class Run2
        {
            public string text { get; set; }
        }

        public class Icon
        {
            public string iconType { get; set; }
        }

        public class Navigationendpoint3
        {
            public string clickTrackingParams { get; set; }
            public Watchplaylistendpoint watchPlaylistEndpoint { get; set; }
            public Playlisteditorendpoint playlistEditorEndpoint { get; set; }
            public Addtoplaylistendpoint addToPlaylistEndpoint { get; set; }
            public Shareentityendpoint shareEntityEndpoint { get; set; }
            public Confirmdialogendpoint confirmDialogEndpoint { get; set; }
        }

        public class Watchplaylistendpoint
        {
            public string playlistId { get; set; }
            public string _params { get; set; }
        }

        public class Playlisteditorendpoint
        {
            public string playlistId { get; set; }
        }

        public class Addtoplaylistendpoint
        {
            public string playlistId { get; set; }
        }

        public class Shareentityendpoint
        {
            public string serializedShareEntity { get; set; }
        }

        public class Confirmdialogendpoint
        {
            public Content content { get; set; }
        }

        public class Content
        {
            public Confirmdialogrenderer confirmDialogRenderer { get; set; }
        }

        public class Confirmdialogrenderer
        {
            public Title1 title { get; set; }
            public string trackingParams { get; set; }
            public Dialogmessage[] dialogMessages { get; set; }
            public Confirmbutton confirmButton { get; set; }
            public Cancelbutton cancelButton { get; set; }
        }

        public class Title1
        {
            public Run3[] runs { get; set; }
        }

        public class Run3
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
            public Serviceendpoint serviceEndpoint { get; set; }
            public string trackingParams { get; set; }
        }

        public class Text1
        {
            public Run4[] runs { get; set; }
        }

        public class Run4
        {
            public string text { get; set; }
        }

        public class Serviceendpoint
        {
            public string clickTrackingParams { get; set; }
            public Deleteplaylistendpoint deletePlaylistEndpoint { get; set; }
        }

        public class Deleteplaylistendpoint
        {
            public string playlistId { get; set; }
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
            public Run5[] runs { get; set; }
        }

        public class Run5
        {
            public string text { get; set; }
        }

        public class Dialogmessage
        {
            public Run6[] runs { get; set; }
        }

        public class Run6
        {
            public string text { get; set; }
        }

        public class Menuserviceitemrenderer
        {
            public Text3 text { get; set; }
            public Icon1 icon { get; set; }
            public Serviceendpoint1 serviceEndpoint { get; set; }
            public string trackingParams { get; set; }
        }

        public class Text3
        {
            public Run7[] runs { get; set; }
        }

        public class Run7
        {
            public string text { get; set; }
        }

        public class Icon1
        {
            public string iconType { get; set; }
        }

        public class Serviceendpoint1
        {
            public string clickTrackingParams { get; set; }
            public Queueaddendpoint queueAddEndpoint { get; set; }
        }

        public class Queueaddendpoint
        {
            public Queuetarget queueTarget { get; set; }
            public string queueInsertPosition { get; set; }
            public Command[] commands { get; set; }
        }

        public class Queuetarget
        {
            public string playlistId { get; set; }
        }

        public class Command
        {
            public string clickTrackingParams { get; set; }
            public Addtotoastaction addToToastAction { get; set; }
        }

        public class Addtotoastaction
        {
            public Item2 item { get; set; }
        }

        public class Item2
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
            public Run8[] runs { get; set; }
        }

        public class Run8
        {
            public string text { get; set; }
        }

        public class Thumbnailoverlay
        {
            public Musicitemthumbnailoverlayrenderer musicItemThumbnailOverlayRenderer { get; set; }
        }

        public class Musicitemthumbnailoverlayrenderer
        {
            public Background background { get; set; }
            public Content1 content { get; set; }
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

        public class Content1
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
            public long backgroundColor { get; set; }
            public long activeBackgroundColor { get; set; }
            public long loadingIndicatorColor { get; set; }
            public Playingicon playingIcon { get; set; }
            public int iconLoadingColor { get; set; }
            public float activeScaleFactor { get; set; }
            public string buttonSize { get; set; }
            public string rippleTarget { get; set; }
            public Accessibilityplaydata accessibilityPlayData { get; set; }
            public Accessibilitypausedata accessibilityPauseData { get; set; }
        }

        public class Playnavigationendpoint
        {
            public string clickTrackingParams { get; set; }
            public Watchplaylistendpoint1 watchPlaylistEndpoint { get; set; }
        }

        public class Watchplaylistendpoint1
        {
            public string playlistId { get; set; }
            public string _params { get; set; }
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
            public Accessibilitydata1 accessibilityData { get; set; }
        }

        public class Accessibilitydata1
        {
            public string label { get; set; }
        }

        public class Accessibilitypausedata
        {
            public Accessibilitydata2 accessibilityData { get; set; }
        }

        public class Accessibilitydata2
        {
            public string label { get; set; }
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