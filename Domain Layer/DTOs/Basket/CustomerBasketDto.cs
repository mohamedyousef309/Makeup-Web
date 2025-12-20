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
        public IEnumerable<BasketItemsDto> items { get; set; } = new HashSet<BasketItemsDto>();


    }
}
