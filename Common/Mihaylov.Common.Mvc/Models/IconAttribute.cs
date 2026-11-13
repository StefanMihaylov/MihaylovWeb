using System;

namespace Mihaylov.Common;

/// <summary>
/// Sets the Bootstrap icon and the default colour of an <see cref="IconType"/> value.
/// </summary>
/// <param name="name">The Bootstrap icon class, for example <c>bi-check-circle-fill</c>.</param>
[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public sealed class IconAttribute(string name) : Attribute
{
    /// <summary>
    /// Gets the Bootstrap icon class.
    /// </summary>
    public string Name { get; } = name;

    /// <summary>
    /// Gets or sets the default colour: a CSS colour or a Bootstrap text class. Null inherits the text colour.
    /// </summary>
    public string Color { get; set; }
}

