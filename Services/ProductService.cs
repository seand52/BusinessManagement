using BusinessManagement.Filter;
using BusinessManagement.Helpers;
using BusinessManagementApi.Models;
using BusinessManagementApi.DAL;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BusinessManagementApi.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Product?> GetProductById(int productId)
        {
            return await _productRepository.GetProductById(productId);
        }
        
        public async Task<PagedList<Product>> GetProducts(PaginationFilter filter, string searchTerm, string userId)
        {
            return await _productRepository.GetProducts(filter, searchTerm, userId);
        }

        public async Task<bool> CreateProduct(Product product, ModelStateDictionary modelState)
        {
            if (!modelState.IsValid)
            {
                Console.WriteLine("inside model not valid");
                return false;
            }

            Console.WriteLine("inside model is valid");
            await _productRepository.InsertProduct(product);
            await _productRepository.Save();
            return true;
        }

        public async Task UpdateProduct(Product product, Product newData)
        {
            product.Reference = newData.Reference;
            product.Description = newData.Description;
            product.Price = newData.Price;
            product.Stock = newData.Stock;

            _productRepository.UpdateProduct(product);
            await _productRepository.Save();
        }

        public async Task DeleteProduct(Product product)
        {
            _productRepository.DeleteProduct(product);
            await _productRepository.Save();
        }
    }
}
