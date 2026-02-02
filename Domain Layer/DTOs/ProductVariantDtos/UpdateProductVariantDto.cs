using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Layer.DTOs.ProductVariantDtos
{
    public class UpdateProductVariantViewModel
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string? ImageUrl { get; set; }

        // دي اللي كانت مسببة Error في الكنترولر (لازم تكون موجودة)
        public List<int> SelectedAttributeValueIds { get; set; } = new List<int>();
    }

}
