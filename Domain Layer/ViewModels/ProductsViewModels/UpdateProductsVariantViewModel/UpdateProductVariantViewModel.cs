using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Layer.ViewModels.ProductsViewModels.UpdateProductsVariantViewModel
{
    public class UpdateProductVariantViewModel
    {
        [Required(ErrorMessage = "Variant Id is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid Variant Id")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Product Id is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid Product Id")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Variant name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Variant name must be between 2 and 100 characters")]
        public string VariantName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Variant value is required")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Variant value must be between 1 and 100 characters")]
        public string VariantValue { get; set; } = string.Empty;

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, 1000000, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }

        
    }

}
