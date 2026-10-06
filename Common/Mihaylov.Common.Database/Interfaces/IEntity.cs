using System;

namespace Mihaylov.Common;

/// <summary>
/// Represents an entity with standard audit properties for creation and last modification.
/// </summary>
public interface IEntity
{
    /// <summary>
    /// Gets or sets the date and time when the entity was created.
    /// </summary>
    DateTime CreatedOn { get; set; }

    /// <summary>
    /// Gets or sets the identifier or display name of the user or system that created the resource.
    /// </summary>
    string CreatedBy { get; set; }

    /// <summary>
    /// Gets or sets the date and time the entity was last modified.
    /// </summary>
    DateTime? ModifiedOn { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the user who last modified the resource.
    /// </summary>
    string ModifiedBy { get; set; }
}
