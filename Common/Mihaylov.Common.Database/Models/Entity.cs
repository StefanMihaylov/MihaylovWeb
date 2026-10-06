using System;

namespace Mihaylov.Common;

/// <summary>
/// Base class that provides creation and modification audit properties.
/// </summary>
public abstract class Entity : IEntity
{
    /// <summary>
    /// Gets or sets the date and time when the entity was created.
    /// </summary>
    public DateTime CreatedOn { get; set; }

    /// <summary>
    /// Gets or sets the name or identifier of the user who created the entity.
    /// </summary>
    public string CreatedBy { get; set; }

    /// <summary>
    /// Gets or sets the date and time the entity was last modified, or null if it has not been modified.
    /// </summary>
    public DateTime? ModifiedOn { get; set; }

    /// <summary>
    /// Gets or sets the name or identifier of the user who last modified the entity.
    /// </summary>
    public string ModifiedBy { get; set; }
}
