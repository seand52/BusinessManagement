using BusinessManagement.DAL;
using BusinessManagement.Filter;
using BusinessManagement.Helpers;
using BusinessManagementApi.Models;

namespace BusinessManagementApi.DAL
{
    public interface ISalesOrderRepository : IGenericRepository<SalesOrder>
    {
        Task<SalesOrder?> GetBy(int salesOrderId, string userId);
        Task<PagedList<SalesOrder>> GetAllBy(string userId, PaginationFilter paginationFilter, SearchParams? searchTerm);
        Task Insert(SalesOrder salesOrder, string userId);
        void Update(SalesOrder salesOrder, SalesOrder newData);
    }
}