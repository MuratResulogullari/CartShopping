﻿using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace cartshopping.webapi.Models.Entities
{
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public int Id { get; set; }
        [BsonElement("ProductName")]
        public string ProductName { get; set; } = null!;
        public decimal Price { get; set; }
        public string Category { get; set; } = null!;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedOn { get; set; } = DateTime.Now;
    }
}