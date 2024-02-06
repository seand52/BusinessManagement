using BusinessManagement.Filter;
using BusinessManagement.Helpers;
using BusinessManagement.Queries;
using BusinessManagementApi.DAL;
using BusinessManagementApi.Models;
using MediatR;

namespace BusinessManagement.Handlers;

public class GetAllProductsHandler: IRequestHandler<GetAllProductsQuery, PagedList<Product>>
{
    private readonly IProductRepository _productRepository;

    public GetAllProductsHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }
    public async Task<PagedList<Product>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        var validFilter = new PaginationFilter(request.Filter.PageNumber, request.Filter.PageSize);
        var products = await _productRepository.GetProducts(validFilter, request.SearchTerm, request.UserId);
        return products;
    }
}