using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Layer.DTOs.Attribute
{
    public class AttributeWithValueDTo
    {
        public int Attributeid { get; set; }

        public string Name { get; set; } = default!; // مثلاً: Color أو Size


        public IEnumerable<AttributeValueDto> Attributes { get; set; } = new HashSet<AttributeValueDto>();
    }
}
