using Microsoft.AspNet.Identity.EntityFramework;
using Mihaylov.Common.WebConfigSettings;

namespace Mihaylov.Database.Models
{
    public class UsersDbContext : IdentityDbContext<AppUser>
    {
        public UsersDbContext(string connectionString)
            : base(connectionString)
        {
        }

        public static UsersDbContext Create()
        {
            var settingsManager = new CustomSettingsManager();
            string connectionString = settingsManager.GetSettingByName("MihaylovDb");

            return new UsersDbContext(connectionString);
        }
    }
}
