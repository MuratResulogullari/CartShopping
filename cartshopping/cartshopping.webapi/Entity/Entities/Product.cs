using cartshopping.webapi.Entity.Entities;

namespace cartshopping.webapi.Models.Entities
{
    public class Product:BaseEntity
    {
        public string ProductName { get; set; } = null!;
        public decimal Price { get; set; }
        public string Category { get; set; } = null!;
    }
}
