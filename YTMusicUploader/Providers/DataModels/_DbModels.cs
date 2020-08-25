using System.Threading.Tasks;

namespace YTMusicUploader.Providers.DataModels
{
    public abstract class DbModels : IDbModel
    {
        public int Id { get; set; } = -1;

        public abstract Task<DbOperationResult> Save();

        public abstract Task<DbOperationResult> Delete();
    }
}
