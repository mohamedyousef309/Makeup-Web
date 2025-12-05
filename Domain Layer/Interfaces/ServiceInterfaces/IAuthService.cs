using Domain_Layer.DTOs.AthanticationDtos;
using Domain_Layer.Entites.Authantication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Layer.Interfaces.ServiceInterfaces
{
    public interface IAuthService
    {
        Task<AuthModleDto> GenerateTokensAsync(User user, IEnumerable<Role> UserRols, IEnumerable<Permissions> UserPermissions);
        Task<AuthModleDto> RefreshTokenAsync(string refreshToken);
    }
}
