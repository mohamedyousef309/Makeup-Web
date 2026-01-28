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


        public decimal Price { get; set; }

        public int Stock { get; set; }

        public string? SKU { get; set; }

        public ICollection<VariantAttributeValue> ProductVariantAttributeValues { get; set; } = new HashSet<VariantAttributeValue>();
        public bool ReduceStock(int quantity)
        {
            Stock -= quantity;
            return Stock == 0;
        }

    }

}
