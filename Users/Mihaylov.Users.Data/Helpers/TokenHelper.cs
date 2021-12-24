using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Mihaylov.Users.Data.Database.Models;
using Mihaylov.Users.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Mihaylov.Users.Data.Helpers
{
    public class TokenHelper : ITokenHelper
    {
        private readonly TokenSettings _appSettings;

        public TokenHelper(IOptions<TokenSettings> appSettings)
        {
            this._appSettings = appSettings.Value;
        }

        public string GetToken(User user, IEnumerable<string> roles)
        {
            var claims = new List<Claim>
            {
                new Claim("sub", user.Id.ToString()),
            };

            if (IsEnabled(_appSettings.ClaimTypes, ClaimType.Username))
            {
                claims.Add(new Claim(ClaimTypes.NameIdentifier, user.UserName));
            }

            if (IsEnabled(_appSettings.ClaimTypes, ClaimType.Email))
            {
                claims.Add(new Claim(ClaimTypes.Email, user.Email));
            }

            if (IsEnabled(_appSettings.ClaimTypes, ClaimType.FullName))
            {
                claims.Add(new Claim(ClaimTypes.Name, $"{user.Profile?.FirstName} {user.Profile?.LastName}"));
            }

            if (IsEnabled(_appSettings.ClaimTypes, ClaimType.FirstName))
            {
                claims.Add(new Claim(ClaimTypes.GivenName, user.Profile?.FirstName));
            }

            if (IsEnabled(_appSettings.ClaimTypes, ClaimType.LastName))
            {
                claims.Add(new Claim(ClaimTypes.Surname, user.Profile?.LastName));
            }

            claims.AddRange(roles.Select(role => new Claim(ClaimsIdentity.DefaultRoleClaimType, role)));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                
                NotBefore = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddDays(_appSettings.ExpiresIn),
                SigningCredentials = new SigningCredentials(GetKey(), SecurityAlgorithms.HmacSha256Signature),
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken = tokenHandler.CreateToken(tokenDescriptor);
            string encryptedToken = tokenHandler.WriteToken(securityToken);

            return encryptedToken;
        }

        public void SetJwtBearerOptions(JwtBearerOptions options)
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                IssuerSigningKey = GetKey(),
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                ValidateIssuer = false,
                ValidateAudience = false
            };
        }

        private SecurityKey GetKey()
        {
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            return new SymmetricSecurityKey(key);
        }

        private bool IsEnabled(int config, ClaimType claim)
        {
            var configType = (ClaimType)config;
            return (configType & claim) == claim;
        }
    }
}
