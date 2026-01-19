using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Layer.Entites
{
    public class ProductVariant : BaseEntity
    {


        // FK to Product
        public int ProductId { get; set; }
        public Product Product { get; set; } = default!;

        public string VariantName { get; set; } = default!;   // e.g., "Color", "Shade"
        public string VariantValue { get; set; } = default!;  // e.g., "01 Ivory", "Red 33"

        public decimal Price { get; set; }

        public int Stock { get; set; }

        public bool ReduceStock(int quantity)
        {
            Stock -= quantity;
            return Stock == 0;
        }

    }

}
