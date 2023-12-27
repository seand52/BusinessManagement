using BusinessManagement.Filter;
using BusinessManagement.Helpers;
using BusinessManagementApi.Models;

namespace BusinessManagementApi.DAL
{
    public interface IProductRepository : IDisposable
    {
        Task<Product?> GetProductById(int productId);
        Task<PagedList<Product>> GetProducts(PaginationFilter filter, string searchTerm);
        Task InsertProduct(Product product);
        void UpdateProduct(Product product);

        void DeleteProduct(Product product);

        Task Save();

    }
}