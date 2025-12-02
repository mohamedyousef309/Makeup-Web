using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Layer.Entites.Authantication
{
    public class Permissions:BaseEntity
    {
        public string Name { get; set; } = default!;

        public IEnumerable<UserPermissions> userPermissions = new HashSet<UserPermissions>();

    }
}
