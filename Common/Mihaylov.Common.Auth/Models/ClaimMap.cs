using System;
using System.Security.Claims;

namespace Mihaylov.Common
{
    /// <summary>
    /// Provides extension methods to map ClaimType values to System.Security.Claims.ClaimTypes strings.
    /// </summary>
    public static class ClaimMap
    {
        /// <summary>
        /// Maps a claim type name to the matching claim type string from <see cref="System.Security.Claims.ClaimTypes"/>.
        /// </summary>
        /// <param name="value">
        /// The name of a <see cref="ClaimType"/> value, for example <c>"Email"</c> (the case does not matter).
        /// <c>null</c>, empty or white space selects <c>Username</c>.
        /// </param>
        /// <returns>The corresponding claim type string from <see cref="System.Security.Claims.ClaimTypes"/>.</returns>
        /// <exception cref="System.ArgumentException">
        /// Thrown when <paramref name="value"/> is not the name of a <see cref="ClaimType"/> value (for example a typo),
        /// or is a number or a list that is not a single defined value, for example <c>"99"</c> or <c>"Username,Email"</c>.
        /// </exception>
        public static string GetClaim(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return ClaimType.Username.GetClaim();
            }

            if (!Enum.TryParse(value, true, out ClaimType type))
            {
                throw new ArgumentException(
                    $"Unknown claim type '{value}'. Use one of: {string.Join(", ", Enum.GetNames(typeof(ClaimType)))}.",
                    nameof(value));
            }

            return type.GetClaim();
        }

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
