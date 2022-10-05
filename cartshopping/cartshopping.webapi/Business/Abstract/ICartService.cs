using cartshopping.webapi.Entity.ViewModels;
using cartshopping.webapi.Models.Entities;

namespace cartshopping.webapi.Business.Abstract
{
    public interface ICartService:IService<Cart>
    {
        Task<ResultViewModel<CartViewModel>> GetCartByUserId(string userId);
    }
}
