using Domain_Layer.DTOs.ProductVariantDtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Domain_Layer.ViewModels.ProductsViewModels.ProductsVariants.AddVariantToproduct
{
    public class AddVariantToproductViewModle
    {

        [Required(ErrorMessage = "ProductId is required")]
        [Range(1, int.MaxValue, ErrorMessage = "ProductId must be greater than 0")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Variants list is required")]
        [MinLength(1, ErrorMessage = "At least one variant must be added")]
        public IEnumerable<CreateProductVariantDto> CreateProductVariants { get; set; }
            = new List<CreateProductVariantDto>();

    }
}
