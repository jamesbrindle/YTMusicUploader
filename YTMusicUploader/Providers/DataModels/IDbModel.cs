using System.Threading.Tasks;

namespace YTMusicUploader.Providers.DataModels
{
    public interface IDbModel
    {
        int Id { get; set; }

        Task<DbOperationResult> Save();

        Task<DbOperationResult> Delete();
    }
}
