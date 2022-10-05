using cartshopping.webapi.Business.Abstract;
using cartshopping.webapi.Entity.Database;
using cartshopping.webapi.Entity.ViewModels;
using cartshopping.webapi.Models.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace cartshopping.webapi.Business.Concrete
{
    public class UserManager : IUserService
    {
        private readonly IMongoCollection<User> _usersCollection;
        public UserManager(IOptions<ShoppingContext> shoppingContext)
        {
            var mongoClient = new MongoClient(shoppingContext.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(shoppingContext.Value.DatabaseName);
            _usersCollection = mongoDatabase.GetCollection<User>(shoppingContext.Value.UsersCollectionName);
        }
        public async Task CreateAsync(User entity)=> 
            await _usersCollection.InsertOneAsync(entity);
        public async Task<ResultViewModel<User>> FindAsync(string id)
        {
            ResultViewModel<User> result = new ResultViewModel<User>();
            try
            {
                var user = await _usersCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
                if (user != null)
                {
                    result.IsSuccess = true;
                    result.Result = user;
                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = "User Bulunamadı.";
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

        public async Task<ResultViewModel<User>> FindAsync(string userName, string password)
        {
            ResultViewModel<User> result = new ResultViewModel<User>();
            try
            {
                var user = await _usersCollection.Find(x => x.UserName == userName && x.Password==password &&x.IsActive)
                    .FirstOrDefaultAsync();
                if (user != null)
                {
                    result.IsSuccess = true;
                    result.Result = user;
                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = "User Bulunamadı.";
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

        public async Task<ResultViewModel<List<User>>> GetAllAsync()
        {
            ResultViewModel<List<User>> result = new ResultViewModel<List<User>> ();
            try
            {
                var users = await _usersCollection.Find(x => x.IsActive).ToListAsync();
                if (users !=null)
                {
                    result.IsSuccess = true;
                    result.Result = users;
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
        public async Task RemoveAsync(string id)=>
            await _usersCollection.DeleteOneAsync(x=>x.Id == id);
        public async Task UpdateAsync(User entity) =>
            await _usersCollection.ReplaceOneAsync(x => x.Id == entity.Id, entity);

    }
}
