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
        public int AttributeId { get; set; }
        public string AttributeName { get; set; } 

        [Required(ErrorMessage = "At least one value is required")]
        public List<string> NewValues { get; set; } = new List<string>();
    }
}
