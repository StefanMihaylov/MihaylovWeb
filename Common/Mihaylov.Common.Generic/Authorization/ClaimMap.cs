using System.Security.Claims;

namespace Mihaylov.Common
{
    /// <summary>
    /// Provides extension methods to map ClaimType values to System.Security.Claims.ClaimTypes strings.
    /// </summary>
    public static class ClaimMap
    {
        /// <summary>
        /// Maps a ClaimType value to the corresponding claim type string from System.Security.Claims.ClaimTypes.
        /// </summary>
        /// <param name="type">The ClaimType to map.</param>
        /// <returns>The corresponding claim type string from System.Security.Claims.ClaimTypes.</returns>
        /// <exception cref="System.ArgumentException">Thrown when the provided ClaimType is not recognized.</exception>
        public static string GetClaim(this ClaimType type)
        {
            switch (type)
            {
                case ClaimType.Username:
                    return ClaimTypes.Upn;

                case ClaimType.Email:
                    return ClaimTypes.Email;

                case ClaimType.FullName:
                    return ClaimTypes.Name;

                case ClaimType.FirstName:
                    return ClaimTypes.GivenName;

                case ClaimType.LastName:
                    return ClaimTypes.Surname;

                default:
                    throw new System.ArgumentException($"Unknown claim type {type}, int: {(int)type}");
            }
        }
    }
}
