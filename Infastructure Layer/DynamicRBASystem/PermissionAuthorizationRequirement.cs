using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infastructure_Layer.DynamicRBASystem
{
    public class PermissionAuthorizationRequirement : IAuthorizationRequirement
    {

        public string[] AllowedPermissions { get; }
        public bool RequireAll { get; }

        public PermissionAuthorizationRequirement(string[] allowedPermissions, bool requireAll = false)
        {
            AllowedPermissions = allowedPermissions;
            RequireAll = requireAll;
        }

    }

}
