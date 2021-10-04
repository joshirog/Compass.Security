using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Compass.Security.Application.Commons.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        Task<bool> AnyAsync(Guid id);

        Task<bool> AnyAsync(Expression<Func<T, bool>> filter);

        Task<int> GetCountAsync(Guid id);

        Task<int> GetCountAsync(Expression<Func<T, bool>> filter);
        
        Task<T> GetByIdAsync(Guid id);
        
        Task<IEnumerable<T>> GetAllAsync();

        Task<T> GetFilterAsync(Expression<Func<T, bool>> filter);

        Task<IEnumerable<T>> GetSearchAsync(Expression<Func<T, bool>> filter);
        
        Task<T> GetFilterAsync(Expression<Func<T, bool>> filter, Expression<Func<T, bool>> order);

        Task<IEnumerable<T>> GetSearchAsync(Expression<Func<T, bool>> filter, Expression<Func<T, bool>> order);

        Task<T> InsertAsync(T entity);

        Task<T> AttachAsync(T entity);
        
        Task<T> UpdateAsync(T entity);
        
        Task<T> DeleteAsync(Guid id);

        Task<IEnumerable<T>> InsertBulkAsync(List<T> entities);

        Task<IEnumerable<T>> UpdateBulkAsync(List<T> entities);

        Task<IEnumerable<T>> DeleteBulkAsync(List<T> entities);
    }
}