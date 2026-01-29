using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Layer.DTOs.Attribute
{
    public class UpdateAttributeDto
    {
        public int Id { get; set; }
        public string AttributeName { get; set; } = string.Empty;
        public IEnumerable<string?> Values { get; set; } = new List<string?>();
    }
}
