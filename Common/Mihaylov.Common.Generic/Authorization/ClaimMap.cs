using System.Security.Claims;

namespace Mihaylov.Common
{
    public static class ClaimMap
    {
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
