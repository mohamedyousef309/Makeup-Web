using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Layer.DTOs.Basket
{
    public class CustomerBasketDto
    {
        public string Id { get; set; }
        public ICollection<BasketItemsDto> items { get; set; } = [];

        public string? PaymentIntentID { get; set; }
        public string? ClientSecret { get; set; }

        public int? DeliveryMethodId { get; set; }
    }
}
