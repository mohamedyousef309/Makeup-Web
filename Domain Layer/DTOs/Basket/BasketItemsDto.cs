using System.ComponentModel.DataAnnotations;

namespace Domain_Layer.DTOs.Basket
{
    public class BasketItemsDto
    {
        public int productid { get; set; }
        public string ProductName { get; set; }

         public int ProductVariantid{ get; set; }
        public int Quantity { get; set; }

        public IEnumerable<string> ProductVariantValues { get; set; }


        public decimal Price { get; set; }

        
        public string? VariantImageUrl { get; set; }


    }
}