using Domain_Layer.DTOs.Attribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Layer.DTOs.ProductVariantDtos
{
    public class VariantWithAtrputeValuesDto
    {
        public int Id { get; set; }

        public int Productid { get; set; }
        public string VariantName { get; set; }

        public decimal Price { get; set; }

        public int Stock { get; set; }


        public string? ImageUrl { get; set; }

        public IEnumerable<AttributeGroupDto> attributeValues { get; set; }=new List<AttributeGroupDto>();
    }
}
