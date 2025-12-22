namespace Domain_Layer.DTOs.OrderDTOs
{
    public class OrderItemsDTo
    {

        public string ProductName { get; set; }
        public string PictureUrl { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}