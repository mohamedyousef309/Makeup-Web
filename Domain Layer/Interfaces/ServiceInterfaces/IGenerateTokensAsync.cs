using Domain_Layer.CQRS.Authantication;
using Domain_Layer.Entites.Authantication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Layer.Interfaces.ServiceInterfaces
{
    public interface IGenerateTokensAsync
    {
        string GenerateTokenAsync(User user,IEnumerable<Role> UserRols,IEnumerable<Permissions> UserPermissions);
        
    }
}
