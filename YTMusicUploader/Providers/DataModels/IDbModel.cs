namespace YTMusicUploader.Providers.DataModels
{
    public interface IDbModel
    {
        int Id { get; set; }

        DbOperationResult Save();

        DbOperationResult Delete();
    }
}
