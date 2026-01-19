using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Layer.DTOs.ProductVariantDtos
{
    public class CreateProductVariantDto
    {
        public string VariantName { get; set; } = default!;
        public string VariantValue { get; set; } = default!;
        public int Stock { get; set; }

        public decimal Price { get; set; }
    }

}
