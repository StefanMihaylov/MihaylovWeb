using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mihaylov.Api.Other.Contracts.Base;
using Mihaylov.Api.Other.Database.Shows.Models;

namespace Mihaylov.Api.Other.Database.Shows.DbConfigurations
{
    internal class TicketProviderConfiguration : IEntityTypeConfiguration<TicketProvider>
    {
        public void Configure(EntityTypeBuilder<TicketProvider> builder)
        {
            builder.HasKey(b => b.TickerProviderId).HasName("TickerProviderId");

            builder.Property(c => c.Name).IsRequired().HasMaxLength(ModelConstants.TicketProviderNameMaxLength);
            builder.Property(c => c.Url).HasMaxLength(ModelConstants.TicketProviderUrlMaxLength);
            builder.EntityConfiguration();

            builder.HasIndex(b => b.Name).IsUnique();
        }
    }
}
