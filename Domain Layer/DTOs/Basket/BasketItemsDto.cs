using System.ComponentModel.DataAnnotations;

namespace Domain_Layer.DTOs.Basket
{
    public class BasketItemsDto
    {
        [Required]
        public int ProductId { get; set; } 

        [Required]
        public string ProductName { get; set; }

        [Required]
        [Range(0.1, double.MaxValue, ErrorMessage = "Price must be greater than zero")]
        public decimal Price { get; set; }

        [Required]
        [Range(1, 1000, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }

        [Required]
        public string PictureUrl { get; set; }


    }
}