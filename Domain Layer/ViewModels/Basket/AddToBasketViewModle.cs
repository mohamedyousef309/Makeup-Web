using System.ComponentModel.DataAnnotations;

namespace Domain_Layer.ViewModels.Basket
{
    public class AddToBasketViewModle
    {
        [Required(ErrorMessage = "ProductId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "ProductId must be greater than 0.")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "ProductVariantId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "ProductId must be greater than 0.")]
        public int ProductVariantId { get; set; }

        [Required(ErrorMessage = "ProductVariant is required.")]
        public string ProductVariant { get; set; }

        [Required(ErrorMessage = "Quantity is required.")]
        [Range(1, 100, ErrorMessage = "Quantity must be between 1 and 100.")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "ProductName is required.")]
        public string ProductName { get; set; }
        [Required(ErrorMessage = "ProductPrice is required.")]
        public decimal ProductPrice { get; set; }




    }
}