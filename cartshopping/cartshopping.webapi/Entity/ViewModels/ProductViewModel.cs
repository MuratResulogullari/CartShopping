using MongoDB.Bson.Serialization.Attributes;

namespace cartshopping.webapi.Entity.ViewModels
{
    public class ProductViewModel
    {
        public string Id { get; set; }
        public string ProductName { get; set; } = null!;
        public decimal Price { get; set; }
        public string Category { get; set; } = null!;
    }
}
