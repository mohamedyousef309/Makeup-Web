using Domain_Layer.DTOs.OrderDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Layer.ViewModels.Order.UpdateOrderDetailsViewModels
{
    public class UpdateOrderDetailsViewModel
    {
        public int OrderId { get; set; }
        public List<OrderItemUpdateDto> Items { get; set; } = new();
    }
}
