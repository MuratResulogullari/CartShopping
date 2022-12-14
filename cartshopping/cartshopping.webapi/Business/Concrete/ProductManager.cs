using cartshopping.webapi.Business.Abstract;
using cartshopping.webapi.Entity.Database;
using cartshopping.webapi.Entity.ViewModels;
using cartshopping.webapi.Models.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace cartshopping.webapi.Business.Concrete
{
    public class ProductManager:IProductService
    {
        private readonly IMongoCollection<Product> _productsCollection;
        public ProductManager(IOptions<ShoppingContext> shoppingContext)
        {
            var mongoClient = new MongoClient(shoppingContext.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(shoppingContext.Value.DatabaseName);
            _productsCollection = mongoDatabase.GetCollection<Product>(shoppingContext.Value.ProductsCollectionName);
        }
        public async Task<ResultViewModel<Product>> CreateAsync(Product entity)
        {
            ResultViewModel<Product> result = new ResultViewModel<Product>();
            try
            {
                await _productsCollection.InsertOneAsync(entity);
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
        public async Task<ResultViewModel<Product>> FindAsync(string id)
        {
            ResultViewModel<Product> result = new ResultViewModel<Product>();
            try
            {
                var product = await _productsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
                if (product != null)
                {
                    result.IsSuccess = true;
                    result.Result = product;
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
        public async Task<ResultViewModel<List<Product>>> GetAllAsync()
        {
            ResultViewModel<List<Product>> result = new ResultViewModel<List<Product>>();
            try
            {
                var products = await _productsCollection.Find(x => x.IsActive).ToListAsync();
                if (products != null)
                {
                    result.IsSuccess = true;
                    result.Result = products;
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
                await _productsCollection.DeleteOneAsync(x => x.Id == id);

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
        public async Task<ResultViewModel<Product>> UpdateAsync(Product entity)
        {
            ResultViewModel<Product> result = new ResultViewModel<Product>();
            try
            {
                await _productsCollection.ReplaceOneAsync(x => x.Id == entity.Id, entity);
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
