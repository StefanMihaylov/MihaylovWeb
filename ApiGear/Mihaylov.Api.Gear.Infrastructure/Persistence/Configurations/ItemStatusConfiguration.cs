using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mihaylov.Api.Gear.Core.Domain.Enums;

namespace Mihaylov.Api.Gear.Infrastructure.Persistence.Configurations;

public class ItemStatusConfiguration : IEntityTypeConfiguration<ItemStatusDb>
{
    public void Configure(EntityTypeBuilder<ItemStatusDb> builder)
    {
        builder.HasKey(x => x.ItemStatusId);

        builder.Property(x => x.ItemStatusId).ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(50);

        var nodeTypes = Enum.GetValues<ItemStatus>()
            .Select(e => new ItemStatusDb
            {
                ItemStatusId = (int)e,
                Name = e.ToString()
            });

        builder.HasData(nodeTypes);
    }
}
