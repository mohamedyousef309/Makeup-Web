using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Layer.Entites
{
    public class Product : BaseEntity
    {

        public string Name { get; set; } = default!;

        public string? Description { get; set; }

        public decimal Price { get; set; }

        public int Stock { get; set; }

        public bool IsActive { get; set; } = true;

        public int productStock { get; set; }


        // FK to Category
        public int CategoryId { get; set; }
        public Category Category { get; set; } = default!;

        // One-to-Many: Product → Variants
        public IEnumerable<ProductVariant> Variants { get; set; } = new HashSet<ProductVariant>();


        public bool ReduceStock(int quantity)
        {
            Stock -= quantity;
            return Stock == 0; 
        }
    }

    }
