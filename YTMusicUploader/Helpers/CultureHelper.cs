using System.Threading;

namespace JBToolkit.Culture
{
    public class CultureHelper
    {
        /// <summary>
        /// GB uses the date format: dd-MM-yyyy
        /// </summary>
        public static void GloballySetCultureToGB()
        {
            var culture = new System.Globalization.CultureInfo("en-GB");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }
    }
}