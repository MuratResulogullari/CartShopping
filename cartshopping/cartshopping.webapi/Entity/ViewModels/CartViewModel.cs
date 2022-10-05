namespace cartshopping.webapi.Entity.ViewModels
{
    public class CartViewModel
    {
        public string? Id { get; set; }
        public string UserId { get; set; } = null!;
        public List<CartItemViewModel> CartItems { get; set; }
    }
}
