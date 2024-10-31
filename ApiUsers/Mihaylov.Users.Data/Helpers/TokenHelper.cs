using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Mihaylov.Common;
using Mihaylov.Users.Data.Database.Models;
using Mihaylov.Users.Data.Interfaces;

namespace Mihaylov.Users.Data.Helpers
{
    public class TokenHelper : ITokenHelper
    {
        private readonly TokenSettings _appSettings;

        public TokenHelper(IOptions<TokenSettings> appSettings)
        {
            this._appSettings = appSettings.Value;
        }

        public string GetToken(User user, IEnumerable<string> roles, int? customClaimTypes)
        {
            int claimTypes = customClaimTypes ?? _appSettings.ClaimTypes;

            var claims = new List<Claim>
            {
                new Claim("sub", user.Id.ToString()),
            };

            AddClaim(claimTypes, claims, ClaimType.Username, user.UserName);
            AddClaim(claimTypes, claims, ClaimType.Email, user.Email);
            AddClaim(claimTypes, claims, ClaimType.FullName, $"{user.Profile?.FirstName} {user.Profile?.LastName}");
            AddClaim(claimTypes, claims, ClaimType.FirstName, user.Profile?.FirstName);
            AddClaim(claimTypes, claims, ClaimType.LastName, user.Profile?.LastName);

            claims.AddRange(roles.Select(role => new Claim(ClaimsIdentity.DefaultRoleClaimType, role)));

            var keyBytes = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var securityKey = new SymmetricSecurityKey(keyBytes);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),

                NotBefore = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddDays(_appSettings.ExpiresIn),
                SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature),
                Issuer = _appSettings.Issuer,
                Audience = _appSettings.Audience,
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken = tokenHandler.CreateToken(tokenDescriptor);
            string encryptedToken = tokenHandler.WriteToken(securityToken);

            return encryptedToken;
        }

        private void AddClaim(int claimTypes, List<Claim> claims, ClaimType type, string data)
        {
            if (IsEnabled(claimTypes, type))
            {
                claims.Add(new Claim(type.GetClaim(), data));
            }
        }

        private bool IsEnabled(int config, ClaimType claim)
        {
            var configType = (ClaimType)config;
            return (configType & claim) == claim;
        }
    }
}
