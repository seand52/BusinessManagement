using BusinessManagement.DAL;
using BusinessManagement.Filter;
using BusinessManagement.Helpers;
using BusinessManagementApi.Models;

namespace BusinessManagementApi.DAL
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<PagedList<Product>> GetAllBy(string userId, PaginationFilter paginationFilter, string searchTerm);
    }
}