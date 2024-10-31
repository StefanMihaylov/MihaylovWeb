using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mihaylov.Common.Database.Models;

namespace Microsoft.EntityFrameworkCore
{
    public static class EntityExtension
    {
        public static void EntityConfiguration<T>(this EntityTypeBuilder<T> builder) where T : Entity
        {
            const int UserNameLength = 256;
            const int CreatedOnPrecision = 3;

            builder.Property(c => c.CreatedBy).IsRequired(true).HasMaxLength(UserNameLength);
            builder.Property(c => c.CreatedOn).IsRequired(true).HasPrecision(CreatedOnPrecision);
            builder.Property(c => c.ModifiedBy).IsRequired(false).HasMaxLength(UserNameLength);
            builder.Property(c => c.ModifiedOn).IsRequired(false).HasPrecision(CreatedOnPrecision);
        }
    }
}
