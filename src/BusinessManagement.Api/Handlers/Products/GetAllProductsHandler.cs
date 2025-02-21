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
    
    public async Task<PagedList<ProductDto>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        var validFilter = new PaginationFilter(request.Filter.PageNumber, request.Filter.PageSize);
        var products = await _unitOfWork.ProductRepository.GetAllBy(request.UserId, validFilter, request.SearchTerm);
        var productDtos = products.Items.Select(item => item.ToDto()).ToList();
        return new PagedList<ProductDto>(productDtos, products.Page, products.PageSize, products.TotalCount);
    }
}