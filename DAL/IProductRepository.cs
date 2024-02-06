using BusinessManagement.Filter;
using BusinessManagement.Helpers;
using BusinessManagementApi.Models;

namespace BusinessManagementApi.DAL
{
    public interface IProductRepository : IDisposable
    {
        Task<Product?> GetProductById(int productId, string userId);
        Task<PagedList<Product>> GetProducts(PaginationFilter filter, string searchTerm, string userId);
        Task InsertProduct(Product product);
        void UpdateProduct(Product product, Product newData);

        void DeleteProduct(Product product);

        Task Save();

    }
}