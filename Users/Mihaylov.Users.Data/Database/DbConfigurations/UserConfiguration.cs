using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mihaylov.Users.Data.Database.Models;

namespace Mihaylov.Users.Data.Database.DbConfigurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.OwnsOne<UserProfile>(u => u.Profile, up =>
                    {
                        up.Property(a => a.FirstName).HasColumnName(nameof(UserProfile.FirstName)).HasMaxLength(25);
                        up.Property(a => a.LastName).HasColumnName(nameof(UserProfile.LastName)).HasMaxLength(25);
                        up.Property(a => a.Gender).HasColumnName(nameof(UserProfile.Gender)).HasConversion<int>();
                        up.HasOne(u => u.GenderModel).WithMany().HasForeignKey(a => a.Gender);
                    });
        }
    }
}
