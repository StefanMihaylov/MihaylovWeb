using Microsoft.AspNet.Identity.EntityFramework;

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
            return new UsersDbContext("MihaylovDbContextCF");
        }
    }
}
