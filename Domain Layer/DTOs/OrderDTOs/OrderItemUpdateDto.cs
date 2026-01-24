using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Layer.DTOs.OrderDTOs
{
    public class OrderItemUpdateDto
    {
        public int ProductVariantId { get; set; }
        public int Quantity { get; set; }
    }
}
