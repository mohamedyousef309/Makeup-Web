using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infastructure_Layer.DynamicRBASystem
{
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionAuthorizationRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionAuthorizationRequirement requirement)
        {
            var userPermissions = context.User.Claims
                 .Where(c => c.Type == "Permission")
                 .Select(c => c.Value)
                 .ToList();

            bool isAuthorized = requirement.RequireAll
                 ? requirement.AllowedPermissions.All(p => userPermissions.Contains(p))
                 : requirement.AllowedPermissions.Any(p => userPermissions.Contains(p));

            if (isAuthorized)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }

}
