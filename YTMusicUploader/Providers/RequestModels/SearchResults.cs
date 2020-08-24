using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace YTMusicUploader.Providers.RequestModels
{
    /// <summary>
    /// Deserialised HttpWebRequest response body send by YouTube Music of search results
    /// </summary>
    [Serializable]
    public class SearchResult : IRequestModel
    {
        public ResponseContext responseContext { get; set; }
        public Contents contents { get; set; }
        public string trackingParams { get; set; }

        [Serializable]
        public class Param
        {
            public string key { get; set; }
            public string value { get; set; }
        }

        [Serializable]
        public class ServiceTrackingParam
        {
            public string service { get; set; }

            [JsonProperty("params")]
            public List<Param> paramaters { get; set; }
        }

        [Serializable]
        public class ResponseContext
        {
            public string visitorData { get; set; }
            public List<ServiceTrackingParam> serviceTrackingParams { get; set; }
        }

        [Serializable]
        public class SearchEndpoint
        {
            public string query { get; set; }

            [JsonProperty("params")]
            public string paramaters { get; set; }
        }

        [Serializable]
        public class Endpoint
        {
            public string clickTrackingParams { get; set; }
            public SearchEndpoint searchEndpoint { get; set; }
        }

        [Serializable]
        public class Run
        {
            public string text { get; set; }
        }

        [Serializable]
        public class Text
        {
            public List<Run> runs { get; set; }
        }

        [Serializable]
        public class Icon
        {
            public string iconType { get; set; }
        }

        [Serializable]
        public class MessageRenderer
        {
            public Text text { get; set; }
            public string trackingParams { get; set; }
            public Icon icon { get; set; }
        }

        [Serializable]
        public class Content3
        {
            public MessageRenderer messageRenderer { get; set; }
        }

        [Serializable]
        public class ItemSectionRenderer
        {
            public List<Content3> contents { get; set; }
            public string trackingParams { get; set; }
        }

        [Serializable]
        public class Content2
        {
            public ItemSectionRenderer itemSectionRenderer { get; set; }
        }

        [Serializable]
        public class SectionListRenderer
        {
            public List<Content2> contents { get; set; }
            public string trackingParams { get; set; }
        }

        [Serializable]
        public class Content
        {
            public SectionListRenderer sectionListRenderer { get; set; }
        }

        [Serializable]
        public class TabRenderer
        {
            public Endpoint endpoint { get; set; }
            public string title { get; set; }
            public bool selected { get; set; }
            public string trackingParams { get; set; }
            public Content content { get; set; }
        }

        [Serializable]
        public class Tab
        {
            public TabRenderer tabRenderer { get; set; }
        }

        [Serializable]
        public class TabbedSearchResultsRenderer
        {
            public List<Tab> tabs { get; set; }
        }

        [Serializable]
        public class Contents
        {
            public TabbedSearchResultsRenderer tabbedSearchResultsRenderer { get; set; }
        }

    }
}