using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Layer.Entites
{
    public class Category : BaseEntity
    {
       
        public string Name { get; set; } = default!;
        public string? Description { get; set; }

        // One-to-Many: Category → Products
        public IEnumerable<Product> Products { get; set; } = new HashSet<Product>();
    }
}
