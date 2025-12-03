using Domain_Layer.CQRS.Authantication;
using Domain_Layer.Entites.Authantication;
using Domain_Layer.Interfaces.ServiceInterfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.Services
{
    public class GenerateTokensAsync : IGenerateTokensAsync
    {
        private readonly IConfiguration _config;

        public GenerateTokensAsync(IConfiguration config )
        {
            this._config = config;
        }
        public string GenerateTokenAsync(User user, IEnumerable<Role> UserRols, IEnumerable<Permissions> UserPermissions)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()), //Makes every JWT unique
                new Claim(JwtRegisteredClaimNames.Sub,user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Iss, _config["JWT:ValidIssuer"]), //Defines WHO issued the token
                new Claim(ClaimTypes.Name,user.Username),
                new Claim(ClaimTypes.Email,user.Email),
            };

            foreach (var item in UserRols)
            {
                claims.Add(new Claim(ClaimTypes.Role, item.Name));

            }

            foreach (var item in UserPermissions)
            {
                claims.Add(new Claim("Permission", item.Name));
            }

            var keyInByte = Encoding.UTF8.GetBytes(_config["JWT:Key"]!);

            var Key = new SymmetricSecurityKey(keyInByte);

            var signingCredentials = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);

            var jwtToken = new JwtSecurityToken(
                issuer: _config["JWT:ValidIssuer"],
                audience: _config["JWT:ValidAudience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials: signingCredentials
                );

            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }
    }
}
