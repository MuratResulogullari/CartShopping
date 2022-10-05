namespace cartshopping.webapi.Entity.ViewModels
{
    public class CartItemViewModel
    {

        public string? CartItemId { get; set; }
        public string CartId { get; set; } = null!;
        public string ProductId { get; set; } = null!;
        public ProductViewModel Product { get; set; }
        public int Quantity { get; set; }
    }
}
