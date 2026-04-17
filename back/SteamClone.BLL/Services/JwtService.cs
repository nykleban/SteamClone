using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using SteamClone.BLL.Settings;
using SteamClone.DAL.Entities;


namespace SteamClone.BLL.Services
{
    public class JwtService
    {
        private readonly JwtSettings _jwtSettings;

        public JwtService(IOptions<JwtSettings> options)
        {
            _jwtSettings = options.Value;
        }

        public string GetAcessToken(UserEntity user, IEnumerable<string> roles)
        {
            if (string.IsNullOrEmpty(_jwtSettings.SecretKey))
            {
                throw new ArgumentNullException("Jwt secret key is null");
            }

            var claims = new List<Claim>()
            {
                new Claim("userName", user.UserName ?? string.Empty),
                new Claim("email", user.Email ?? string.Empty),
                new Claim("firstName", user.FirstName ?? string.Empty),
                new Claim("lastName", user.LastName ?? string.Empty),
                new Claim("image", user.Image ?? string.Empty)
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var secretKeyBytes = Encoding.UTF8.GetBytes(_jwtSettings.SecretKey);
            var signInKey = new SymmetricSecurityKey(secretKeyBytes);

            var credentials = new SigningCredentials(signInKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                signingCredentials: credentials,
                expires: DateTime.UtcNow.AddHours(_jwtSettings.ExpireHours)
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
