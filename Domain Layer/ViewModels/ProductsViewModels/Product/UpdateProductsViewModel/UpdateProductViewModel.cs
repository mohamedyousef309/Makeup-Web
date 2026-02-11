using Domain_Layer.ViewModels.ProductsViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Layer.ViewModels.ProductsViewModels.Product.UpdateProductsViewModel
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


        public int? CategoryId { get; set; }

        public bool IsActive { get; set; } = true;

        public IFormFile? ImageUrl { get; set; }


    }

}
