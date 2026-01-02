using Domain_Layer.ViewModels.ProductsViewModels.CreateProductsVariantViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Layer.ViewModels.ProductsViewModels.CreateProductsViewModel
{
    public class CreateProductViewModel
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int Stock { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public List<CreateProductVariantViewModel> Variants { get; set; } = new List<CreateProductVariantViewModel>();
    }

}
