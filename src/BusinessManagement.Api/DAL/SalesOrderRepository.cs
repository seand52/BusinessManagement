using BusinessManagement.Database;
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