using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Layer.ViewModels.ProductsViewModels.ProductsVariants
{
    public class UpdateProdcutVariantStockViewModle
    {
        [Required(ErrorMessage = "Variant Id is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid product variant id")]
        public int ProductVariantId { get; set; }

        [Required(ErrorMessage = "Stock is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Stock cannot be negative")]
        public int NewStock { get; set; }

        [Required(ErrorMessage = "Product Id is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid Product Id")]
        public int ProductId { get; set; }
    }
}
