using cartshopping.webapi.Entity.ViewModels;
using System.Linq.Expressions;

namespace cartshopping.webapi.Business.Abstract
{
    public interface IService<T>
    {
        Task CreateAsync(T entity);
        Task RemoveAsync(string id);
        Task UpdateAsync(T entity);
        Task<ResultViewModel<List<T>>> GetAllAsync();
        Task<ResultViewModel<T>> FindAsync(string id);
    }
}
