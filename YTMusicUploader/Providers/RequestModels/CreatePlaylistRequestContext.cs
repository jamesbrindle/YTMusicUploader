namespace YTMusicUploader.Providers.RequestModels
{
    /// <summary>
    /// Deserialised HttpWebRequest request body sent for creating a playlist
    /// </summary>
    public class CreatePlaylistRequestContext
    {
        public Context context { get; set; }
        public string[] videoIds { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string privacyStatus { get; set; }

        public class Context
        {
            public Client client { get; set; }
            public Capabilities capabilities { get; set; }
            public Request request { get; set; }
            public Activeplayers activePlayers { get; set; }
            public User user { get; set; }
        }

        public class Client
        {
            public string clientName { get; set; }
            public string clientVersion { get; set; }
            public string hl { get; set; }
            public string gl { get; set; }
            public object[] experimentIds { get; set; }
            public string experimentsToken { get; set; }
            public string browserName { get; set; }
            public string browserVersion { get; set; }
            public string osName { get; set; }
            public string osVersion { get; set; }
            public string platform { get; set; }
            public int utcOffsetMinutes { get; set; }
            public Locationinfo locationInfo { get; set; }
            public Musicappinfo musicAppInfo { get; set; }
        }

        public class Locationinfo
        {
            public string locationPermissionAuthorizationStatus { get; set; }
        }

        public class Musicappinfo
        {
            public string musicActivityMasterSwitch { get; set; }
            public string musicLocationMasterSwitch { get; set; }
            public string pwaInstallabilityStatus { get; set; }
        }

        public class Capabilities
        {
        }

        public class Request
        {
            public Internalexperimentflag[] internalExperimentFlags { get; set; }
            public string sessionIndex { get; set; }
        }

        public class Internalexperimentflag
        {
            public string key { get; set; }
            public string value { get; set; }
        }

        public class Activeplayers
        {
        }

        public class User
        {
            public bool enableSafetyMode { get; set; }
        }
    }
}