using Microsoft.AspNet.Identity.EntityFramework;
using Mihaylov.Common.WebConfigSettings.Interfaces;

namespace Mihaylov.Database.Models
{
    public class UsersDbContext : IdentityDbContext<AppUser>
    {
        //public MihaylovDbContext()
        //    : base("MihaylovDbContext", throwIfV1Schema: false)
        //{
        //}

        public UsersDbContext(string connectionString)
            : base(connectionString)
        {
        }

        public static UsersDbContext Create()
        {
           // string connectionString = "MihaylovDbContextCF";

            var settingsManager = new CustomSettingsManager();
            string connectionString = settingsManager.GetSettingByName("MihaylovDbCF");

            return new UsersDbContext(connectionString);
        }
    }
}
