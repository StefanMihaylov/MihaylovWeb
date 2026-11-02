namespace Mihaylov.Common;

/// <summary>
/// Configuration for a web host’s authentication and session behavior, including cookie name, optional username claim
/// selection, and the login redirect URL.
/// </summary>
public class WebHostSettings
{
    /// <summary> Gets or sets the name of the cookie. </summary>
    public string CookieName { get; set; }

    /// <summary> Gets or sets the URL used to initiate user authentication. </summary>
    public string LoginUrl { get; set; }
}
