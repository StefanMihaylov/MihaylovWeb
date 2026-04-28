using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mihaylov.Api.Gear.Core.Domain.Entities;

namespace Mihaylov.Api.Gear.Infrastructure.Persistence.Configurations;

public class GearNodeConfiguration : IEntityTypeConfiguration<GearNode>
{
    public void Configure(EntityTypeBuilder<GearNode> builder)
    {
        builder.HasKey(x => x.GearNodeId);
        builder.Property(x => x.GearNodeId).ValueGeneratedOnAdd().UseIdentityColumn(1000, 1);

        builder.Property(x => x.Quantity).IsRequired(true);
        builder.Property(x => x.IsPacked).IsRequired(true).HasDefaultValue(false);
        builder.Property(x => x.IsExcluded).IsRequired(true).HasDefaultValue(false);
        builder.Property(x => x.IsRequired).IsRequired(true).HasDefaultValue(false);

        // Trip relationship
        builder.Property(x => x.TripId).IsRequired(true);
        builder.HasOne(x => x.Trip).WithMany(x => x.Nodes).HasForeignKey(x => x.TripId).OnDelete(DeleteBehavior.NoAction);

        // Self-Referencing Recursive Relationship
        builder.Property(x => x.ParentId).IsRequired(false);
        builder.HasOne(x => x.Parent).WithMany(x => x.Children).HasForeignKey(x => x.ParentId).OnDelete(DeleteBehavior.NoAction);

        builder.Property(x => x.NodeTypeId).IsRequired(true);
        builder.HasOne(x => x.NodeType).WithMany().HasForeignKey(x => x.NodeTypeId).IsRequired(true).OnDelete(DeleteBehavior.NoAction);
        builder.Property(x => x.GroupId).IsRequired(false);
        builder.HasOne(x => x.Group).WithMany().HasForeignKey(x => x.GroupId).IsRequired(false).OnDelete(DeleteBehavior.NoAction);
        builder.Property(x => x.CategoryId).IsRequired(false);
        builder.HasOne(x => x.Category).WithMany().HasForeignKey(x => x.CategoryId).IsRequired(false).OnDelete(DeleteBehavior.NoAction);
        builder.Property(x => x.InventoryItemId).IsRequired(false);
        builder.HasOne(x => x.InventoryItem).WithMany().HasForeignKey(x => x.InventoryItemId).IsRequired(false).OnDelete(DeleteBehavior.NoAction);

        // Performance: Composite index for fast tree traversal
        builder.HasIndex(x => new { x.TripId, x.ParentId });
    }
}