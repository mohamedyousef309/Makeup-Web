using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Layer.DTOs.Attribute
{
    public class AddValuesToAttributeDto
    {
        public int AttributeId { get; set; } 
        public IEnumerable<string> Values { get; set; } = new List<string>(); 
    }
}
