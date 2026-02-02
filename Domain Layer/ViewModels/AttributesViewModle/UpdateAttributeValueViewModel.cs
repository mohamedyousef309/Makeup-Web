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
        [Required(ErrorMessage = "Id is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid Id")]
        public int Id { get; set; }

        [Required(ErrorMessage = "AttributeId is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid AttributeId")]
        public int AttributeId { get; set; }

        [Required(ErrorMessage = "Value text is required")]
        [MaxLength(100, ErrorMessage = "Value must not exceed 100 characters")]
        public string Value { get; set; } = default!;
    }
}
