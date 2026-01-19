namespace Domain_Layer.Entites.Basket
{
    public class CartItem
    {
        public int ProductId { get; set; }
        public int ProductVariantid { get; set; }

        public string ProductVariant { get; set; }
        public string UserCartId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        public string? PictureUrl { get; set; }
    }
}