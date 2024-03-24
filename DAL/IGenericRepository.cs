using System.Linq.Expressions;
using BusinessManagement.Filter;
using BusinessManagement.Helpers;

namespace BusinessManagement.DAL;

public interface IGenericRepository<T> where T : class
{
    Task<T?> GetBy(Expression<Func<T, bool>> filter);
    Task<PagedList<T>> GetAllBy(Expression<Func<T, bool>> filter,
        PaginationFilter paginationFilter,
        Expression<Func<T, bool>>? searchQuery, string includeProperties = "");
    Task Insert(T entity);
    void Update(T entity);
    void Delete(T entity);
}