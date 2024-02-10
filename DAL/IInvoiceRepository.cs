using BusinessManagement.Filter;
using BusinessManagement.Helpers;
using BusinessManagementApi.Models;

namespace BusinessManagementApi.DAL
{
    public interface IInvoiceRepository : IDisposable
    {
        Task InsertInvoice(Invoice invoice);
        Task<Invoice?> GetInvoiceById(int invoiceId, string userId);
        Task<PagedList<Invoice>> GetInvoices(PaginationFilter filter, string searchTerm, string userId);
        void UpdateInvoice(Invoice invoice, Invoice newData);
        
        void DeleteInvoice(Invoice invoice);
        
        Task Save();

    }
}