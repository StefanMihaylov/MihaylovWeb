using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mihaylov.Common;

namespace Microsoft.EntityFrameworkCore;

/// <summary>
/// Configures common auditing properties for Entity types on an <![CDATA[EntityTypeBuilder<T>]]>, including CreatedBy,
/// CreatedOn, ModifiedBy, and ModifiedOn.
/// </summary>
public static class EntityExtension
{
    /// <summary>
    /// Configures common audit properties for an entity type: CreatedBy and CreatedOn are required; ModifiedBy and
    /// ModifiedOn are optional. Applies a 256-character limit to user names and a precision of 3 to date/time
    /// values.
    /// </summary>
    /// <typeparam name="T">The entity CLR type to configure; must derive from Entity.</typeparam>
    /// <param name="builder">The EntityTypeBuilder for the entity type.</param>
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
