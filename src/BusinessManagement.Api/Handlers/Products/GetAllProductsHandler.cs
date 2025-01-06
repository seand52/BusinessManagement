using System.Linq.Expressions;
using BusinessManagement.DAL;
using BusinessManagement.Filter;
using BusinessManagement.Helpers;
using BusinessManagement.Queries;
using BusinessManagementApi.Dto;
using BusinessManagementApi.Models;
using MediatR;

namespace BusinessManagement.Handlers;

public class GetAllProductsHandler: IRequestHandler<GetAllProductsQuery, PagedList<ProductDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllProductsHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    private Expression<Func<Product, bool>>?  BuildSearchTerm(GetAllProductsQuery request)
    {
        if (request.SearchTerm == null)
        {
            return null;
        }

        return p => p.Reference.Contains(request.SearchTerm);
    }
    public async Task<PagedList<ProductDto>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        var validFilter = new PaginationFilter(request.Filter.PageNumber, request.Filter.PageSize);
        var searchTerm = BuildSearchTerm(request);
        var products = await _unitOfWork.ProductRepository.GetAllBy(p => p.UserId == request.UserId, validFilter, searchTerm);
        var productDtos = products.Items.Select(item => item.ToDto()).ToList();
        return new PagedList<ProductDto>(productDtos, products.Page, products.PageSize, products.TotalCount);
    }
}