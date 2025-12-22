using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Layer.Entites.Order
{
    public enum OrderStatus
    {
        [EnumMember(Value = "pending")]
        pending,

        [EnumMember(Value = "PaymentReceving")]
        PaymentReceving,

        [EnumMember(Value = "PaymentFaild")]
        PaymentFaild
    }
    public class Order:BaseEntity
    {
        public string BuyerEmail { get; set; }

        public string PhoneNumber { get; set; } = default!;
        public DateTimeOffset orderDate { get; set; } = DateTimeOffset.Now;
        public OrderStatus status { get; set; } = OrderStatus.pending;
        public string Address { get; set; }

        public decimal Deliverycost { get; set; }

        public IEnumerable<OrderItems> Items { get; set; } = new HashSet<OrderItems>();

        public decimal subTotal { get; set; }

        public decimal GetTotal() => subTotal + Deliverycost;
    }
}
