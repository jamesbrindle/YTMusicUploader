using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YTMusicUploader.Providers.Repos;

namespace YTMusicUploader.Providers.DataModels
{
    public class PlaylistFile : DbModels
    {
        public PlaylistFile()
        { }

        public PlaylistFile(string path, DateTime lastModifiedDate)
        {
            Path = path;
            LastModifiedDate = lastModifiedDate;
        }

        public string Title { get; set; }
        public string Description { get; set; }
        public string PlaylistId { get; set; }
        public string Path { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public DateTime? LastUpload { get; set; }
        public List<PlaylistFileItem> PlaylistItems { get; set; } = new List<PlaylistFileItem>();

        public async override Task<DbOperationResult> Delete()
        {
            var result = await new PlaylistFileRepo().Delete(this);
            if (!result.IsError)
                Id = -1;

            return result;
        }

        public async override Task<DbOperationResult> Save()
        {
            if (Id > 0)
            {
                return await new PlaylistFileRepo().Update(this);
            }
            else
            {
                var result = await new PlaylistFileRepo().Insert(this);
                if (!result.IsError)
                    Id = result.Id;

                return result;
            }
        }

        public class PlaylistFileItem
        {
            public string Path { get; set; }
            public string Artist { get; set; }
            public string Album { get; set; }
            public string Track { get; set; }
            public string VideoId { get; set; }
        }
    }
}
