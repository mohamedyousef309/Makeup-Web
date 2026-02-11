using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Layer.DTOs.ProductVariantDtos
{
    public class VariantDbDto
    {
        public int id { get; set; }

        public string VariantName { get; set; }
        public string ProductName { get; set; }
        public int productid { get; set; }
        public decimal price { get; set; }

        public int Stock { get; set; }

        public string? ImageUrl { get; set; }

        public string VariantValue { get; set; }

    }
}
