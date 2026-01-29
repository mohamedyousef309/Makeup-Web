using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Layer.ViewModels.AttributesViewModle
{
    public class UpdateAttributeViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Attribute Name is required")]
        [StringLength(50, MinimumLength = 2)]
        public string AttributeName { get; set; } = string.Empty;
    }
}
