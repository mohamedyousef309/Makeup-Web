using Domain_Layer.DTOs.AthanticationDtos;
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
    public class AuthService : IAuthService
    {
        private readonly IConfiguration configuration;

        public AuthService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public Task<AuthModleDto> GenerateTokensAsync(User user, IEnumerable<Role> UserRols, IEnumerable<Permissions> UserPermissions)
        {
            var authModel = new AuthModleDto
            {
                IsAuthenticated = true
            };

            // ACCESS TOKEN =======================
            var jwt =  CreateToken(user,UserRols,UserPermissions);
            authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwt);
            authModel.TokenExpiresOn = jwt.ValidTo;
        }

        public Task<AuthModleDto> RefreshTokenAsync(string refreshToken)
        {
            throw new NotImplementedException();
        }


        private JwtSecurityToken CreateToken(User user, IEnumerable<Role> UserRols, IEnumerable<Permissions> UserPermissions)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()), //Makes every JWT unique
                new Claim(JwtRegisteredClaimNames.Sub,user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Iss, configuration["JWT:ValidIssuer"]), //Defines WHO issued the token
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

            var keyInByte = Encoding.UTF8.GetBytes(configuration["JWT:Key"]!);

            var Key = new SymmetricSecurityKey(keyInByte);

            var signingCredentials = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);

            var jwtToken = new JwtSecurityToken(
                issuer: configuration["JWT:ValidIssuer"],
                audience: configuration["JWT:ValidAudience"],
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials: signingCredentials
                );

            return jwtToken;

        }
    }
}
