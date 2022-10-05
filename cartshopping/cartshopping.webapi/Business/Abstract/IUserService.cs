using cartshopping.webapi.Entity.ViewModels;
using cartshopping.webapi.Models.Entities;

namespace cartshopping.webapi.Business.Abstract
{
    public interface IUserService:IService<User>
    {
        Task<ResultViewModel<User>> FindAsync(LoginViewModel loginViewModel);
        ResultViewModel<User> GetByUsername(string userName);
    }
}
