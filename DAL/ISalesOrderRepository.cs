using BusinessManagement.DAL;
using BusinessManagementApi.Models;

namespace BusinessManagementApi.DAL
{
    public interface ISalesOrderRepository : IGenericRepository<SalesOrder>
    {
        Task<SalesOrder?> GetBy(int salesOrderId, string userId);
        void Update(SalesOrder salesOrder, SalesOrder newData);
    }
}