using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Layer.ViewModels.ProductsViewModels.ListItemViewModel
{
    public class ProductListItemViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        //public decimal Price { get; set; }

        //public int Stock { get; set; }

        public bool IsActive { get; set; } = true;
    }

}
