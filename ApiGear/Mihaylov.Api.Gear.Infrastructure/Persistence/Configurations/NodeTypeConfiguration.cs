using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mihaylov.Api.Gear.Core.Domain.Enums;

namespace Mihaylov.Api.Gear.Infrastructure.Persistence.Configurations
{
    public class NodeTypeConfiguration : IEntityTypeConfiguration<NodeTypeDb>
    {
        public void Configure(EntityTypeBuilder<NodeTypeDb> builder)
        {
            builder.HasKey(x => x.NodeTypeId);

            builder.Property(x => x.NodeTypeId).ValueGeneratedNever();
            builder.Property(x => x.Name).IsRequired().HasMaxLength(50);

            var nodeTypes = Enum.GetValues<NodeType>()
                .Select(e => new NodeTypeDb
                {
                    NodeTypeId = (int)e,
                    Name = e.ToString()
                });

            builder.HasData(nodeTypes);
        }
    }
}
