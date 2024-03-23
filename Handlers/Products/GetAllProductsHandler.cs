using BusinessManagement.Filter;
using BusinessManagement.Helpers;
using BusinessManagement.Queries;
using BusinessManagementApi.DAL;
using BusinessManagementApi.Dto;
using BusinessManagementApi.Models;
using MediatR;

namespace BusinessManagement.Handlers;

public class GetAllProductsHandler: IRequestHandler<GetAllProductsQuery, PagedList<ProductDto>>
{
    private readonly IProductRepository _productRepository;

    public GetAllProductsHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }
    public async Task<PagedList<ProductDto>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        var validFilter = new PaginationFilter(request.Filter.PageNumber, request.Filter.PageSize);
        var products = await _productRepository.GetProducts(validFilter, request.SearchTerm, request.UserId);
        var productDtos = products.Items.Select(item => item.ToDto()).ToList();
        return new PagedList<ProductDto>(productDtos, products.TotalCount, products.Page, products.PageSize);
    }
}