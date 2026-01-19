using Domain_Layer.DTOs.ProductVariantDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Layer.ViewModels.ProductsViewModels
{
    public class CreateProductWithVariantsDto
    {
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public int CategoryId { get; set; }

        public List<CreateProductVariantDto?> Variants { get; set; } = new List<CreateProductVariantDto?>();
    }
}
