using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Layer.DTOs.Attribute
{
    public class AttributeGroupDto
    {
        public string Name { get; set; } = default!;
        public IEnumerable<AttributeValueSelectionDto> Values { get; set; } = new HashSet<AttributeValueSelectionDto>();
    }
}

