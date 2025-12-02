using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Layer.Entites.Authantication
{
    public class UserRole:BaseEntity
    {
        public int Userid { get; set; } = default!;
        public User user { get; set; } = default!;

        public int Roleid { get; set; } = default!;

        public Role role { get; set; } = default!;
    }
}
