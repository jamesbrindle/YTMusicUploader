
namespace YTMusicUploader.Providers.RequestModels
{
    /// <summary>
    /// Deserialised HttpWebRequest response body received from YouTube Music after a search
    /// </summary>
    public class BrowsePlaylistsResultsContext : IRequestModel
    {
        public Responsecontext responseContext { get; set; }
        public string trackingParams { get; set; }
        public Contents contents { get; set; }
    }

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

    public class Contents
    {
        public Singlecolumnbrowseresultsrenderer singleColumnBrowseResultsRenderer { get; set; }
    }

    public class Singlecolumnbrowseresultsrenderer
    {
        public Tab[] tabs { get; set; }
    }

    public class Tab
    {
        public Tabrenderer tabRenderer { get; set; }
    }

    public class Tabrenderer
    {
        public bool selected { get; set; }
        public Content content { get; set; }
        public string tabIdentifier { get; set; }
        public string trackingParams { get; set; }
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
        public Musiccarouselshelfrenderer musicCarouselShelfRenderer { get; set; }
        public Itemsectionrenderer itemSectionRenderer { get; set; }
    }

    public class Musiccarouselshelfrenderer
    {
        public Header header { get; set; }
        public Content2[] contents { get; set; }
        public string trackingParams { get; set; }
        public Backgroundoverlay backgroundOverlay { get; set; }
        public string itemSize { get; set; }
    }

    public class Header
    {
        public Musiccarouselshelfbasicheaderrenderer musicCarouselShelfBasicHeaderRenderer { get; set; }
    }

    public class Musiccarouselshelfbasicheaderrenderer
    {
        public Title title { get; set; }
        public Accessibilitydata accessibilityData { get; set; }
        public Endicon[] endIcons { get; set; }
        public string headerStyle { get; set; }
        public string trackingParams { get; set; }
    }

    public class Title
    {
        public Run[] runs { get; set; }
    }

    public class Run
    {
        public string text { get; set; }
    }

    public class Accessibilitydata
    {
        public Accessibilitydata1 accessibilityData { get; set; }
    }

    public class Accessibilitydata1
    {
        public string label { get; set; }
    }

    public class Endicon
    {
        public Iconlinkrenderer iconLinkRenderer { get; set; }
    }

    public class Iconlinkrenderer
    {
        public Icon icon { get; set; }
        public Tooltip tooltip { get; set; }
        public Navigationendpoint navigationEndpoint { get; set; }
        public string trackingParams { get; set; }
    }

    public class Icon
    {
        public string iconType { get; set; }
    }

    public class Tooltip
    {
        public Run1[] runs { get; set; }
    }

    public class Run1
    {
        public string text { get; set; }
    }

    public class Navigationendpoint
    {
        public string clickTrackingParams { get; set; }
        public Browseendpoint browseEndpoint { get; set; }
    }

    public class Browseendpoint
    {
        public string browseId { get; set; }
    }

    public class Backgroundoverlay
    {
        public Verticalgradient verticalGradient { get; set; }
    }

    public class Verticalgradient
    {
        public string[] gradientLayerColors { get; set; }
    }

    public class Content2
    {
        public Musictworowitemrenderer musicTwoRowItemRenderer { get; set; }
    }

    public class Musictworowitemrenderer
    {
        public Thumbnailrenderer thumbnailRenderer { get; set; }
        public string aspectRatio { get; set; }
        public Title1 title { get; set; }
        public Subtitle subtitle { get; set; }
        public Navigationendpoint3 navigationEndpoint { get; set; }
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

    public class Title1
    {
        public Run2[] runs { get; set; }
    }

    public class Run2
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
        public Run3[] runs { get; set; }
    }

    public class Run3
    {
        public string text { get; set; }
        public Navigationendpoint2 navigationEndpoint { get; set; }
    }

    public class Navigationendpoint2
    {
        public string clickTrackingParams { get; set; }
        public Browseendpoint2 browseEndpoint { get; set; }
    }

    public class Browseendpoint2
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

    public class Navigationendpoint3
    {
        public string clickTrackingParams { get; set; }
        public Browseendpoint3 browseEndpoint { get; set; }
        public Watchendpoint watchEndpoint { get; set; }
    }

    public class Browseendpoint3
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
        public Togglemenuserviceitemrenderer toggleMenuServiceItemRenderer { get; set; }
    }

    public class Menunavigationitemrenderer
    {
        public Text text { get; set; }
        public Icon1 icon { get; set; }
        public Navigationendpoint4 navigationEndpoint { get; set; }
        public string trackingParams { get; set; }
    }

    public class Text
    {
        public Run4[] runs { get; set; }
    }

    public class Run4
    {
        public string text { get; set; }
    }

    public class Icon1
    {
        public string iconType { get; set; }
    }

    public class Navigationendpoint4
    {
        public string clickTrackingParams { get; set; }
        public Watchplaylistendpoint watchPlaylistEndpoint { get; set; }
        public Addtoplaylistendpoint addToPlaylistEndpoint { get; set; }
        public Shareentityendpoint shareEntityEndpoint { get; set; }
        public Watchendpoint1 watchEndpoint { get; set; }
        public Browseendpoint4 browseEndpoint { get; set; }
    }

    public class Watchplaylistendpoint
    {
        public string playlistId { get; set; }
        public string _params { get; set; }
    }

    public class Addtoplaylistendpoint
    {
        public string playlistId { get; set; }
        public string videoId { get; set; }
    }

    public class Shareentityendpoint
    {
        public string serializedShareEntity { get; set; }
    }

    public class Watchendpoint1
    {
        public string videoId { get; set; }
        public string playlistId { get; set; }
        public string _params { get; set; }
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

    public class Browseendpoint4
    {
        public string browseId { get; set; }
        public Browseendpointcontextsupportedconfigs3 browseEndpointContextSupportedConfigs { get; set; }
    }

    public class Browseendpointcontextsupportedconfigs3
    {
        public Browseendpointcontextmusicconfig3 browseEndpointContextMusicConfig { get; set; }
    }

    public class Browseendpointcontextmusicconfig3
    {
        public string pageType { get; set; }
    }

    public class Menuserviceitemrenderer
    {
        public Text1 text { get; set; }
        public Icon2 icon { get; set; }
        public Serviceendpoint serviceEndpoint { get; set; }
        public string trackingParams { get; set; }
    }

    public class Text1
    {
        public Run5[] runs { get; set; }
    }

    public class Run5
    {
        public string text { get; set; }
    }

    public class Icon2
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
        public Command[] commands { get; set; }
    }

    public class Queuetarget
    {
        public string playlistId { get; set; }
        public string videoId { get; set; }
    }

    public class Command
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

    public class Togglemenuserviceitemrenderer
    {
        public Defaulttext defaultText { get; set; }
        public Defaulticon defaultIcon { get; set; }
        public Defaultserviceendpoint defaultServiceEndpoint { get; set; }
        public Toggledtext toggledText { get; set; }
        public Toggledicon toggledIcon { get; set; }
        public Toggledserviceendpoint toggledServiceEndpoint { get; set; }
        public string trackingParams { get; set; }
    }

    public class Defaulttext
    {
        public Run7[] runs { get; set; }
    }

    public class Run7
    {
        public string text { get; set; }
    }

    public class Defaulticon
    {
        public string iconType { get; set; }
    }

    public class Defaultserviceendpoint
    {
        public string clickTrackingParams { get; set; }
        public Likeendpoint likeEndpoint { get; set; }
        public Feedbackendpoint feedbackEndpoint { get; set; }
    }

    public class Likeendpoint
    {
        public string status { get; set; }
        public Target target { get; set; }
    }

    public class Target
    {
        public string playlistId { get; set; }
        public string videoId { get; set; }
    }

    public class Feedbackendpoint
    {
        public string feedbackToken { get; set; }
    }

    public class Toggledtext
    {
        public Run8[] runs { get; set; }
    }

    public class Run8
    {
        public string text { get; set; }
    }

    public class Toggledicon
    {
        public string iconType { get; set; }
    }

    public class Toggledserviceendpoint
    {
        public string clickTrackingParams { get; set; }
        public Likeendpoint1 likeEndpoint { get; set; }
        public Feedbackendpoint1 feedbackEndpoint { get; set; }
    }

    public class Likeendpoint1
    {
        public string status { get; set; }
        public Target1 target { get; set; }
        public Action[] actions { get; set; }
    }

    public class Target1
    {
        public string playlistId { get; set; }
        public string videoId { get; set; }
    }

    public class Action
    {
        public string clickTrackingParams { get; set; }
        public Musiclibrarystatusupdatecommand musicLibraryStatusUpdateCommand { get; set; }
    }

    public class Musiclibrarystatusupdatecommand
    {
        public string libraryStatus { get; set; }
        public string addToLibraryFeedbackToken { get; set; }
    }

    public class Feedbackendpoint1
    {
        public string feedbackToken { get; set; }
    }

    public class Thumbnailoverlay
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
        public Verticalgradient1 verticalGradient { get; set; }
    }

    public class Verticalgradient1
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
        public Watchendpoint2 watchEndpoint { get; set; }
    }

    public class Watchplaylistendpoint1
    {
        public string playlistId { get; set; }
        public string _params { get; set; }
    }

    public class Watchendpoint2
    {
        public string videoId { get; set; }
        public string playlistId { get; set; }
        public string _params { get; set; }
        public Watchendpointmusicsupportedconfigs2 watchEndpointMusicSupportedConfigs { get; set; }
    }

    public class Watchendpointmusicsupportedconfigs2
    {
        public Watchendpointmusicconfig2 watchEndpointMusicConfig { get; set; }
    }

    public class Watchendpointmusicconfig2
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
        public Accessibilitydata3 accessibilityData { get; set; }
    }

    public class Accessibilitydata3
    {
        public string label { get; set; }
    }

    public class Accessibilitypausedata
    {
        public Accessibilitydata4 accessibilityData { get; set; }
    }

    public class Accessibilitydata4
    {
        public string label { get; set; }
    }

    public class Itemsectionrenderer
    {
        public Content4[] contents { get; set; }
        public string trackingParams { get; set; }
        public Header1 header { get; set; }
    }

    public class Header1
    {
        public Itemsectiontabbedheaderrenderer itemSectionTabbedHeaderRenderer { get; set; }
    }

    public class Itemsectiontabbedheaderrenderer
    {
        public Tab1[] tabs { get; set; }
        public string trackingParams { get; set; }
        public Enditem[] endItems { get; set; }
    }

    public class Tab1
    {
        public Itemsectiontabrenderer itemSectionTabRenderer { get; set; }
    }

    public class Itemsectiontabrenderer
    {
        public Title2 title { get; set; }
        public Endpoint endpoint { get; set; }
        public bool selected { get; set; }
        public string trackingParams { get; set; }
        public string targetId { get; set; }
    }

    public class Title2
    {
        public Run9[] runs { get; set; }
    }

    public class Run9
    {
        public string text { get; set; }
    }

    public class Endpoint
    {
        public string clickTrackingParams { get; set; }
        public Browseendpoint5 browseEndpoint { get; set; }
    }

    public class Browseendpoint5
    {
        public string browseId { get; set; }
    }

    public class Enditem
    {
        public Dropdownrenderer dropdownRenderer { get; set; }
    }

    public class Dropdownrenderer
    {
        public Entry[] entries { get; set; }
    }

    public class Entry
    {
        public Dropdownitemrenderer dropdownItemRenderer { get; set; }
    }

    public class Dropdownitemrenderer
    {
        public Label label { get; set; }
        public bool isSelected { get; set; }
        public Onselectcommand onSelectCommand { get; set; }
    }

    public class Label
    {
        public Run10[] runs { get; set; }
    }

    public class Run10
    {
        public string text { get; set; }
    }

    public class Onselectcommand
    {
        public string clickTrackingParams { get; set; }
        public Browseendpoint6 browseEndpoint { get; set; }
    }

    public class Browseendpoint6
    {
        public string browseId { get; set; }
        public string _params { get; set; }
    }

    public class Content4
    {
        public Gridrenderer gridRenderer { get; set; }
    }

    public class Gridrenderer
    {
        public Item2[] items { get; set; }
        public string trackingParams { get; set; }
        public string itemSize { get; set; }
    }

    public class Item2
    {
        public Musictworowitemrenderer1 musicTwoRowItemRenderer { get; set; }
    }

    public class Musictworowitemrenderer1
    {
        public Thumbnailrenderer1 thumbnailRenderer { get; set; }
        public string aspectRatio { get; set; }
        public Title3 title { get; set; }
        public Navigationendpoint6 navigationEndpoint { get; set; }
        public string trackingParams { get; set; }
        public Subtitle1 subtitle { get; set; }
        public Menu1 menu { get; set; }
        public Thumbnailoverlay1 thumbnailOverlay { get; set; }
    }

    public class Thumbnailrenderer1
    {
        public Musicthumbnailrenderer1 musicThumbnailRenderer { get; set; }
    }

    public class Musicthumbnailrenderer1
    {
        public Thumbnail2 thumbnail { get; set; }
        public string thumbnailCrop { get; set; }
        public string thumbnailScale { get; set; }
        public string trackingParams { get; set; }
    }

    public class Thumbnail2
    {
        public Thumbnail3[] thumbnails { get; set; }
    }

    public class Thumbnail3
    {
        public string url { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }

    public class Title3
    {
        public Run11[] runs { get; set; }
    }

    public class Run11
    {
        public string text { get; set; }
        public Navigationendpoint5 navigationEndpoint { get; set; }
    }

    public class Navigationendpoint5
    {
        public string clickTrackingParams { get; set; }
        public Createplaylistendpoint createPlaylistEndpoint { get; set; }
        public Browseendpoint7 browseEndpoint { get; set; }
    }

    public class Createplaylistendpoint
    {
        public bool hack { get; set; }
    }

    public class Browseendpoint7
    {
        public string browseId { get; set; }
        public Browseendpointcontextsupportedconfigs4 browseEndpointContextSupportedConfigs { get; set; }
    }

    public class Browseendpointcontextsupportedconfigs4
    {
        public Browseendpointcontextmusicconfig4 browseEndpointContextMusicConfig { get; set; }
    }

    public class Browseendpointcontextmusicconfig4
    {
        public string pageType { get; set; }
    }

    public class Navigationendpoint6
    {
        public string clickTrackingParams { get; set; }
        public Createplaylistendpoint1 createPlaylistEndpoint { get; set; }
        public Browseendpoint8 browseEndpoint { get; set; }
    }

    public class Createplaylistendpoint1
    {
        public bool hack { get; set; }
    }

    public class Browseendpoint8
    {
        public string browseId { get; set; }
        public Browseendpointcontextsupportedconfigs5 browseEndpointContextSupportedConfigs { get; set; }
    }

    public class Browseendpointcontextsupportedconfigs5
    {
        public Browseendpointcontextmusicconfig5 browseEndpointContextMusicConfig { get; set; }
    }

    public class Browseendpointcontextmusicconfig5
    {
        public string pageType { get; set; }
    }

    public class Subtitle1
    {
        public Run12[] runs { get; set; }
    }

    public class Run12
    {
        public string text { get; set; }
        public Navigationendpoint7 navigationEndpoint { get; set; }
    }

    public class Navigationendpoint7
    {
        public string clickTrackingParams { get; set; }
        public Browseendpoint9 browseEndpoint { get; set; }
    }

    public class Browseendpoint9
    {
        public string browseId { get; set; }
        public Browseendpointcontextsupportedconfigs6 browseEndpointContextSupportedConfigs { get; set; }
    }

    public class Browseendpointcontextsupportedconfigs6
    {
        public Browseendpointcontextmusicconfig6 browseEndpointContextMusicConfig { get; set; }
    }

    public class Browseendpointcontextmusicconfig6
    {
        public string pageType { get; set; }
    }

    public class Menu1
    {
        public Menurenderer1 menuRenderer { get; set; }
    }

    public class Menurenderer1
    {
        public Item3[] items { get; set; }
        public string trackingParams { get; set; }
        public Accessibility1 accessibility { get; set; }
    }

    public class Accessibility1
    {
        public Accessibilitydata5 accessibilityData { get; set; }
    }

    public class Accessibilitydata5
    {
        public string label { get; set; }
    }

    public class Item3
    {
        public Menunavigationitemrenderer1 menuNavigationItemRenderer { get; set; }
        public Menuserviceitemrenderer1 menuServiceItemRenderer { get; set; }
    }

    public class Menunavigationitemrenderer1
    {
        public Text2 text { get; set; }
        public Icon3 icon { get; set; }
        public Navigationendpoint8 navigationEndpoint { get; set; }
        public string trackingParams { get; set; }
    }

    public class Text2
    {
        public Run13[] runs { get; set; }
    }

    public class Run13
    {
        public string text { get; set; }
    }

    public class Icon3
    {
        public string iconType { get; set; }
    }

    public class Navigationendpoint8
    {
        public string clickTrackingParams { get; set; }
        public Watchplaylistendpoint2 watchPlaylistEndpoint { get; set; }
        public Playlisteditorendpoint playlistEditorEndpoint { get; set; }
        public Addtoplaylistendpoint1 addToPlaylistEndpoint { get; set; }
        public Shareentityendpoint1 shareEntityEndpoint { get; set; }
        public Confirmdialogendpoint confirmDialogEndpoint { get; set; }
    }

    public class Watchplaylistendpoint2
    {
        public string playlistId { get; set; }
        public string _params { get; set; }
    }

    public class Playlisteditorendpoint
    {
        public string playlistId { get; set; }
    }

    public class Addtoplaylistendpoint1
    {
        public string playlistId { get; set; }
    }

    public class Shareentityendpoint1
    {
        public string serializedShareEntity { get; set; }
    }

    public class Confirmdialogendpoint
    {
        public Content5 content { get; set; }
    }

    public class Content5
    {
        public Confirmdialogrenderer confirmDialogRenderer { get; set; }
    }

    public class Confirmdialogrenderer
    {
        public Title4 title { get; set; }
        public string trackingParams { get; set; }
        public Dialogmessage[] dialogMessages { get; set; }
        public Confirmbutton confirmButton { get; set; }
        public Cancelbutton cancelButton { get; set; }
    }

    public class Title4
    {
        public Run14[] runs { get; set; }
    }

    public class Run14
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
        public Text3 text { get; set; }
        public Serviceendpoint1 serviceEndpoint { get; set; }
        public string trackingParams { get; set; }
    }

    public class Text3
    {
        public Run15[] runs { get; set; }
    }

    public class Run15
    {
        public string text { get; set; }
    }

    public class Serviceendpoint1
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
        public Text4 text { get; set; }
        public string trackingParams { get; set; }
    }

    public class Text4
    {
        public Run16[] runs { get; set; }
    }

    public class Run16
    {
        public string text { get; set; }
    }

    public class Dialogmessage
    {
        public Run17[] runs { get; set; }
    }

    public class Run17
    {
        public string text { get; set; }
    }

    public class Menuserviceitemrenderer1
    {
        public Text5 text { get; set; }
        public Icon4 icon { get; set; }
        public Serviceendpoint2 serviceEndpoint { get; set; }
        public string trackingParams { get; set; }
    }

    public class Text5
    {
        public Run18[] runs { get; set; }
    }

    public class Run18
    {
        public string text { get; set; }
    }

    public class Icon4
    {
        public string iconType { get; set; }
    }

    public class Serviceendpoint2
    {
        public string clickTrackingParams { get; set; }
        public Queueaddendpoint1 queueAddEndpoint { get; set; }
    }

    public class Queueaddendpoint1
    {
        public Queuetarget1 queueTarget { get; set; }
        public string queueInsertPosition { get; set; }
        public Command1[] commands { get; set; }
    }

    public class Queuetarget1
    {
        public string playlistId { get; set; }
    }

    public class Command1
    {
        public string clickTrackingParams { get; set; }
        public Addtotoastaction1 addToToastAction { get; set; }
    }

    public class Addtotoastaction1
    {
        public Item4 item { get; set; }
    }

    public class Item4
    {
        public Notificationtextrenderer1 notificationTextRenderer { get; set; }
    }

    public class Notificationtextrenderer1
    {
        public Successresponsetext1 successResponseText { get; set; }
        public string trackingParams { get; set; }
    }

    public class Successresponsetext1
    {
        public Run19[] runs { get; set; }
    }

    public class Run19
    {
        public string text { get; set; }
    }

    public class Thumbnailoverlay1
    {
        public Musicitemthumbnailoverlayrenderer1 musicItemThumbnailOverlayRenderer { get; set; }
    }

    public class Musicitemthumbnailoverlayrenderer1
    {
        public Background1 background { get; set; }
        public Content6 content { get; set; }
        public string contentPosition { get; set; }
        public string displayStyle { get; set; }
    }

    public class Background1
    {
        public Verticalgradient2 verticalGradient { get; set; }
    }

    public class Verticalgradient2
    {
        public string[] gradientLayerColors { get; set; }
    }

    public class Content6
    {
        public Musicplaybuttonrenderer1 musicPlayButtonRenderer { get; set; }
    }

    public class Musicplaybuttonrenderer1
    {
        public Playnavigationendpoint1 playNavigationEndpoint { get; set; }
        public string trackingParams { get; set; }
        public Playicon1 playIcon { get; set; }
        public Pauseicon1 pauseIcon { get; set; }
        public long iconColor { get; set; }
        public long backgroundColor { get; set; }
        public long activeBackgroundColor { get; set; }
        public long loadingIndicatorColor { get; set; }
        public Playingicon1 playingIcon { get; set; }
        public int iconLoadingColor { get; set; }
        public float activeScaleFactor { get; set; }
        public string buttonSize { get; set; }
        public string rippleTarget { get; set; }
        public Accessibilityplaydata1 accessibilityPlayData { get; set; }
        public Accessibilitypausedata1 accessibilityPauseData { get; set; }
    }

    public class Playnavigationendpoint1
    {
        public string clickTrackingParams { get; set; }
        public Watchplaylistendpoint3 watchPlaylistEndpoint { get; set; }
    }

    public class Watchplaylistendpoint3
    {
        public string playlistId { get; set; }
        public string _params { get; set; }
    }

    public class Playicon1
    {
        public string iconType { get; set; }
    }

    public class Pauseicon1
    {
        public string iconType { get; set; }
    }

    public class Playingicon1
    {
        public string iconType { get; set; }
    }

    public class Accessibilityplaydata1
    {
        public Accessibilitydata6 accessibilityData { get; set; }
    }

    public class Accessibilitydata6
    {
        public string label { get; set; }
    }

    public class Accessibilitypausedata1
    {
        public Accessibilitydata7 accessibilityData { get; set; }
    }

    public class Accessibilitydata7
    {
        public string label { get; set; }
    }
}
