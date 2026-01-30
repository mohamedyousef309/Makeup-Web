using Domain_Layer.DTOs.Attribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Layer.DTOs.ProductVariantDtos
{
    public class ProductVariantDto 
    {
      public int Id { get; set; }

        public int ProductId { get; set; }
        public int Stock { get; set; }

        public decimal Price { get; set; }

        public string? VariantImage { get; set; }

        public IEnumerable<AttributeValueResponseDto> SelectedAttributes { get; set; }=new HashSet<AttributeValueResponseDto>();
    }

}
