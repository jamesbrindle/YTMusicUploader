using JBToolkit.Threads;
using System;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using YTMusicUploader.Business.Pipes;

namespace YTMusicUploader
{
    static class Program
    {
        /// <summary>
        /// Basically used so a new instance of YTMusicUploader can just show / restore the existing process window
        /// </summary>
        internal static WcfServer WcfServer { get; set; } = new WcfServer("JBS_YTMusicUploader");
        internal static MainForm MainForm = null;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {

            //
            // Don't allow 2 instances of application to open
            // Try and focus existing window instead
            //
            using (new Mutex(true, "YTMusicUploader", out bool createdNew))
            {
                if (createdNew)
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);

                    WcfServer.Received += ProcessWcfMessage;
                    WcfServer.Faulted += ServerFaulted;
                    WcfServer.Start();

                    if (args.Contains("-hidden"))
                        MainForm = new MainForm(true);
                    else
                        MainForm = new MainForm(false);

                    Application.Run(MainForm);
                }
                else
                {
                    // Send a message to current process tellin it to show

                    var wcfClient = new WcfClient("JBS_YTMusicUploader");
                    int sleepCount = 0;

                    while (true)
                    {
                        try
                        {
                            wcfClient.Send("Show");
                            break;
                        }
                        catch
                        {
                            if (sleepCount > 7000)
                                break;

                            ThreadHelper.SafeSleep(50);
                            sleepCount += 50;

                            wcfClient = new WcfClient("JBS_YTMusicUploader");
                        }
                    }
                }
            }
        }

        private static void ServerFaulted(object sender, EventArgs e)
        {
            if (MainForm != null)
            {
                WcfServer = new WcfServer("JBS_YTMusicUploader");
                WcfServer.Received += ProcessWcfMessage;
                WcfServer.Faulted += ServerFaulted;
                WcfServer.Start();
            }
        }

        private static void ProcessWcfMessage(object sender, DataReceivedEventArgs e)
        {
            if (e.Data == "Show")
                if (MainForm != null)
                    MainForm.ShowForm();
        }
    }
}
