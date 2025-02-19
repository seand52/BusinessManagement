using BusinessManagement.DAL;
using BusinessManagement.Filter;
using BusinessManagement.Helpers;
using BusinessManagementApi.Models;

namespace BusinessManagementApi.DAL
{
    public interface IClientRepository : IGenericRepository<Client>
    {
        Task<PagedList<Client>> GetAllBy(string userId, PaginationFilter paginationFilter,  string? searchTerm);
        
    }
}