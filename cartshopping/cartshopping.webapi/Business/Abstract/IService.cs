using cartshopping.webapi.Entity.ViewModels;
using System.Linq.Expressions;

namespace cartshopping.webapi.Business.Abstract
{
    public interface IService<T>
    {
        Task<ResultViewModel<T>> CreateAsync(T entity);
        Task<ResultViewModel> RemoveAsync(string id);
        Task<ResultViewModel<T>> UpdateAsync(T entity);
        Task<ResultViewModel<List<T>>> GetAllAsync();
        Task<ResultViewModel<T>> FindAsync(string id);
    }
}
