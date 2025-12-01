using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Layer.Entites
{
    public class OrderItem
    {
        public int Id { get; set; }

        // FK to Order
        public int OrderId { get; set; }
        public Order Order { get; set; } = default!;

        // FK to Product
        public int ProductId { get; set; }
        public Product Product { get; set; } = default!;

        // FK to ProductVariant (optional)
        public int? VariantId { get; set; }
        public ProductVariant? Variant { get; set; }

        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; } // price at order time
    }

}
