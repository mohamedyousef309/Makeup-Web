using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Layer.ViewModels.AttributesViewModle
{
    public class UpdateAttributeValueViewModel
    {
        public int Id { get; set; } 
        public int AttributeId { get; set; } 

        [Required(ErrorMessage = "Value text is required")]
        [MaxLength(100)]
        public string Value { get; set; } = default!;
    }
}
