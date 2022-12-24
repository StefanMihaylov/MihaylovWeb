using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mihaylov.Common.Abstract.Databases.Models;

namespace Mihaylov.Common.Abstract.Databases.DbConfigurations
{
    public abstract class EntityConfiguration<T> : IEntityTypeConfiguration<T> where T : Entity
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property(t => t.CreatedBy).HasMaxLength(60);
            builder.Property(t => t.ModifiedBy).HasMaxLength(60);
        }
    }
}
