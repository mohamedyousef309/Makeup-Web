using Domain_Layer.Entites.Order;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Layer.ViewModels.Order
{
    public class UpdateOrderStatusViewModle
    {
        [Required(ErrorMessage = "OrderId is required")]
        [Range(1, int.MaxValue, ErrorMessage = "OrderId must be greater than 0")]
        public int Orderid { get; set; }

        [Required(ErrorMessage = "Order status is required")]
        [EnumDataType(typeof(OrderStatus), ErrorMessage = "Invalid order status")]
        public OrderStatus OrderStatus { get; set; }
    }
}
