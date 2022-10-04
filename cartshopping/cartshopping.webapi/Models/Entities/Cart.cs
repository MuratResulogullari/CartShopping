using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace cartshopping.webapi.Models.Entities
{
    public class Cart
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public int Id { get; set; }
        public int UserId { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedOn { get; set; } = DateTime.Now;
    }
}
