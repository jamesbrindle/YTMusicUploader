using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YTMusicUploader.Business
{
    public class PlaylistProcessor
    {
        private MainForm MainForm { get; set; }
        public bool Stopped { get; set; } = false;

        public PlaylistProcessor(MainForm mainForm)
        {
            MainForm = mainForm;
        }

        public void Processor()
        {
            Stopped = false;
        }
    }
}
