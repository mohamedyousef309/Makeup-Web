using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Layer.ViewModels.ProductsViewModels.ProductsVariants.AddAttrputeToVariant
{
    public class AddAttrputeToVariantViewModle
    {
        [Required(ErrorMessage = "Product Variant is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid Product Variant Id")]
        public int ProductVariantId { get; set; }

        [Required(ErrorMessage = "AttributeValueId is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid AttributeValueId Id")]
        public int AttributeValueId { get; set; }
    }
}
