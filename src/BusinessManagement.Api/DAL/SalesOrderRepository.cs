using BusinessManagement.Database;
using BusinessManagement.Filter;
using BusinessManagement.Helpers;
using BusinessManagementApi.Models;
using ContosoUniversity.DAL;
using Microsoft.EntityFrameworkCore;

namespace BusinessManagementApi.DAL

{
    public class SalesOrderRepository : GenericRepository<SalesOrder>, ISalesOrderRepository
    {
        private readonly ApplicationContext _context;

        public SalesOrderRepository(ApplicationContext context) : base(context)
        {
            _context = context;
        }
        
        public async Task<PagedList<SalesOrder>> GetAllBy(string userId, PaginationFilter paginationFilter, SearchParams? searchParams)
        {
            var query = _context.SalesOrders.Where(t => t.UserId == userId);
            
            if (searchParams?.ClientName != null)
            {
                query = query.Where(p => p.Client.Name.ToLower().Contains(searchParams.ClientName.ToLower()));
            }

            if (searchParams?.ClientId != null)
            {
                query = query.Where(p => p.Client.Id == searchParams.ClientId);
            }
            
            query = query.Include("Client").OrderByDescending(c => c.Id).AsNoTracking();

            return await PagedList<SalesOrder>.CreateAsync(query, paginationFilter.PageNumber, paginationFilter.PageSize);
        }

        
        public async Task<SalesOrder?> GetBy(int salesOrder, string userId)
        {
            return await _context.SalesOrders.Where(p => p.UserId == userId && p.Id == salesOrder)
                .Include(p => p.Client)
                .Include(x => x.SalesOrderProducts)
                .FirstOrDefaultAsync();
        }
        
        public void Update(SalesOrder salesOrder, SalesOrder newData)
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
    }
}