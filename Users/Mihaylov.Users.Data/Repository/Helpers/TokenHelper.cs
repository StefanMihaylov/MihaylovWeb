﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Mihaylov.Users.Data.Database.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Mihaylov.Users.Data.Repository.Helpers
{
    public class TokenHelper : ITokenHelper
    {
        private readonly AppUserSettings _appSettings;

        public TokenHelper(IOptions<AppUserSettings> appSettings)
        {
            this._appSettings = appSettings.Value;
        }

        public string GetToken(User user, IEnumerable<string> roles)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.Id.ToString())
            };

            claims.AddRange(roles.Select(role => new Claim(ClaimsIdentity.DefaultRoleClaimType, role)));

            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken = tokenHandler.CreateToken(tokenDescriptor);
            string encryptedToken = tokenHandler.WriteToken(securityToken);
            return encryptedToken;
        }

        public void SetJwtBearerOptions(JwtBearerOptions options)
        {
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false
            };
        }
    }
}