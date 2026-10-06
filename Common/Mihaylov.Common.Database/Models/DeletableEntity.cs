using System;

namespace Mihaylov.Common;

/// <summary>
/// Represents an entity that supports soft deletion by tracking deletion state and associated metadata.
/// </summary>
public abstract class DeletableEntity : Entity, IDeletableEntity
{
    /// <summary>
    /// Gets or sets the date and time the entity was deleted.
    /// </summary>
    public DateTime? DeletedOn { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the user who deleted the entity.
    /// </summary>
    public string DeletedBy { get; set; }

    /// <summary>
    /// Indicates whether the entity is marked as deleted.
    /// </summary>
    public bool IsDeleted { get; set; }
}
