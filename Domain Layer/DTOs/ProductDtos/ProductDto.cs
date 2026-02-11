using Domain_Layer.DTOs.ProductVariantDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Layer.DTOs.ProductDtos
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public string ImageUrl { get; set; }

        public int? CategoryId { get; set; }

        public IEnumerable<VariantDbDto> VariantsDto { get; set; } = new HashSet<VariantDbDto>();
    }
}
