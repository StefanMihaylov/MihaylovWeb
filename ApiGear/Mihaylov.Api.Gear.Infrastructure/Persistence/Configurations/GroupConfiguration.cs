using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mihaylov.Api.Gear.Core.Domain.Lookups;

namespace Mihaylov.Api.Gear.Infrastructure.Persistence.Configurations;

public class GroupConfiguration : IEntityTypeConfiguration<Group>
{
    public void Configure(EntityTypeBuilder<Group> builder)
    {
        builder.HasKey(x => x.GroupId);
        builder.Property(x => x.GroupId).ValueGeneratedOnAdd().UseIdentityColumn(1, 1);

        builder.Property(x => x.Name).IsRequired(true).HasMaxLength(200);
    }
}
