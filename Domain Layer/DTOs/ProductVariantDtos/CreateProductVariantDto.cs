using Domain_Layer.DTOs.Attribute;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Layer.DTOs.ProductVariantDtos
{
    public class CreateProductVariantDto
    {
        [Required(ErrorMessage = "Variant name is required")]
        [MinLength(2, ErrorMessage = "Variant name must be at least 2 characters")]
        [MaxLength(50, ErrorMessage = "Variant name must not exceed 50 characters")]
        public string VariantName { get; set; } = default!;

        [Required(ErrorMessage = "Variant value is required")]
        [MinLength(1, ErrorMessage = "Variant value cannot be empty")]
        [MaxLength(50, ErrorMessage = "Variant value must not exceed 50 characters")]
        public string VariantValue { get; set; } = default!;

        [Range(0, int.MaxValue, ErrorMessage = "Stock cannot be negative")]
        public int Stock { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }

        public IFormFile? Variantpecture { get; set; }

        [Required(ErrorMessage = "Attribute values are required")]
        [MinLength(1, ErrorMessage = "At least one attribute value must be selected")]
        public IEnumerable<int> AttributeValueId { get; set; } = new HashSet<int>();
    }

}
