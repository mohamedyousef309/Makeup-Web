using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infastructure_Layer.DynamicRBASystem
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class HasPermissionAttribute: AuthorizeAttribute
    {
        public const string Prefix = "Permission:";


        public HasPermissionAttribute(bool requireAll, params string[] permissions)
        {
            if (permissions == null|| permissions.Length==0)
            {
                throw new ArgumentException("At least one permission must be provided.", nameof(permissions));
            }

            var mode = requireAll ? "All" : "Any";

            Policy = $"{Prefix}{mode}:{string.Join(',',permissions)}";     // Permission:ALL:User.Create,User.Delete



        }

        public HasPermissionAttribute(string permission) : this(false, permission) { }
    }


}
