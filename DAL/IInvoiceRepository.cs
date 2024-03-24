using BusinessManagement.DAL;
using BusinessManagementApi.Models;

namespace BusinessManagementApi.DAL
{
    public interface IInvoiceRepository : IGenericRepository<Invoice>
    {
        Task<Invoice?> GetBy(int invoiceId, string userId);
        void Update(Invoice invoice, Invoice newData);
        
    }
}