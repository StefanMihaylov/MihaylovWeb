namespace Mihaylov.Common;

/// <summary>
/// The most common Bootstrap icons used in the views.
/// Icons sources: https://fontawesome.com/v4/icons/, https://icons.getbootstrap.com/
/// </summary>
public enum IconType
{
    /// <summary>
    /// Green check (circle) - bi-check-circle-fill | green, text-success
    /// </summary>
    [Icon("bi-check-circle-fill", Color = Colour.Green)]
    Success = 1,

    /// <summary>
    /// Red X (circle) - bi-x-circle-fill | red, text-danger
    /// </summary>
    [Icon("bi-x-circle-fill", Color = Colour.Red)]
    Error,

    /// <summary>
    /// Red Arrow UP - bi-shift-fill | red, text-danger
    /// </summary>
    [Icon("bi-shift-fill", Color = Colour.Red)]
    Up,

    /// <summary>
    /// Red Thumbs Down - fa-thumbs-down | red, text-danger
    /// </summary>
    [Icon("fa-thumbs-down", Color = Colour.Red)]
    Down,

    /// <summary>
    /// Red close octagon - bi-x-octagon | red, text-danger
    /// </summary>
    [Icon("bi-x-octagon", Color = Colour.Red)]
    Close,

    /// <summary>
    /// Add (plus) - bi-plus-circle | no color
    /// </summary>
    [Icon("bi-plus-circle")]
    New,

    /// <summary>
    /// Add (plus) - fa-plus | no color
    /// </summary>
    [Icon("fa-plus")]
    New2,

    /// <summary>
    /// Edit (pencil) - bi-pencil-fill | no color
    /// </summary>
    [Icon("bi-pencil-fill")]
    Edit,

    /// <summary>
    /// Edit (pencil) - fa-edit | no color
    /// </summary>
    [Icon("fa-edit")]
    Edit2,

    /// <summary>
    /// Delete - bi-eraser | no color
    /// </summary>
    [Icon("bi-eraser")]
    Delete,

    /// <summary>
    /// Delete (fill red) - bi-eraser-fill | red, text-danger
    /// </summary>
    [Icon("bi-eraser-fill", Color = Colour.Red)]
    DeleteFill,

    /// <summary>
    /// Red Delete (trash) - fa-trash | red, text-danger
    /// </summary>
    [Icon("fa-trash", Color = Colour.Red)]
    Delete2,

    /// <summary>
    /// Orange Exclamation circle - bi-exclamation-circle-fill | orange, text-warning
    /// </summary>
    [Icon("bi-exclamation-circle-fill", Color = Colour.Orange)]
    Warning,

    /// <summary>
    /// Orange Exclamation triangle - bi-exclamation-triangle-fill | no color
    /// </summary>
    [Icon("bi-exclamation-triangle-fill")]
    Warning2,

    /// <summary>
    /// Red Exclamation triangle - fa-exclamation-triangle | red, text-danger
    /// </summary>
    [Icon("fa-exclamation-triangle", Color = Colour.Red)]
    Warning3,

    /// <summary>
    /// Orange Pause circle - bi-pause-circle-fill | orange, text-warning
    /// </summary>
    [Icon("bi-pause-circle-fill", Color = Colour.Orange)]
    Pause,

    /// <summary>
    /// Green storage - bi-database-fill | green, text-success
    /// </summary>
    [Icon("bi-database-fill", Color = Colour.Green)]
    Storage,

    /// <summary>
    /// Green Play - bi-play-fill | green, text-success
    /// </summary>
    [Icon("bi-play-fill", Color = Colour.Green)]
    Play,

    /// <summary>
    /// Green Play - bi-play-btn-fill | green, text-success
    /// </summary>
    [Icon("bi-play-btn-fill", Color = Colour.Green)]
    Play2,

    /// <summary>
    /// Question mark circle - bi-question-circle-fill | no color
    /// </summary>
    [Icon("bi-question-circle-fill")]
    Question,

    /// <summary>
    /// Question mark diamond - bi-question-diamond | no color
    /// </summary>
    [Icon("bi-question-diamond")]
    Question2,

    /// <summary>
    /// Repeat circle - bi-repeat | no color
    /// </summary>
    [Icon("bi-repeat")]
    Repeat,

    /// <summary>
    /// Save - bi-floppy2 | no color
    /// </summary>
    [Icon("bi-floppy2")]
    Save,

    /// <summary>
    /// View - bi-binoculars | no color
    /// </summary>
    [Icon("bi-binoculars")]
    View,

    /// <summary>
    /// View - fa-eye | no color
    /// </summary>
    [Icon("fa-eye")]
    View2,

    /// <summary>
    /// Search - bi-search | no color
    /// </summary>
    [Icon("bi-search")]
    Search,

    /// <summary>
    /// List - bi-list-ul | no color
    /// </summary>
    [Icon("bi-list-ul")]
    List,

    /// <summary>
    /// List - fa-list-ul | no color
    /// </summary>
    [Icon("fa-list-ul")]
    List2,

    /// <summary>
    /// Merge - bi-union | no color
    /// </summary>
    [Icon("bi-union")]
    Merge,

    /// <summary>
    /// Download - bi-download | no color
    /// </summary>
    [Icon("bi-download")]
    Download,

    /// <summary>
    /// Open box - fa-box-open | no color
    /// </summary>
    [Icon("fa-box-open")]
    BoxOpen,

    /// <summary>
    /// Back - bi-box-arrow-in-right | no color
    /// </summary>
    [Icon("bi-box-arrow-in-right")]
    Back,

    /// <summary>
    /// Copy - fa-copy | no color
    /// </summary>
    [Icon("fa-copy")]
    Copy,

    /// <summary>
    /// Red Ban - fa-ban | red. text-danger
    /// </summary>
    [Icon("fa-ban", Color = Colour.Red)]
    Banned,
}
