using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mihaylov.Site.Database.Models.Base;

namespace Mihaylov.Site.Database.Models.DbConfigurations
{
    public class LookupTableConfiguration<T> : IEntityTypeConfiguration<T> where T : LookupTable
    {
        private readonly int _nameLength;
        private const int DescriptionLength = 60;

        public LookupTableConfiguration(int nameLength)
        {
            _nameLength = nameLength;
        }

        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).ValueGeneratedOnAdd().IsRequired();
            builder.Property(t => t.Name).IsRequired().HasMaxLength(_nameLength);
            builder.Property(t => t.Description).HasMaxLength(DescriptionLength);
        }
    }
}
