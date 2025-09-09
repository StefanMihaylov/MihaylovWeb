using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mihaylov.Api.Site.Database.Models;

namespace Mihaylov.Api.Site.Database.DbConfigurations
{
    public class DefaultFilterConfiguration : IEntityTypeConfiguration<DefaultFilter>
    {
        public void Configure(EntityTypeBuilder<DefaultFilter> builder)
        {
            builder.HasKey(t => t.DefaultFilterId).HasName("PK_DefaultFilters_DefaultFilterId");
            builder.Property(t => t.DefaultFilterId).ValueGeneratedOnAdd().IsRequired();

            builder.Property(t => t.IsEnabled).IsRequired().HasDefaultValue(false);
            builder.Property(t => t.IsArchive).IsRequired().HasDefaultValue(false);
            builder.Property(t => t.IsPreview).IsRequired(false);

            builder.Property(t => t.AccountTypeId).IsRequired(false);
            builder.HasOne(t => t.AccountType).WithMany().IsRequired(false).HasForeignKey(t => t.AccountTypeId).OnDelete(DeleteBehavior.NoAction);

            builder.Property(t => t.StatusId).IsRequired(false);
            builder.HasOne(t => t.Status).WithMany().IsRequired(false).HasForeignKey(t => t.StatusId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
