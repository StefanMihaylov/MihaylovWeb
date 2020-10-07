using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mihaylov.Site.Database.Models.DbConfigurations
{
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).ValueGeneratedOnAdd().IsRequired();
            builder.Property(t => t.Username).IsRequired().HasMaxLength(50);

            builder.HasOne(t => t.AccountType).WithMany().HasForeignKey(t => t.AccountTypeId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(t => t.Person).WithMany(p => p.Accounts).HasForeignKey(t => t.PersonId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
