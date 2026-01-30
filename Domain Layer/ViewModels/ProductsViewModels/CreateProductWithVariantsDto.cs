using Domain_Layer.DTOs.ProductVariantDtos;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Layer.ViewModels.ProductsViewModels
{
    public class CreateProductWithVariantsDto
    {
        [Required(ErrorMessage = "Product name is required")]
        [MinLength(2, ErrorMessage = "Product name must be at least 2 characters")]
        [MaxLength(100, ErrorMessage = "Product name must not exceed 100 characters")]
        public string Name { get; set; } = default!;

        [MaxLength(500, ErrorMessage = "Description must not exceed 500 characters")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Category is required")]
        [Range(1, int.MaxValue, ErrorMessage = "CategoryId must be greater than 0")]
        public int CategoryId { get; set; }

        public IFormFile? Productpecture { get; set; }



        public List<CreateProductVariantDto?> Variants { get; set; } = new List<CreateProductVariantDto?>();
    }
}
