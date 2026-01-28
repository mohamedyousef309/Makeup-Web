using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Layer.ViewModels.AttributesViewModle
{
    public class AddAttributesWithValuesViewModel
    {
        [Required(ErrorMessage = "Attribute name is required")]
        public string AttributeName { get; set; } = default!;

        public IEnumerable<string?> Values { get; set; } = new HashSet<string?>();
    }
}
