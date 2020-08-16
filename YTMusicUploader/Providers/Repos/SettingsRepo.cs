using Dapper;
using System.Linq;
using YTMusicUploader.Providers.Models;

namespace YTMusicUploader.Providers.Repos
{
    public class SettingsRepo: DataAccess
    {
        public Settings Load()
        {
            using (var conn = DbConnection())
            {
                conn.Open();
                var settings = conn.Query<Settings>(
                        @"SELECT 
                            Id, 
                            StartWithWindows, 
                            ThrottleSpeed, 
                            AuthenticationCookie
                        FROM Settings").FirstOrDefault();
                return settings;
            }
        }

        public void Save()
        {
            using (var conn = DbConnection())
            {
                conn.Open();
                var settings = conn.Execute(
                        @"UPDATE Settings
                             SET StartWithWindows = @StartWithWindows
                                 ThrottleSpeed = @ThrottleSpeed, 
                                 AuthenticationCookie @AuthenticationCookie");
            }
        }
    }
}
