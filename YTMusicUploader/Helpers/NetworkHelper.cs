using System.Net;

namespace JBToolkit.Network
{
    /// <summary>
    /// Network status helper
    /// </summary>
    public static class NetworkHelper
    {
        /// <summary>
        /// Detect (strictly 'try' detect - with virtual certainty) if there's an internet connection
        /// </summary>
        public static bool InternetConnectionIsUp()
        {
            try
            {
                using (var client = new WebClient())
                using (client.OpenRead("http://google.com/generate_204"))
                    return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
