using System;
using YTMusicUploader.Providers.Repos;

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

        public int Id { get; set; } = -1;
        public string Path { get; set; }
        public DateTime LastUpload { get; set; }
        public bool? Error { get; set; }
        public string ErrorReason { get; set; }

        public DbOperationResult Save()
        {
            if (Id > 0)
            {
                return new MusicFileRepo().Update(this);
            }
            else
            {
                var result = new MusicFileRepo().Insert(this);
                if (!result.IsError)
                    Id = result.Id;

                return result;
            }
        }

        public DbOperationResult Delete()
        {
            var result = new MusicFileRepo().Delete(this);
            if (!result.IsError)
                Id = -1;

            return result;
        }
    }
}
