using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Layer.ViewModels.ProductsViewModels.ProductsVariants.CreateProductsVariantViewModel
{
    public class CreateProductVariantViewModel
    {
        public string VariantName { get; set; } = string.Empty;
        public string VariantValue { get; set; } = string.Empty;
        public int Stock { get; set; }
    }

}
