using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mihaylov.Api.Dictionary.Database.Models;
using DTO = Mihaylov.Api.Dictionary.Contracts.Models;

namespace Mihaylov.Api.Dictionary.Database.DbConfigurations
{
    internal class RecordTypeConfiguration : IEntityTypeConfiguration<RecordType>
    {
        public void Configure(EntityTypeBuilder<RecordType> builder)
        {
            builder.HasKey(t => t.RecordTypeId).HasName("PK_RecordTypes_RecordTypeId");
            builder.Property(t => t.RecordTypeId).ValueGeneratedOnAdd().IsRequired();

            builder.Property(e => e.Name).IsRequired().HasMaxLength(DTO.RecordType.NameMaxLength);
        }
    }
}
