using cartshopping.webapi.Business.Abstract;
using cartshopping.webapi.Entity.Database;
using cartshopping.webapi.Entity.ViewModels;
using cartshopping.webapi.Models.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace cartshopping.webapi.Business.Concrete
{
    public class CartItemManager:ICartItemService
    {
        private readonly IMongoCollection<CartItem> _cartItemsCollection;
        public CartItemManager(IOptions<ShoppingContext> shoppingContext)
        {
            var mongoClient = new MongoClient(shoppingContext.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(shoppingContext.Value.DatabaseName);
            _cartItemsCollection = mongoDatabase.GetCollection<CartItem>(shoppingContext.Value.CartItemsCollectionName);
        }
        public async Task CreateAsync(CartItem entity) =>
            await _cartItemsCollection.InsertOneAsync(entity);
        public async Task<ResultViewModel<CartItem>> FindAsync(string id)
        {
            ResultViewModel<CartItem> result = new ResultViewModel<CartItem>();
            try
            {
                var cartItem = await _cartItemsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
                if (cartItem != null)
                {
                    result.IsSuccess = true;
                    result.Result = cartItem;
                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = "CartItem Bulunamadı.";
                }
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine(ex.Message);
                result.IsSuccess = false;
                result.Message = "Server Error!";
                return result;
            }
        }

        public async Task<ResultViewModel<List<CartItem>>> GetAllAsync()
        {
            ResultViewModel<List<CartItem>> result = new ResultViewModel<List<CartItem>>();
            try
            {
                var cartItems = await _cartItemsCollection.Find(x => x.IsActive).ToListAsync();
                if (cartItems != null)
                {
                    result.IsSuccess = true;
                    result.Result = cartItems;
                }
                else
                {
                    result.IsSuccess = false;
                }
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine(ex.Message);
                result.IsSuccess = false;
                result.Message = "Server Error!";
                return result;
            }
        }
        public async Task RemoveAsync(string id) =>
            await _cartItemsCollection.DeleteOneAsync(x => x.Id == id);
        public async Task UpdateAsync(CartItem entity) =>
            await _cartItemsCollection.ReplaceOneAsync(x => x.Id == entity.Id, entity);

    }
}
