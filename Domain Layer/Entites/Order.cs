using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Layer.Entites
{
    public class Order : BaseEntity
    {
        

        

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string Status { get; set; } = "Pending";
        // Possible: Pending, Confirmed, Shipped, Delivered, Cancelled

        public decimal TotalPrice { get; set; }

        // One-to-Many: Order → Items
        public IEnumerable<OrderItem> Items { get; set; } = new HashSet<OrderItem>();
    }

}
