using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Layer.Entites
{
    public class Order
    {
        public int Id { get; set; }

        

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string Status { get; set; } = "Pending";
        // Possible: Pending, Confirmed, Shipped, Delivered, Cancelled

        public decimal TotalPrice { get; set; }

        // One-to-Many: Order → Items
        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
    }

}
