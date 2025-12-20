using System.ComponentModel.DataAnnotations;

namespace Domain_Layer.DTOs.Basket
{
    public class BasketItemsDto
    {

        public string ProductName { get; set; }

      
        public int Quantity { get; set; }

        
        public decimal Price { get; set; }

        
        public string PictureUrl { get; set; }


    }
}