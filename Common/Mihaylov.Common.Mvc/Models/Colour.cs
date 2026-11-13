namespace Mihaylov.Common;

/// <summary>
/// Named colour values. Each colour is the Bootstrap text class that provides it, so the actual shade comes from
/// the Bootstrap theme. <see cref="Disabled"/> is not a colour, it means "no colour".
/// </summary>
public class Colour
{
    /// <summary>
    /// Green: the Bootstrap class <c>text-success</c>.
    /// </summary>
    public const string Green = "text-success";

    /// <summary>
    /// Red: the Bootstrap class <c>text-danger</c>.
    /// </summary>
    public const string Red = "text-danger";

    /// <summary>
    /// Orange: the Bootstrap class <c>text-warning</c>. Bootstrap has no orange text class, so this is yellow
    /// in the default theme.
    /// </summary>
    public const string Orange = "text-warning";

    /// <summary>
    /// Blue: the Bootstrap class <c>text-primary</c>. The shade comes from the theme's primary colour, so it follows
    /// theme overrides and can be a colour other than blue.
    /// </summary>
    public const string Blue = "text-primary";

    /// <summary>
    /// No colour. Where a colour is expected, such as the <c>color</c> of the <c>&lt;icon&gt;</c> tag, it removes any
    /// default colour, so the element inherits the colour of its surroundings. It does not grey the element out.
    /// The comparison is case sensitive.
    /// </summary>
    public const string Disabled = "disabled";
}
