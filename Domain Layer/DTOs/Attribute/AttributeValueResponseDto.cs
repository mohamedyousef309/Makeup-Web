using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Layer.DTOs.Attribute
{
    public class AttributeValueResponseDto
    {
        public int Id { get; set; }        
        public string AttributeName { get; set; } = default!; 
        public string Value { get; set; } = default!;         
    }
}
