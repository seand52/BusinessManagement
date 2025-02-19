using BusinessManagement.DAL;
using BusinessManagement.Filter;
using BusinessManagement.Helpers;
using BusinessManagementApi.Models;

namespace BusinessManagementApi.DAL
{
    public interface IInvoiceRepository : IGenericRepository<Invoice>
    {
        Task<Invoice?> GetBy(int invoiceId, string userId);
        Task<PagedList<Invoice>> GetAllBy(string userId, PaginationFilter paginationFilter, SearchParams? searchTerm);
        
        Task Insert(Invoice invoice, string userId);
        void Update(Invoice invoice, Invoice newData);
        
    }
}