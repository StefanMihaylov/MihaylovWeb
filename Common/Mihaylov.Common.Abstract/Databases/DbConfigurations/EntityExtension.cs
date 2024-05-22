using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mihaylov.Common.Abstract.Databases.Models;

namespace Microsoft.EntityFrameworkCore
{
    public  static class EntityExtension
    {
        public static void EntityConfiguration<T>(this EntityTypeBuilder<T> builder) where T: Entity
        {
            builder.Property(c => c.CreatedBy).IsRequired(true).HasMaxLength(256);
            builder.Property(c => c.CreatedOn).IsRequired(true).HasPrecision(3);
            builder.Property(c => c.ModifiedBy).IsRequired(false).HasMaxLength(256);
            builder.Property(c => c.ModifiedOn).IsRequired(false).HasPrecision(3);
        }
    }
}
