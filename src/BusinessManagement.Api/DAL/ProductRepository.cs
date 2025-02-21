using BusinessManagement.Database;
using BusinessManagement.Filter;
using BusinessManagement.Helpers;
using BusinessManagementApi.Models;
using ContosoUniversity.DAL;
using Microsoft.EntityFrameworkCore;

namespace BusinessManagementApi.DAL

{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly ApplicationContext _context;

        public ProductRepository(ApplicationContext context) : base(context)
        {
            _context = context;
        }
        
        public async Task<PagedList<Product>> GetAllBy(string userId, PaginationFilter paginationFilter, string searchTerm)
        {
            var query = _context.Products.Where(t => t.UserId == userId);
            
            if (searchTerm != null)
            {
                query = query.Where(p => p.Reference.Contains(searchTerm));
            }

            query = query.OrderByDescending(p => p.Id).AsNoTracking();

            return await PagedList<Product>.CreateAsync(query, paginationFilter.PageNumber, paginationFilter.PageSize);
        }
    }
}