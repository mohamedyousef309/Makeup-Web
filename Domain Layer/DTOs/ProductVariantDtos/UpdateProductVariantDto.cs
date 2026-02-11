using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Layer.DTOs.ProductVariantDtos
{
    public class UpdateProductVariantDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public IFormFile? ImageUrl { get; set; }

        public List<int> SelectedAttributeValueIds { get; set; } = new List<int>();
    }

}
