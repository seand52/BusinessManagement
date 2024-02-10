using BusinessManagement.Database;
using BusinessManagement.Filter;
using BusinessManagement.Helpers;
using BusinessManagementApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BusinessManagementApi.DAL

{
    public class InvoiceRepository : IInvoiceRepository, IDisposable
    {
        private readonly ApplicationContext _context;

        public InvoiceRepository(ApplicationContext context)
        {
            _context = context;
        }
        
        public async Task<Invoice?> GetInvoiceById(int invoiceId, string userId)
        {
            return await _context.Invoices.Where(p => p.UserId == userId && p.Id == invoiceId)
                .Include(p => p.Client)
                .Include(x => x.InvoiceProducts)
                .FirstOrDefaultAsync();
        }
        
        public async Task<PagedList<Invoice>> GetInvoices(PaginationFilter filter, string searchTerm, string userId)
        {
            var query = _context.Invoices.Where(p => p.UserId == userId)
                .Include(p => p.Client)
                .Include(x => x.InvoiceProducts)
                .AsQueryable();
            var res  = await PagedList<Invoice>.CreateAsync(query, filter.PageNumber, filter.PageSize);
            return res;
        }
        public async Task InsertInvoice(Invoice invoice)
        {
            await _context.Invoices.AddAsync(invoice);
        }
        
        public void UpdateInvoice(Invoice invoice, Invoice newData)
        {
            invoice.TotalPrice = newData.TotalPrice;
            invoice.Re = newData.Re;
            invoice.TransportPrice = newData.TransportPrice;
            invoice.Tax = newData.Tax;
            invoice.PaymentType = newData.PaymentType;
            
            var newProducts = newData.InvoiceProducts;
            foreach (var product in newProducts)
            {
                product.InvoiceId = invoice.Id;
            }
            _context.Invoices.Update(invoice);
            _context.InvoiceProduct.RemoveRange(invoice.InvoiceProducts);
            _context.InvoiceProduct.AddRange(newProducts);
        }
        
        public void DeleteInvoice(Invoice invoice)
        {
            _context.Invoices.Remove(invoice);
        }
        
        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }

            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}