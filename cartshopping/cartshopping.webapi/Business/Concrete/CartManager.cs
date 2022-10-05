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
                        };
                        var product = _productCollection.Find(x => x.Id == ci.ProductId && x.IsActive).FirstOrDefault();
                        
                            cartItemView.Product = new ProductViewModel
                            {
                                Id = product.Id,
                                ProductName =product.ProductName,
                                Category = product.Category,
                                Price = product.Price
                            };
                         cartItemsViewModel.Add(cartItemView);
                    }

                    cartViewModel.CartItems=cartItemsViewModel;
                    result.Result = cartViewModel;
                }
                else
                {
                     // create cart for user 1 to 1 relationship
                    Cart newCart = new Cart
                    {
                        UserId = userId,
                        IsActive = true,
                        CreatedOn = DateTime.Now
                    };
                    await _cartsCollection.InsertOneAsync(newCart);
                    result.IsSuccess = false;
                    result.Message = "Sepette ürün bulunamdı.";
                    result.Result = new CartViewModel { Id=newCart.Id,UserId=newCart.UserId};
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
        public async Task<ResultViewModel<Cart>> CreateAsync(Cart entity)
        {
            ResultViewModel<Cart> result = new ResultViewModel<Cart>();
            try
            {
                await _cartsCollection.InsertOneAsync(entity);
                result.IsSuccess = true;
                result.Result = entity;
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
        public async Task<ResultViewModel<Cart>> FindAsync(string id)
        {
            ResultViewModel<Cart> result = new ResultViewModel<Cart>();
            try
            {
                var cart = await _cartsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
                if (cart != null)
                {
                    result.IsSuccess = true;
                    result.Result = cart;
                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = "Product Bulunamadı.";
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
        public async Task<ResultViewModel> RemoveAsync(string id)
        {
            ResultViewModel result = new ResultViewModel();
            try
            {
                await _cartsCollection.DeleteOneAsync(x => x.Id == id);

                result.IsSuccess = true;
                result.Result = id;
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
        public async Task<ResultViewModel<Cart>> UpdateAsync(Cart entity)
        {
            ResultViewModel<Cart> result = new ResultViewModel<Cart>();
            try
            {
                await _cartsCollection.ReplaceOneAsync(x => x.Id == entity.Id, entity);
                result.IsSuccess = true;
                result.Result = entity;
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

    }
}
