namespace Mihaylov.Common;

/// <summary>
/// Represents settings used to create and validate JSON Web Tokens, containing the signing secret, issuer, and
/// audience.
/// </summary>
public class JwtTokenSettings
{
    /// <summary>
    /// Gets or sets the secret used for authentication or cryptographic operations.
    /// </summary>
    public string Secret { get; set; }

    /// <summary>
    /// Gets or sets the issuer identifier for the token or credential.
    /// </summary>
    public string Issuer { get; set; }

    /// <summary>
    /// Gets or sets the intended audience identifier used for token validation.
    /// </summary>
    public string Audience { get; set; }

    /// <summary>
    /// Copies secret, issuer, and audience values from another JwtTokenSettings into the current instance.
    /// </summary>
    /// <param name="other">The source JwtTokenSettings to copy values from.</param>
    public void Copy(JwtTokenSettings other)
    {
        Secret = other.Secret;
        Issuer = other.Issuer;
        Audience = other.Audience;
    }
}
