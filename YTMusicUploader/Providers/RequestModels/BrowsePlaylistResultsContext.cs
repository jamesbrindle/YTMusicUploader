namespace YTMusicUploader.Providers.RequestModels
{
    public class BrowsePlaylistResultsContext : IRequestModel
    {
        public Responsecontext responseContext { get; set; }
        public string trackingParams { get; set; }
        public Contents contents { get; set; }
        public Header header { get; set; }


        public class Responsecontext
        {
            public string visitorData { get; set; }
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
            public Content content { get; set; }
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
            public Musicplaylistshelfrenderer musicPlaylistShelfRenderer { get; set; }
        }

        public class Musicplaylistshelfrenderer
        {
            public string playlistId { get; set; }
            public Content2[] contents { get; set; }
            public int collapsedItemCount { get; set; }
            public string trackingParams { get; set; }
            public bool contentsReorderable { get; set; }
            public Continuation[] continuations { get; set; }
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
            public Fixedcolumn[] fixedColumns { get; set; }
            public Menu menu { get; set; }
            public Playlistitemdata playlistItemData { get; set; }
            public Badge[] badges { get; set; }
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
            public Watchendpoint watchEndpoint { get; set; }
        }

        public class Watchendpoint
        {
            public string videoId { get; set; }
            public string playlistId { get; set; }
            public string playlistSetVideoId { get; set; }
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
            public Toplevelbutton[] topLevelButtons { get; set; }
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
            public Watchendpoint1 watchEndpoint { get; set; }
            public Addtoplaylistendpoint addToPlaylistEndpoint { get; set; }
            public Browseendpoint browseEndpoint { get; set; }
            public Shareentityendpoint shareEntityEndpoint { get; set; }
            public Confirmdialogendpoint confirmDialogEndpoint { get; set; }
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

        public class Addtoplaylistendpoint
        {
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

        public class Shareentityendpoint
        {
            public string serializedShareEntity { get; set; }
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
            public Playlisteditendpoint playlistEditEndpoint { get; set; }
        }

        public class Queueaddendpoint
        {
            public Queuetarget queueTarget { get; set; }
            public string queueInsertPosition { get; set; }
            public Command1[] commands { get; set; }
        }

        public class Queuetarget
        {
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

        public class Playlisteditendpoint
        {
            public string playlistId { get; set; }
            public Action[] actions { get; set; }
        }

        public class Action
        {
            public string setVideoId { get; set; }
            public string action { get; set; }
            public string removedVideoId { get; set; }
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
            public Feedbackendpoint feedbackEndpoint { get; set; }
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
            public Feedbackendpoint1 feedbackEndpoint { get; set; }
        }

        public class Feedbackendpoint1
        {
            public string feedbackToken { get; set; }
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
            public Action1[] actions { get; set; }
        }

        public class Target1
        {
            public string videoId { get; set; }
        }

        public class Action1
        {
            public string clickTrackingParams { get; set; }
            public Musiclibrarystatusupdatecommand musicLibraryStatusUpdateCommand { get; set; }
        }

        public class Musiclibrarystatusupdatecommand
        {
            public string libraryStatus { get; set; }
            public string addToLibraryFeedbackToken { get; set; }
        }

        public class Playlistitemdata
        {
            public string playlistSetVideoId { get; set; }
            public string videoId { get; set; }
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
            public Run9[] runs { get; set; }
        }

        public class Run9
        {
            public string text { get; set; }
            public Navigationendpoint1 navigationEndpoint { get; set; }
        }

        public class Navigationendpoint1
        {
            public string clickTrackingParams { get; set; }
            public Watchendpoint2 watchEndpoint { get; set; }
            public Browseendpoint1 browseEndpoint { get; set; }
        }

        public class Watchendpoint2
        {
            public string videoId { get; set; }
            public string playlistId { get; set; }
            public string playerParams { get; set; }
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
            public Run10[] runs { get; set; }
        }

        public class Run10
        {
            public string text { get; set; }
        }

        public class Badge
        {
            public Musicinlinebadgerenderer musicInlineBadgeRenderer { get; set; }
        }

        public class Musicinlinebadgerenderer
        {
            public string trackingParams { get; set; }
            public Icon2 icon { get; set; }
            public Accessibilitydata3 accessibilityData { get; set; }
        }

        public class Icon2
        {
            public string iconType { get; set; }
        }

        public class Accessibilitydata3
        {
            public Accessibilitydata4 accessibilityData { get; set; }
        }

        public class Accessibilitydata4
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

        public class Header
        {
            public Musiceditableplaylistdetailheaderrenderer musicEditablePlaylistDetailHeaderRenderer { get; set; }
        }

        public class Musiceditableplaylistdetailheaderrenderer
        {
            public Header1 header { get; set; }
            public Editheader editHeader { get; set; }
            public string trackingParams { get; set; }
            public string playlistId { get; set; }
        }

        public class Header1
        {
            public Musicdetailheaderrenderer musicDetailHeaderRenderer { get; set; }
        }

        public class Musicdetailheaderrenderer
        {
            public Title1 title { get; set; }
            public Subtitle subtitle { get; set; }
            public Menu1 menu { get; set; }
            public Thumbnail3 thumbnail { get; set; }
            public string trackingParams { get; set; }
            public Morebutton moreButton { get; set; }
            public Secondsubtitle secondSubtitle { get; set; }
        }

        public class Title1
        {
            public Run11[] runs { get; set; }
        }

        public class Run11
        {
            public string text { get; set; }
        }

        public class Subtitle
        {
            public Run12[] runs { get; set; }
        }

        public class Run12
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

        public class Menu1
        {
            public Menurenderer1 menuRenderer { get; set; }
        }

        public class Menurenderer1
        {
            public Item2[] items { get; set; }
            public string trackingParams { get; set; }
            public Toplevelbutton1[] topLevelButtons { get; set; }
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

        public class Item2
        {
            public Menunavigationitemrenderer1 menuNavigationItemRenderer { get; set; }
            public Menuserviceitemrenderer1 menuServiceItemRenderer { get; set; }
        }

        public class Menunavigationitemrenderer1
        {
            public Text6 text { get; set; }
            public Icon3 icon { get; set; }
            public Navigationendpoint3 navigationEndpoint { get; set; }
            public string trackingParams { get; set; }
        }

        public class Text6
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

        public class Navigationendpoint3
        {
            public string clickTrackingParams { get; set; }
            public Watchplaylistendpoint watchPlaylistEndpoint { get; set; }
            public Addtoplaylistendpoint1 addToPlaylistEndpoint { get; set; }
            public Shareentityendpoint1 shareEntityEndpoint { get; set; }
            public Confirmdialogendpoint1 confirmDialogEndpoint { get; set; }
        }

        public class Watchplaylistendpoint
        {
            public string playlistId { get; set; }
            public string _params { get; set; }
        }

        public class Addtoplaylistendpoint1
        {
            public string playlistId { get; set; }
        }

        public class Shareentityendpoint1
        {
            public string serializedShareEntity { get; set; }
        }

        public class Confirmdialogendpoint1
        {
            public Content5 content { get; set; }
        }

        public class Content5
        {
            public Confirmdialogrenderer1 confirmDialogRenderer { get; set; }
        }

        public class Confirmdialogrenderer1
        {
            public Title2 title { get; set; }
            public string trackingParams { get; set; }
            public Dialogmessage1[] dialogMessages { get; set; }
            public Confirmbutton1 confirmButton { get; set; }
            public Cancelbutton1 cancelButton { get; set; }
        }

        public class Title2
        {
            public Run14[] runs { get; set; }
        }

        public class Run14
        {
            public string text { get; set; }
        }

        public class Confirmbutton1
        {
            public Buttonrenderer2 buttonRenderer { get; set; }
        }

        public class Buttonrenderer2
        {
            public string style { get; set; }
            public string size { get; set; }
            public bool isDisabled { get; set; }
            public Text7 text { get; set; }
            public Serviceendpoint2 serviceEndpoint { get; set; }
            public string trackingParams { get; set; }
        }

        public class Text7
        {
            public Run15[] runs { get; set; }
        }

        public class Run15
        {
            public string text { get; set; }
        }

        public class Serviceendpoint2
        {
            public string clickTrackingParams { get; set; }
            public Deleteplaylistendpoint deletePlaylistEndpoint { get; set; }
        }

        public class Deleteplaylistendpoint
        {
            public string playlistId { get; set; }
        }

        public class Cancelbutton1
        {
            public Buttonrenderer3 buttonRenderer { get; set; }
        }

        public class Buttonrenderer3
        {
            public string style { get; set; }
            public string size { get; set; }
            public bool isDisabled { get; set; }
            public Text8 text { get; set; }
            public string trackingParams { get; set; }
        }

        public class Text8
        {
            public Run16[] runs { get; set; }
        }

        public class Run16
        {
            public string text { get; set; }
        }

        public class Dialogmessage1
        {
            public Run17[] runs { get; set; }
        }

        public class Run17
        {
            public string text { get; set; }
        }

        public class Menuserviceitemrenderer1
        {
            public Text9 text { get; set; }
            public Icon4 icon { get; set; }
            public Serviceendpoint3 serviceEndpoint { get; set; }
            public string trackingParams { get; set; }
        }

        public class Text9
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

        public class Serviceendpoint3
        {
            public string clickTrackingParams { get; set; }
            public Queueaddendpoint1 queueAddEndpoint { get; set; }
        }

        public class Queueaddendpoint1
        {
            public Queuetarget1 queueTarget { get; set; }
            public string queueInsertPosition { get; set; }
            public Command2[] commands { get; set; }
        }

        public class Queuetarget1
        {
            public string playlistId { get; set; }
        }

        public class Command2
        {
            public string clickTrackingParams { get; set; }
            public Addtotoastaction1 addToToastAction { get; set; }
        }

        public class Addtotoastaction1
        {
            public Item3 item { get; set; }
        }

        public class Item3
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

        public class Toplevelbutton1
        {
            public Buttonrenderer4 buttonRenderer { get; set; }
        }

        public class Buttonrenderer4
        {
            public string style { get; set; }
            public string size { get; set; }
            public Text10 text { get; set; }
            public Icon5 icon { get; set; }
            public Navigationendpoint4 navigationEndpoint { get; set; }
            public Accessibility2 accessibility { get; set; }
            public string trackingParams { get; set; }
            public Accessibilitydata6 accessibilityData { get; set; }
            public string targetId { get; set; }
        }

        public class Text10
        {
            public Run20[] runs { get; set; }
        }

        public class Run20
        {
            public string text { get; set; }
        }

        public class Icon5
        {
            public string iconType { get; set; }
        }

        public class Navigationendpoint4
        {
            public string clickTrackingParams { get; set; }
            public Watchplaylistendpoint1 watchPlaylistEndpoint { get; set; }
            public Playlisteditorendpoint playlistEditorEndpoint { get; set; }
        }

        public class Watchplaylistendpoint1
        {
            public string playlistId { get; set; }
            public string _params { get; set; }
        }

        public class Playlisteditorendpoint
        {
            public string playlistId { get; set; }
        }

        public class Accessibility2
        {
            public string label { get; set; }
        }

        public class Accessibilitydata6
        {
            public Accessibilitydata7 accessibilityData { get; set; }
        }

        public class Accessibilitydata7
        {
            public string label { get; set; }
        }

        public class Thumbnail3
        {
            public Croppedsquarethumbnailrenderer croppedSquareThumbnailRenderer { get; set; }
        }

        public class Croppedsquarethumbnailrenderer
        {
            public Thumbnail4 thumbnail { get; set; }
            public string trackingParams { get; set; }
        }

        public class Thumbnail4
        {
            public Thumbnail5[] thumbnails { get; set; }
        }

        public class Thumbnail5
        {
            public string url { get; set; }
            public int width { get; set; }
            public int height { get; set; }
        }

        public class Morebutton
        {
            public Togglebuttonrenderer toggleButtonRenderer { get; set; }
        }

        public class Togglebuttonrenderer
        {
            public bool isToggled { get; set; }
            public bool isDisabled { get; set; }
            public Defaulticon1 defaultIcon { get; set; }
            public Defaulttext1 defaultText { get; set; }
            public Toggledicon1 toggledIcon { get; set; }
            public Toggledtext1 toggledText { get; set; }
            public string trackingParams { get; set; }
        }

        public class Defaulticon1
        {
            public string iconType { get; set; }
        }

        public class Defaulttext1
        {
            public Run21[] runs { get; set; }
        }

        public class Run21
        {
            public string text { get; set; }
        }

        public class Toggledicon1
        {
            public string iconType { get; set; }
        }

        public class Toggledtext1
        {
            public Run22[] runs { get; set; }
        }

        public class Run22
        {
            public string text { get; set; }
        }

        public class Secondsubtitle
        {
            public Run23[] runs { get; set; }
        }

        public class Run23
        {
            public string text { get; set; }
        }

        public class Editheader
        {
            public Musicplaylisteditheaderrenderer musicPlaylistEditHeaderRenderer { get; set; }
        }

        public class Musicplaylisteditheaderrenderer
        {
            public Title3 title { get; set; }
            public Edittitle editTitle { get; set; }
            public Description description { get; set; }
            public Editdescription editDescription { get; set; }
            public string privacy { get; set; }
            public string trackingParams { get; set; }
            public string playlistId { get; set; }
            public Collaborationsettingscommand collaborationSettingsCommand { get; set; }
            public Privacydropdown privacyDropdown { get; set; }
        }

        public class Title3
        {
            public Run24[] runs { get; set; }
        }

        public class Description
        {
            public Run24[] runs { get; set; }
        }

        public class Run24
        {
            public string text { get; set; }
        }

        public class Edittitle
        {
            public Run25[] runs { get; set; }
        }

        public class Run25
        {
            public string text { get; set; }
        }

        public class Editdescription
        {
            public Run26[] runs { get; set; }
        }

        public class Run26
        {
            public string text { get; set; }
        }

        public class Collaborationsettingscommand
        {
            public string clickTrackingParams { get; set; }
            public Playlisteditorendpoint1 playlistEditorEndpoint { get; set; }
        }

        public class Playlisteditorendpoint1
        {
            public string playlistId { get; set; }
            public bool openCollaborationPage { get; set; }
        }

        public class Privacydropdown
        {
            public Dropdownrenderer dropdownRenderer { get; set; }
        }

        public class Dropdownrenderer
        {
            public Entry[] entries { get; set; }
            public string label { get; set; }
            public Accessibility3 accessibility { get; set; }
        }

        public class Accessibility3
        {
            public string label { get; set; }
        }

        public class Entry
        {
            public Dropdownitemrenderer dropdownItemRenderer { get; set; }
        }

        public class Dropdownitemrenderer
        {
            public Label label { get; set; }
            public bool isSelected { get; set; }
            public Accessibility4 accessibility { get; set; }
            public string stringValue { get; set; }
            public Icon6 icon { get; set; }
            public Descriptiontext descriptionText { get; set; }
        }

        public class Label
        {
            public Run27[] runs { get; set; }
        }

        public class Run27
        {
            public string text { get; set; }
        }

        public class Accessibility4
        {
            public string label { get; set; }
        }

        public class Icon6
        {
            public string iconType { get; set; }
        }

        public class Descriptiontext
        {
            public Run28[] runs { get; set; }
        }

        public class Run28
        {
            public string text { get; set; }
        }
    }
}