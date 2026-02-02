using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Layer.ViewModels.AttributesViewModle
{
    public class AddValuesToAttributeViewModel
    {
        [Required(ErrorMessage = "AttributeId is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid AttributeId")]
        public int AttributeId { get; set; }

        [Required(ErrorMessage = "Attribute name is required")]
        [MaxLength(100, ErrorMessage = "Attribute name must not exceed 100 characters")]
        public string AttributeName { get; set; } = string.Empty;

        [Required(ErrorMessage = "At least one value is required")]
        public List<string> NewValues { get; set; } = new List<string>();
    }
}
