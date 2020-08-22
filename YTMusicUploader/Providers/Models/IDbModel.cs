namespace YTMusicUploader.Providers.Models
{
    public interface IDbModel
    {
        int Id { get; set; }

        DbOperationResult Save();

        DbOperationResult Delete();
    }
}
