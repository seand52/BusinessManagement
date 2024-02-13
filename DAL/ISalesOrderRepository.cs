using BusinessManagement.Filter;
using BusinessManagement.Helpers;
using BusinessManagementApi.Models;

namespace BusinessManagementApi.DAL
{
    public interface ISalesOrderRepository : IDisposable
    {
        Task InsertSalesOrder(SalesOrder salesOrder);
        Task<SalesOrder?> GetSalesOrderById(int salesOrderId, string userId);
        Task<PagedList<SalesOrder>> GetSalesOrders(PaginationFilter filter, string searchTerm, string userId);
        void UpdateSalesOrder(SalesOrder salesOrder, SalesOrder newData);
        
        void DeleteSalesOrder(SalesOrder salesOrder);
        
        Task Save();

    }
}