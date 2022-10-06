using cartshopping.webapi.Entity.Entities;

namespace cartshopping.webapi.Models.Entities
{
    public class CartItem:BaseEntity
    {
        public string CartId { get; set; } = null!;
        public string ProductId { get; set; } = null!;
        public int Quantity { get; set; } 
    }
}
