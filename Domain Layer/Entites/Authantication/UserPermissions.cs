using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Layer.Entites.Authantication
{
    public class UserPermissions:BaseEntity
    {
        public int Userid { get; set; }
        public User user { get; set; } = default!;


        public int PermissionId { get; set; }
        public Permissions permission { get; set; } = default!;  
    }
}
