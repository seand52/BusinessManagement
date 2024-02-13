using BusinessManagement.Database;
using BusinessManagement.Filter;
using BusinessManagement.Helpers;
using BusinessManagementApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BusinessManagementApi.DAL

{
    public class SalesOrderRepository : ISalesOrderRepository, IDisposable
    {
        private readonly ApplicationContext _context;

        public SalesOrderRepository(ApplicationContext context)
        {
            _context = context;
        }
        
        public async Task<SalesOrder?> GetSalesOrderById(int salesOrder, string userId)
        {
            return await _context.SalesOrders.Where(p => p.UserId == userId && p.Id == salesOrder)
                .Include(p => p.Client)
                .Include(x => x.SalesOrderProducts)
                .FirstOrDefaultAsync();
        }
        
        public async Task<PagedList<SalesOrder>> GetSalesOrders(PaginationFilter filter, string searchTerm, string userId)
        {
            var query = _context.SalesOrders.Where(p => p.UserId == userId)
                .Include(p => p.Client)
                .Include(x => x.SalesOrderProducts)
                .AsQueryable();
            var res  = await PagedList<SalesOrder>.CreateAsync(query, filter.PageNumber, filter.PageSize);
            return res;
        }
        public async Task InsertSalesOrder(SalesOrder salesOrder)
        {
            await _context.SalesOrders.AddAsync(salesOrder);
        }
        
        public void UpdateSalesOrder(SalesOrder salesOrder, SalesOrder newData)
        {
            salesOrder.TotalPrice = newData.TotalPrice;
            salesOrder.Re = newData.Re;
            salesOrder.TransportPrice = newData.TransportPrice;
            salesOrder.Tax = newData.Tax;
            salesOrder.PaymentType = newData.PaymentType;
            salesOrder.Expired = newData.Expired;
            
            var newProducts = newData.SalesOrderProducts;
            foreach (var product in newProducts)
            {
                product.SalesOrderId = salesOrder.Id;
            }
            _context.SalesOrders.Update(salesOrder);
            _context.SalesOrderProduct.RemoveRange(salesOrder.SalesOrderProducts);
            _context.SalesOrderProduct.AddRange(newProducts);
        }
        
        public void DeleteSalesOrder(SalesOrder salesOrder)
        {
            _context.SalesOrders.Remove(salesOrder);
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