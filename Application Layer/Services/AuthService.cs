using Application_Layer.CQRS.Authantication.Commads.AddRefreshToken;
using Application_Layer.CQRS.Authantication.Quries.GetRefreshToken;
using Application_Layer.CQRS.User.Quries.GetUserbyEmail;
using Application_Layer.CQRS.User.Quries.GetUserByRefreshToken;
using Domain_Layer.DTOs.AthanticationDtos;
using Domain_Layer.Entites.Authantication;
using Domain_Layer.Interfaces.ServiceInterfaces;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration configuration;
        private readonly IMediator mediator;

        public AuthService(IConfiguration configuration,IMediator mediator)
        {
            this.configuration = configuration;
            this.mediator = mediator;
        }
        public async Task<AuthModleDto> GenerateTokensAsync(User user, IEnumerable<Role> UserRols, IEnumerable<Permissions> UserPermissions)
        {
            var authModel = new AuthModleDto
            {
                IsAuthenticated = true
            };

            // ACCESS TOKEN =======================
            var jwt =  CreateToken(user,UserRols,UserPermissions);
            authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwt);
            authModel.TokenExpiresOn = jwt.ValidTo;

            var RefreshTokenResponse = await mediator.Send(new GetRefrshTokenByuserIdQuery(user.Id));

            if (!RefreshTokenResponse.IsSuccess)
            {
                RefreshTokenResponse.Data = GenerateRefreshToken();
                RefreshTokenResponse.Data.userid = user.Id;
                await mediator.Send(new AddRefreshTokenCommand(RefreshTokenResponse.Data));
            }

            authModel.RefreshToken= RefreshTokenResponse.Data.Token;
            authModel.RefreshTokenExpiration = RefreshTokenResponse.Data.ExpiresOn;
            return authModel;

        }

        public async Task<AuthModleDto> RefreshTokenAsync(string refreshToken)
        {
            var UserRespone = await mediator.Send(new GetUserByRefreshTokenQuery(refreshToken));
            if (!UserRespone.IsSuccess)
            {
                return new AuthModleDto
                {
                    IsAuthenticated = false,
                };
            }

            var refreshTokenRespone=await mediator.Send(new GetRefrshTokenByuserIdQuery(UserRespone.Data.Id));
            if (!refreshTokenRespone.IsSuccess)
            {
                return new AuthModleDto
                {
                    IsAuthenticated = false,
                };
            }

            if (refreshTokenRespone.Data is null || !refreshTokenRespone.Data.IsActive)
                return new AuthModleDto { IsAuthenticated = false };

            refreshTokenRespone.Data.IsUsed = true;
            refreshTokenRespone.Data.RevokedOn = DateTime.UtcNow;

            var roles = UserRespone.Data.UserRoles.Select(ur => ur.role);
            var permissions = UserRespone.Data.userPermissions.Select(up => up.permission);
            var jwt = CreateToken(UserRespone.Data, roles, permissions);

            var authModel = new AuthModleDto
            {
                IsAuthenticated = true,
                Token = new JwtSecurityTokenHandler().WriteToken(jwt),
                TokenExpiresOn = jwt.ValidTo,
                RefreshToken = refreshTokenRespone.Data.Token,
                RefreshTokenExpiration = refreshTokenRespone.Data.ExpiresOn,

            };
            return authModel;


        }


        private RefreshTokens GenerateRefreshToken() 
        {
            var random = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(random);

            return new RefreshTokens
            {
                Token = Convert.ToBase64String(random),
                CreatedAt = DateTime.UtcNow,
                ExpiresOn = DateTime.UtcNow.AddDays(7)
            };

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
