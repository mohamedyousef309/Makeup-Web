using Domain_Layer.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Layer.ViewModels.ProductsViewModels
{
    public class CreatVariantDto
    {
        public int ProductId { get; set; }

        public string VariantName { get; set; } = default!;  
        public string VariantValue { get; set; } = default!;  

        public decimal Price { get; set; }

        public int Stock { get; set; }
    }
}
