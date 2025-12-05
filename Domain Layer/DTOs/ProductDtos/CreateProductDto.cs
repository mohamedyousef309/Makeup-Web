using Domain_Layer.DTOs.ProductVariantDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Layer.DTOs.ProductDtos
{
    public class CreateProductDto
    {
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public decimal Price { get; set; }

        public int Stock { get; set; }
        public int CategoryId { get; set; }

        
        public List<CreateProductVariantDto>? Variants { get; set; }
    }

}
