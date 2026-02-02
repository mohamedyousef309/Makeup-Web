using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Layer.ViewModels.Permissions
{
    public class RemoveUserPermissionViewModle
    {
        [Required(ErrorMessage = "User is required")]
        [Range(1, int.MaxValue, ErrorMessage = "UserId must be greater than 0")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Permission is required")]
        [Range(1, int.MaxValue, ErrorMessage = "PermissionId must be greater than 0")]
        public int PermissionId { get; set; }
    }
}
