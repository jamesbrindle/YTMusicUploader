using System.Net;

namespace JBTookit.Network
{
    public static class NetworkHelper
    {
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
