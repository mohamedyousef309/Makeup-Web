using Domain_Layer.DTOs.AthanticationDtos;
using Domain_Layer.Entites.Authantication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Layer.DTOs.UserDTOS
{
    public class UserToReturnWithRolsDTo
    {
        public int Id { get; set; }
        public string Username { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
        public string Picture { get; set; } = default!;
        public string UserAddress { get; set; } = default!;

        public IEnumerable<UserRolsDTo> UserRoles { get; set; } = new HashSet<UserRolsDTo>();


        public IEnumerable<UserPermissionsDTo> UserPermissions { get; set; } = new HashSet<UserPermissionsDTo>();
    }
}
