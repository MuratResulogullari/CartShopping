namespace cartshopping.webapi.Entity.ViewModels
{
    public class UserViewModel
    {
        public string? UserId { get; set; }  
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public CartItemViewModel Cart { get; set; }
    }
}
