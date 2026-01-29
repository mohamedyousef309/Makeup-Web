using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Layer.DTOs.Attribute
{
    public class AttributeValueDto
    {
        public int id {  get; set; }
        public string Value { get; set; } = default!; // مثلاً: Red

        public int AttributeId { get; set; }
    }
}
