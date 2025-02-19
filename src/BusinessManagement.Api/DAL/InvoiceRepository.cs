using BusinessManagement.Database;
using BusinessManagement.Filter;
using BusinessManagement.Helpers;
using BusinessManagementApi.Models;
using ContosoUniversity.DAL;
using Microsoft.EntityFrameworkCore;

namespace BusinessManagementApi.DAL

{
    public class InvoiceRepository : GenericRepository<Invoice>, IInvoiceRepository
    {
        private readonly ApplicationContext _context;

        public InvoiceRepository(ApplicationContext context) : base(context)
        {
            _context = context;
        }
        
        public async Task<PagedList<Invoice>> GetAllBy(string userId, PaginationFilter paginationFilter, SearchParams? searchParams)
        {
            var query = _context.Invoices.Where(t => t.UserId == userId);
            
            if (searchParams?.ClientName != null)
            {
                query = query.Where(p => p.Client.Name.ToLower().Contains(searchParams.ClientName.ToLower()));
            }

            if (searchParams?.ClientId != null)
            {
                query = query.Where(p => p.Client.Id == searchParams.ClientId);
            }
            
            query = query.Include("Client").OrderByDescending(c => c.Id).AsNoTracking();

            return await PagedList<Invoice>.CreateAsync(query, paginationFilter.PageNumber, paginationFilter.PageSize);
        }

        public async Task<Invoice?> GetBy(int invoiceId, string userId)
        {
            return await _context.Invoices.Where(p => p.UserId == userId && p.Id == invoiceId)
                .Include(p => p.Client)
                .Include(x => x.InvoiceProducts)
                .FirstOrDefaultAsync();
        }

        public void Update(Invoice invoice, Invoice newData)
        {
            invoice.TotalPrice = newData.TotalPrice;
            invoice.Re = newData.Re;
            invoice.TransportPrice = newData.TransportPrice;
            invoice.Tax = newData.Tax;
            invoice.PaymentType = newData.PaymentType;
            invoice.DateIssued = newData.DateIssued;

            var newProducts = newData.InvoiceProducts;
            foreach (var product in newProducts)
            {
                product.InvoiceId = invoice.Id;
            }

            _context.Invoices.Update(invoice);
            _context.InvoiceProduct.RemoveRange(invoice.InvoiceProducts);
            _context.InvoiceProduct.AddRange(newProducts);
        }
    }
}