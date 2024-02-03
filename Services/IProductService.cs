using BusinessManagement.Filter;
using BusinessManagement.Helpers;
using BusinessManagementApi.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BusinessManagementApi.Services {
    public interface IProductService {
        Task<Product?> GetProductById(int productId);
        Task<PagedList<Product>> GetProducts(PaginationFilter filter, string searchTerm, string userId);
        Task<bool> CreateProduct(Product product, ModelStateDictionary modelState);
        Task UpdateProduct(Product product, Product? newData);
        Task DeleteProduct(Product product);
    }
}