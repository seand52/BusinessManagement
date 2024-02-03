using BusinessManagement.Database;
using BusinessManagement.Filter;
using BusinessManagement.Helpers;
using BusinessManagementApi.Models;

namespace BusinessManagementApi.DAL
{
    public class ProductRepository : IProductRepository, IDisposable
    {
        private readonly ApplicationContext _context;

        public ProductRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<Product?> GetProductById(int productId)
        {
            return await _context.Products.FindAsync(productId);
        }

        public async Task<PagedList<Product>> GetProducts(PaginationFilter filter, string searchTerm, string userId)
        {
            IQueryable<Product> productsQuery = _context.Products.Where(p => p.UserId == userId);

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                productsQuery = productsQuery.Where(p => p.Reference.Contains(searchTerm));
            }

            return await PagedList<Product>.CreateAsync(productsQuery, filter.PageNumber, filter.PageSize);
        }

        public async Task InsertProduct(Product product)
        {
            await _context.Products.AddAsync(product);
        }

        public void UpdateProduct(Product product)
        {
            _context.Products.Update(product);
        }

        public void DeleteProduct(Product product)
        {
            _context.Products.Remove(product);
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