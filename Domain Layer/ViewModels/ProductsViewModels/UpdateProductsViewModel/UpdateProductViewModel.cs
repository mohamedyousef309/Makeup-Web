using Domain_Layer.ViewModels.ProductsViewModels.UpdateProductsVariantViewModel;
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
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int Stock { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public bool IsActive { get; set; } = true;

        public List<UpdateProductVariantViewModel> Variants { get; set; } = new List<UpdateProductVariantViewModel>();
    }

}
