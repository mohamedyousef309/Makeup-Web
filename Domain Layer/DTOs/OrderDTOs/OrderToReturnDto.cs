using Domain_Layer.Entites.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Layer.DTOs.OrderDTOs
{
    public class OrderToReturnDto
    {

        public int orderid { get; set; }
        public string BuyerEmail { get; set; }

        public string PhoneNumber { get; set; } = default!;
        public DateTimeOffset orderDate { get; set; } = DateTimeOffset.Now;
        public string status { get; set; } 
        public string Address { get; set; }

        public decimal Deliverycost { get; set; }

        public ICollection<OrderItemsDTo> Items { get; set; } = new HashSet<OrderItemsDTo>();

        public decimal subTotal { get; set; }

    }
}
