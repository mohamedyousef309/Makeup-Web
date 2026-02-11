using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Layer.ViewModels.ProductsViewModels.ProductsVariants.UpdateProductsVariantViewModel
{
    public class UpdateProductVariantViewModel
    {
        [Required(ErrorMessage = "Variant Id is required")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Product Id is required")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Variant name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters")]
        public string VariantName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, 1000000, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Stock quantity is required")]
        [Range(0, 10000, ErrorMessage = "Stock must be a positive number")]
        public int Stock { get; set; }

        // جعلناها Nullable لأن الأدمن قد لا يريد تغيير الصورة في كل مرة
        [Display(Name = "Update Variant Image")]
        public IFormFile? ImageFile { get; set; }
    }

}
