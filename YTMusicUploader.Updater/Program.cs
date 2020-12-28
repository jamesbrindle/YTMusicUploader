using System;
using System.Windows.Forms;

namespace YTMusicUploader.Updater
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length == 4)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new UpdaterForm(args[0], args[1], args[2], args[3]));
            }
        }
    }
}
