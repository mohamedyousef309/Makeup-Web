using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Layer.DTOs.ProductDtos
{
    public class GetAllProductsDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string ImageUrl { get; set; }

        public bool IsAvailable => Stock > 0;

        public string StockMessage => IsAvailable ? "In Stock" : "Out of Stock";
    }
}
