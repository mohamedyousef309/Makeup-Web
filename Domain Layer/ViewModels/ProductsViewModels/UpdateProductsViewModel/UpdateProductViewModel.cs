using Domain_Layer.ViewModels.ProductsViewModels.UpdateProductsVariantViewModel;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Layer.ViewModels.ProductsViewModels.UpdateProductsViewModel
{
    public class UpdateProductViewModel
    {
        [Required(ErrorMessage = "Product Id is required.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Product name is required.")]
        [StringLength(200, ErrorMessage = "Product name must not exceed 200 characters.")]
        public string Name { get; set; } = string.Empty;

        [StringLength(1000, ErrorMessage = "Description must not exceed 1000 characters.")]
        public string Description { get; set; } = string.Empty;

        //[Required(ErrorMessage = "Product price is required.")]
        //[Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        //public decimal Price { get; set; }

        //[Required(ErrorMessage = "Product stock is required.")]
        //[Range(0, int.MaxValue, ErrorMessage = "Stock cannot be negative.")]
        //public int Stock { get; set; }

        public int? CategoryId { get; set; }

        public bool IsActive { get; set; } = true;

        public IFormFile? ImageUrl { get; set; }


    }

}
