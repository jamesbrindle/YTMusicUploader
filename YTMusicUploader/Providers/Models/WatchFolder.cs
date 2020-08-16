using System;
using YTMusicUploader.Providers.Repos;

namespace YTMusicUploader.Providers.Models
{
    [Serializable]
    public class WatchFolder
    {
        public int Id { get; set; }
        public string Path { get; set; }

        public DbOperationResult Save()
        {
            var result = new WatchFolderRepo().Insert(this);
            if (!result.IsError)
                Id = result.Id;

            return result;
        }

        public DbOperationResult Delete()
        {
            var result = new WatchFolderRepo().Delete(this);
            if (!result.IsError)
                Id = -1;

            return result;
        }
    }
}
