using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Layer.Entites
{
    public class Attribute:BaseEntity
    {
        public string Name { get; set; } = default!; // مثلاً: Color أو Size

        // علاقة 1-to-Many مع القيم
        public ICollection<AttributeValue> Values { get; set; } = new HashSet<AttributeValue>();
    }
}
