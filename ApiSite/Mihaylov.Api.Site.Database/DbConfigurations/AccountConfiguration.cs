using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mihaylov.Api.Site.Database.Models;
using DTO = Mihaylov.Api.Site.Contracts.Models;

namespace Mihaylov.Api.Site.Database.DbConfigurations
{
    internal class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.HasKey(t => t.AccountId).HasName("PK_Accounts_AccountId");
            builder.Property(t => t.AccountId).ValueGeneratedOnAdd().IsRequired().UseIdentityColumn(1000, 1);

            builder.Property(t => t.Username).IsRequired().HasMaxLength(DTO.Account.NameMaxLength);
            builder.Property(t => t.DisplayName).IsRequired().HasMaxLength(DTO.Account.DisplayNameMaxLength);
            builder.Property(t => t.CreateDate).IsRequired(false);
            builder.Property(t => t.LastOnlineDate).IsRequired(false);
            builder.Property(t => t.Details).IsRequired(false).HasMaxLength(DTO.Account.DetailsMaxLength);

            builder.Property(t => t.AccountTypeId).IsRequired();
            builder.HasOne(t => t.AccountType).WithMany().IsRequired().HasForeignKey(t => t.AccountTypeId).OnDelete(DeleteBehavior.NoAction);

            builder.Property(t => t.PersonId).IsRequired();
            builder.HasOne(t => t.Person).WithMany(p => p.Accounts).IsRequired().HasForeignKey(t => t.PersonId).OnDelete(DeleteBehavior.NoAction);

            builder.Property(t => t.StatusId).IsRequired();
            builder.HasOne(t => t.Status).WithMany().IsRequired().HasForeignKey(t => t.StatusId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
