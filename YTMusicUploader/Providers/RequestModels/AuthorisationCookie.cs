using Newtonsoft.Json;

namespace YTMusicUploader.Providers.RequestModels
{
    public class AuthorisationCookie
    {
        [JsonProperty("__Secure-1PAPISID")]
        public string __Secure1PAPISID { get; set; }

        [JsonProperty("__Secure-1PSID")]
        public string __Secure1PSID { get; set; }

        [JsonProperty("__Secure-3PAPISID")]
        public string __Secure3PAPISID { get; set; }

        [JsonProperty("__Secure-3PSID")]
        public string __Secure3PSID { get; set; }

        [JsonProperty("__Secure-3PSIDCC")]
        public string __Secure3PSIDCC { get; set; }

        public string _gcl_au { get; set; }
        public string APISID { get; set; }
        public string CONSENT { get; set; }
        public string HSID { get; set; }
        public string LOGIN_INFO { get; set; }
        public string PREF { get; set; }
        public string SAPISID { get; set; }
        public string SID { get; set; }
        public string SIDCC { get; set; }
        public string SSID { get; set; }
        public string VISITOR_INFO1_LIVE { get; set; }
        public string YSC { get; set; }

        public override string ToString()
        {
            string cookieString = string.Empty;

            if (VISITOR_INFO1_LIVE != null)
                cookieString += $"VISITOR_INFO1_LIVE={VISITOR_INFO1_LIVE}; ";

            if (_gcl_au != null)
                cookieString += $"_gcl_au={_gcl_au}; ";

            if (PREF != null)
                cookieString += $"PREF={PREF}; ";

            if (HSID != null)
                cookieString += $"HSID={HSID}; ";

            if (SSID != null)
                cookieString += $"SSID={SSID}; ";

            if (APISID != null)
                cookieString += $"APISID={APISID}; ";

            if (SAPISID != null)
                cookieString += $"SAPISID={SAPISID}; ";

            if (__Secure3PAPISID != null)
                cookieString += $"__Secure-3PAPISID={__Secure3PAPISID}; ";

            if (LOGIN_INFO != null)
                cookieString += $"LOGIN_INFO={LOGIN_INFO}; ";

            if (SID != null)
                cookieString += $"SID={SID}; ";

            if (__Secure3PSID != null)
                cookieString += $"__Secure-3PSID={__Secure3PSID}; ";

            if (YSC != null)
                cookieString += $"YSC={YSC}; ";

            if (SIDCC != null)
                cookieString += $"SIDCC={SIDCC}; ";

            if (__Secure3PSIDCC != null)
                cookieString += $"__Secure-3PSIDCC={__Secure3PSIDCC}; ";

            if (__Secure1PAPISID != null)
                cookieString += $"__Secure-1PAPISID={__Secure1PAPISID}; ";

            if (__Secure1PSID != null)
                cookieString += $"__Secure-1PSID={__Secure1PSID}; ";

            return cookieString.Trim();
        }
    }
}
