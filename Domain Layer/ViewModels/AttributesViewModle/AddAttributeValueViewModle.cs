using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Layer.ViewModels.AttributesViewModle
{
    public class AddAttributeValueViewModle
    {
        [Required(ErrorMessage = "Attribute value is required")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Value must be between 1 and 50 characters")]
        public string Value { get; set; } = default!;

       
        public int AttributeId { get; set; }
    }
}
