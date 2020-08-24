using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace YTMusicUploader.Providers.RequestModels
{
    /// <summary>
    /// Deserialised HttpWebRequest body to send to YouTube Music to perform a search of uploaded songs
    /// </summary>
    [Serializable]
    public class SearchContext : IRequestModel
    {
        public Context context { get; set; }

        [JsonProperty("params")]
        public string parameters { get; set; }
        public string query { get; set; }


        [Serializable]
        public class ActivePlayers
        {
        }

        [Serializable]
        public class Capabilities
        {
        }

        [Serializable]
        public class ClickTracking
        {
            public string clickTrackingParams { get; set; }
        }

        [Serializable]
        public class LocationInfo
        {
            public string locationPermissionAuthorizationStatus { get; set; }
        }

        [Serializable]
        public class MusicAppInfo
        {
            public string musicActivityMasterSwitch { get; set; }
            public string musicLocationMasterSwitch { get; set; }
            public string pwaInstallabilityStatus { get; set; }
        }

        [Serializable]
        public class Client
        {
            public string browserName { get; set; }
            public string browserVersion { get; set; }
            public string clientName { get; set; }
            public string clientVersion { get; set; }
            public List<object> experimentIds { get; set; }
            public string experimentsToken { get; set; }
            public string gl { get; set; }
            public string hl { get; set; }
            public LocationInfo locationInfo { get; set; }
            public MusicAppInfo musicAppInfo { get; set; }
            public string osName { get; set; }
            public string osVersion { get; set; }
            public int utcOffsetMinutes { get; set; }
        }

        [Serializable]
        public class InternalExperimentFlag
        {
            public string key { get; set; }
            public string value { get; set; }
        }

        [Serializable]
        public class Request
        {
            public List<InternalExperimentFlag> internalExperimentFlags { get; set; }
            public int sessionIndex { get; set; }
        }

        [Serializable]
        public class User
        {
            public bool enableSafetyMode { get; set; }
        }

        [Serializable]
        public class Context
        {
            public ActivePlayers activePlayers { get; set; }
            public Capabilities capabilities { get; set; }
            public ClickTracking clickTracking { get; set; }
            public Client client { get; set; }
            public Request request { get; set; }
            public User user { get; set; }
        }
    }
}
