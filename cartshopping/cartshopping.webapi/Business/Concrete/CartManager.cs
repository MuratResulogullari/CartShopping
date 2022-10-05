using cartshopping.webapi.Business.Abstract;
using cartshopping.webapi.Entity.Database;
using cartshopping.webapi.Entity.ViewModels;
using cartshopping.webapi.Models.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace cartshopping.webapi.Business.Concrete
{
    public class CartManager:ICartService
    {
        private readonly IMongoCollection<Cart> _cartsCollection;
        private readonly IMongoCollection<CartItem> _cartItemsCollection;
        private readonly IMongoCollection<Product> _productCollection;
        
        public CartManager(IOptions<ShoppingContext> shoppingContext)
        {
            var mongoClient = new MongoClient(shoppingContext.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(shoppingContext.Value.DatabaseName);
            _cartsCollection = mongoDatabase.GetCollection<Cart>(shoppingContext.Value.CartsCollectionName);
            _cartItemsCollection = mongoDatabase.GetCollection<CartItem>(shoppingContext.Value.CartItemsCollectionName);
            _productCollection = mongoDatabase.GetCollection<Product>(shoppingContext.Value.ProductsCollectionName);
        }
        public async Task CreateAsync(Cart entity) =>
            await _cartsCollection.InsertOneAsync(entity);
        public async Task<ResultViewModel<Cart>> FindAsync(string id)
        {
            ResultViewModel<Cart> result = new ResultViewModel<Cart>();
            try
            {
                var cart = await _cartsCollection.Find(x => x.Id == id && x.IsActive).FirstOrDefaultAsync();
                if (cart != null)
                {
                    result.IsSuccess = true;
                    result.Result = cart;
                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = "Cart Bulunamadı.";
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

        public async Task<ResultViewModel<List<Cart>>> GetAllAsync()
        {
            ResultViewModel<List<Cart>> result = new ResultViewModel<List<Cart>>();
            try
            {
                var carts = await _cartsCollection.Find(x => x.IsActive).ToListAsync();
                if (carts != null)
                {
                    result.IsSuccess = true;
                    result.Result = carts;
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

        public async Task<ResultViewModel<CartViewModel>> GetCartByUserId(string userId)
        {

            ResultViewModel<CartViewModel> result = new ResultViewModel<CartViewModel>();
            CartViewModel cartViewModel = new CartViewModel();
            List<CartItemViewModel> cartItemsViewModel = new List<CartItemViewModel>();
            ProductViewModel productViewModel = new ProductViewModel();

            try
            {
                var cart = await _cartsCollection.Find(x => x.UserId == userId && x.IsActive).FirstOrDefaultAsync();
                if (cart != null)
                {
                    result.IsSuccess=  true;
                    cartViewModel.Id = cart.Id;
                    cartViewModel.UserId=cart.UserId;
                   
                    var cartItems =await _cartItemsCollection.Find(x => x.CartId== cart.Id && x.IsActive).ToListAsync();
                               
                    foreach (var ci in cartItems)
                    {
                        var cartItemView = new CartItemViewModel
                        {
                            CartItemId = ci.Id,
                            Quantity = ci.Quantity,
                            ProductId = ci.ProductId,
                            Product = (ProductViewModel)_productCollection.Find(x => x.Id == ci.ProductId && x.IsActive)

                        };
                        cartItemsViewModel.Add(cartItemView);
                    }

                    cartViewModel.CartItems=cartItemsViewModel;
                    result.Result = cartViewModel;
                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = "Cart Bulunamadı.";
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
            await _cartsCollection.DeleteOneAsync(x => x.Id == id);
        public async Task UpdateAsync(Cart entity) =>
            await _cartsCollection.ReplaceOneAsync(x => x.Id == entity.Id, entity);

    }
}
