using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Layer.ViewModels.Permissions
{
    public class AddPermissionToUserViewModel
    {
        [Required(ErrorMessage = "User is required")]
        [Range(1, int.MaxValue, ErrorMessage = "UserId must be greater than 0")]
        public int UserId { get; set; }


        [Required(ErrorMessage = "At least one permission is required")]
        [MinLength(1, ErrorMessage = "At least one permission must be selected")]
        public IEnumerable<int> PermissionIds { get; set; } = new List<int>();
    }
}
