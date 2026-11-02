namespace Mihaylov.Common
{
    /// <summary>
    /// Represents settings used to create and validate JSON Web Tokens
    /// </summary>
    public class TokenSettings
    {
        /// <summary> Gets or sets the secret used for authentication or cryptographic operations. </summary>
        public string Secret { get; set; }

        /// <summary> Gets or sets the issuer identifier for the token or credential. </summary>
        public string Issuer { get; set; }

        /// <summary> Gets or sets the intended audience identifier used for token validation.</summary>
        public string Audience { get; set; }

        /// <summary> Gets or sets the claim type that contains the username. </summary>
        public string NameClaimType { get; set; }
    }
}
