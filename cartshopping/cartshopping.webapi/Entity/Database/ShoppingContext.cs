namespace cartshopping.webapi.Entity.Database
{
    public class ShoppingContext
    {
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string UsersCollectionName { get; set; } = null!;
        public string ProductsCollectionName { get; set; } = null!;
        public string CartsCollectionName { get; set; } = null!;
        public string CartItemsCollectionName { get; set; } = null!;

    }
}
