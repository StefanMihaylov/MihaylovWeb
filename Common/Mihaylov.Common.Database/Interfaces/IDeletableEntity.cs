using System;

namespace Mihaylov.Common;

/// <summary>
/// Defines an entity that supports soft deletion and records deletion metadata.
/// </summary>
public interface IDeletableEntity : IEntity
{
    /// <summary>
    /// The date and time the entity was deleted, or null if it has not been deleted.
    /// </summary>
    DateTime? DeletedOn { get; set; }

    /// <summary>
    /// Identifier of the user who deleted the entity.
    /// </summary>
    string DeletedBy { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the entity is soft-deleted.
    /// </summary>
    bool IsDeleted { get; set; }
}
