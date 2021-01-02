using JBToolkit.Network;
using JBToolkit.Threads;
using System.Threading;
using YTMusicUploader.Providers;

namespace YTMusicUploader.Business
{
    /// <summary>
    /// Performs operations while the application is in idle state, such as retreiving data from MusicBrainz
    /// and updating the MusicFile database entries with the retreived data
    /// </summary>
    public class IdleProcessor
    {
        private MainForm MainForm { get; set; }
        public bool Paused { get; set; } = true;
        public bool Stopped { get; set; } = false;
        public Thread IdleProcessorThread { get; set; }

        public IdleProcessor(MainForm mainForm)
        {
            MainForm = mainForm;
            Stopped = false;

            IdleProcessorThread = new Thread((ThreadStart)delegate
            {
                while (true)
                {
                    try
                    {
                        while (Paused)
                        {
                            if (MainFormAborting())
                                return;

                            ThreadHelper.SafeSleep(2000);
                        }

                        while (!NetworkHelper.InternetConnectionIsUp())
                        {
                            if (MainFormAborting())
                                return;

                            ThreadHelper.SafeSleep(5000);
                        }

                        while (MainForm.ManagingYTMusicStatus == MainForm.ManagingYTMusicStatusEnum.Showing)
                        {
                            MainForm.SetPaused(true);
                            ThreadHelper.SafeSleep(1000);
                        }
                        if (MainForm.ManagingYTMusicStatus == MainForm.ManagingYTMusicStatusEnum.CloseChanges)
                            return;

                        PopulateRandomMusicEntryWithMissingMbId();
                        ThreadHelper.SafeSleep(100);
                        PopulateRandomMusicEntryWithMissingEntityId();

                        if (MainFormAborting())
                            return;

                        ThreadHelper.SafeSleep(2000);
                    }
                    catch { }
                }
            })
            {
                IsBackground = true,
                Priority = ThreadPriority.Lowest
            };

            IdleProcessorThread.Start();
        }

        private bool MainFormAborting()
        {
            if (MainForm.Aborting)
            {
                Stopped = true;
                return true;
            }

            return false;
        }

        private void PopulateRandomMusicEntryWithMissingMbId()
        {
            var musicFile = MainForm.MusicFileRepo.GetRandmonMusicFileWithMissingMbId().Result;
            if (musicFile != null && !string.IsNullOrEmpty(musicFile.Path))
            {
                var trackAndReleaseMbId = MainForm.MusicDataFetcher.GetTrackAndReleaseMbId(musicFile.Path, true);

                if (!string.IsNullOrEmpty(trackAndReleaseMbId.MbId))
                    musicFile.MbId = trackAndReleaseMbId.MbId;

                if (!string.IsNullOrEmpty(trackAndReleaseMbId.ReleaseMbId))
                    musicFile.ReleaseMbId = trackAndReleaseMbId.ReleaseMbId;

                musicFile.Save().Wait();
            }
        }

        private void PopulateRandomMusicEntryWithMissingEntityId()
        {
            if (MainForm.ConnectedToYTMusic)
            {
                var musicFile = MainForm.MusicFileRepo.GetRandmonMusicFileWithMissingEntityId().Result;
                if (musicFile != null && !string.IsNullOrEmpty(musicFile.Path))
                {
                    string entityId = musicFile.EntityId;
                    Requests.IsSongUploaded(
                        musicFile.Path,
                        MainForm.Settings.AuthenticationCookie,
                        ref entityId,
                        out string videoId,
                        MainForm.MusicDataFetcher,
                        false,
                        false);

                    if (!string.IsNullOrEmpty(entityId))
                    {
                        musicFile.EntityId = entityId;
                        musicFile.VideoId = videoId;
                        musicFile.Save().Wait();
                    }
                }
            }
        }
    }
}
