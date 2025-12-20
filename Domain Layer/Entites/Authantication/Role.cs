using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Layer.Entites.Authantication
{
    public class Role:BaseEntity
    {
        public string Name { get; set; }=default!;


        public IEnumerable<UserRole> UserRoles { get; set; } = new HashSet<UserRole>();




    }
}
