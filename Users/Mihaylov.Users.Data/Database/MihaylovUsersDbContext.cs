using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Mihaylov.Users.Data.Database.Models;

namespace Mihaylov.Users.Data.Database
{
    public class MihaylovUsersDbContext : IdentityDbContext<User>
    {
        public MihaylovUsersDbContext(DbContextOptions<MihaylovUsersDbContext> options)
            : base(options)
        {
        }
    }
}
