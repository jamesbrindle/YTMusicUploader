using System;

namespace YTMusicUploader.Providers.Models
{
    [Serializable]
    public class MusicFile
    {
        public MusicFile()
        { }

        public MusicFile(string path)
        {
            Path = path;
        }

        public int Id { get; set; }
        public string Path { get; set; }
        public DateTime LastUpload { get; set; }
        public bool? Error { get; set; }
        public string ErrorReason { get; set; }
    }
}
