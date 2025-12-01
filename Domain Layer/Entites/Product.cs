using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Layer.Entites
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
    
        public string? Description { get; set; }

        public decimal Price { get; set; }
      
        public int Stock { get; set; }

        public bool IsActive { get; set; } = true;
     

        // FK to Category
        public int CategoryId { get; set; }
        public Category Category { get; set; } = default!;

        // One-to-Many: Product → Variants
        public ICollection<ProductVariant> Variants { get; set; } = new List<ProductVariant>();
    }
}
