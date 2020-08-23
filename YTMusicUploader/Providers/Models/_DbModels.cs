namespace YTMusicUploader.Providers.Models
{
    public abstract class DbModels : IDbModel
    {
        public int Id { get; set; } = -1;

        public abstract DbOperationResult Save();

        public abstract DbOperationResult Delete();
    }
}
