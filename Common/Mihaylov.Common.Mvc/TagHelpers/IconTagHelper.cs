using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Mihaylov.Common;

/// <summary>
/// Renders a Bootstrap or Font Awesome icon: <c>&lt;icon type="IconType.Success" /&gt;</c>.
/// The icon is an <c>&lt;i&gt;</c> element. It is wrapped in a <c>&lt;span&gt;</c> only when the span has
/// something to carry: a CSS colour, <see cref="Class"/>, or other attributes written on the tag.
/// </summary>
[HtmlTargetElement("icon", TagStructure = TagStructure.WithoutEndTag)]
public class IconTagHelper : TagHelper
{
    private static readonly IDictionary<IconType, IconAttribute> IconAttributes = GetIcons();

    /// <summary>
    /// Gets or sets one of the common icons. Use the full enum name, for example <c>IconType.Success</c>.
    /// Provides the icon and its default colour.
    /// </summary>
    public IconType? Type { get; set; }

    /// <summary>
    /// Gets or sets the icon class to render, for example <c>bi-house</c> or <c>fa-trash</c>. It wins over the icon of
    /// <see cref="Type"/>. A name starting with <c>bi-</c> or <c>fa-</c> also gets the base class <c>bi</c> or <c>fa</c>.
    /// Any other value is used as is, so write all the classes yourself (for example <c>fab fa-github</c>).
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the colour of the icon. It is either:
    /// <list type="bullet">
    /// <item>a Bootstrap text class (<c>text-danger</c>), which is added to the <c>&lt;i&gt;</c> element, or</item>
    /// <item>a CSS colour (<c>green</c>, <c>#198754</c>), which is set as an inline style on the <c>&lt;span&gt;</c> element.</item>
    /// </list>
    /// When empty, the colour of the <see cref="Type"/> value is used. The value <c>disabled</c> removes any colour,
    /// including the default one, so the icon inherits the text colour.
    /// </summary>
    public string Color { get; set; }

    /// <summary>
    /// Gets or sets space separated classes to add to the <c>&lt;span&gt;</c> element. Setting it
    /// always renders the span. Other attributes written on the tag (<c>id</c>, <c>title</c>, <c>style</c>, <c>data-*</c>)
    /// also go to the span, and a <c>style</c> written on the tag is placed after the colour, so it wins.
    /// </summary>
    public string Class { get; set; }

    /// <summary>
    /// Builds the HTML of an icon for code that runs outside a view, for example a view model.
    /// The result is the <c>&lt;i&gt;</c> element only, without a <c>&lt;span&gt;</c>.
    /// </summary>
    /// <param name="iconType">The icon type. Provides the icon and its default colour.</param>
    /// <param name="iconColor">A Bootstrap text class (<c>text-danger</c>), or <see cref="Colour.Disabled"/> for no colour.
    /// When empty, the default colour of <paramref name="iconType"/> is used. A CSS colour (<c>green</c>) is ignored, because there is no span to carry it.</param>
    /// <param name="iconName">The icon class to render, for example <c>bi-house</c>. It wins over the icon of <paramref name="iconType"/>.</param>
    /// <returns>The HTML of the icon, for example <c>&lt;i aria-hidden="true" class="bi bi-x-circle-fill text-danger"&gt;&lt;/i&gt;</c>.</returns>
    /// <exception cref="InvalidOperationException">Thrown when <paramref name="iconType"/> has no <see cref="IconAttribute"/>.</exception>
    public static string BuildIcon(IconType iconType, string iconColor = null, string iconName = null)
    {
        var defaults = ResolveDefaults(iconType);
        string color = GetColor(iconColor, defaults?.Color);

        var icon = BuildIcon(iconName, defaults?.Name, color);

        using var writer = new StringWriter();
        icon.WriteTo(writer, HtmlEncoder.Default);
        var result = writer.ToString();

        return result;
    }

    /// <inheritdoc />
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        var defaults = ResolveDefaults(Type);
        string color = GetColor(Color, defaults?.Color);

        var icon = BuildIcon(Name, defaults?.Name, color);

        string cssColor = IsTextClass(color) ? null : color;

        if (cssColor == null && string.IsNullOrWhiteSpace(Class) && output.Attributes.Count == 0)
        {
            output.TagName = null;
            output.Content.SetHtmlContent(icon);
            return;
        }

        ConfigureSpan(output, cssColor, Class);
        output.Content.SetHtmlContent(icon);
    }

    /// <summary>
    /// Gets the <see cref="IconAttribute"/> of an <see cref="IconType"/> value: its icon class and default colour.
    /// </summary>
    /// <param name="iconType">The icon type, or null.</param>
    /// <returns>The attribute, or null when <paramref name="iconType"/> is null.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the value has no <see cref="IconAttribute"/>.</exception>
    public static IconAttribute ResolveDefaults(IconType? iconType)
    {
        if (!iconType.HasValue)
        {
            return null;
        }

        if (!IconAttributes.TryGetValue(iconType.Value, out IconAttribute defaults) || defaults == null)
        {
            throw new InvalidOperationException($"IconType.{iconType} has no [Icon] attribute.");
        }

        return defaults;
    }

    private static string GetColor(string inputColor, string defaultColor)
    {
        string color = !string.IsNullOrWhiteSpace(inputColor) ? inputColor : defaultColor;
        if (string.IsNullOrWhiteSpace(color) || color == Colour.Disabled)
        {
            color = null;
        }

        return color;
    }

    private static void ConfigureSpan(TagHelperOutput output, string cssColor, string className)
    {
        output.TagName = "span";
        output.TagMode = TagMode.StartTagAndEndTag;

        if (cssColor != null)
        {
            var style = $"color:{cssColor}";
            if (output.Attributes.TryGetAttribute("style", out var existing) && existing.Value != null)
            {
                style += $";{existing.Value}";
            }

            output.Attributes.SetAttribute("style", style);
        }

        if (!string.IsNullOrWhiteSpace(className))
        {
            foreach (var c in className.Split((char[])null, StringSplitOptions.RemoveEmptyEntries))
            {
                output.AddClass(c, HtmlEncoder.Default);
            }
        }
    }

    private static IHtmlContent BuildIcon(string name, string defaultName, string color)
    {
        var iconClass = !string.IsNullOrWhiteSpace(name) ? name : defaultName;
        if (string.IsNullOrWhiteSpace(iconClass))
        {
            throw new InvalidOperationException("<icon> needs a 'type' or a 'name'.");
        }

        var icon = new TagBuilder("i")
        {
            TagRenderMode = TagRenderMode.Normal
        };

        if (iconClass.StartsWith("bi-", StringComparison.Ordinal))
        {
            icon.AddCssClass("bi");
        }
        else if (iconClass.StartsWith("fa-", StringComparison.Ordinal))
        {
            icon.AddCssClass("fa");
        }

        icon.AddCssClass(iconClass);

        if (IsTextClass(color))
        {
            icon.AddCssClass(color);
        }

        icon.Attributes["aria-hidden"] = "true";

        return icon;
    }

    private static bool IsTextClass(string color)
    {
        if (string.IsNullOrWhiteSpace(color))
        {
            return false;
        }

        return color.StartsWith("text-", StringComparison.Ordinal);
    }

    private static IDictionary<IconType, IconAttribute> GetIcons()
    {
        return typeof(IconType)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .ToDictionary(f => (IconType)f.GetValue(null), f => f.GetCustomAttribute<IconAttribute>());
    }
}
