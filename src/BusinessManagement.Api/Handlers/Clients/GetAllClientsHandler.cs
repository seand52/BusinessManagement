using System.Linq.Expressions;
using BusinessManagement.DAL;
using BusinessManagement.Filter;
using BusinessManagement.Helpers;
using BusinessManagement.Queries;
using BusinessManagementApi.Dto;
using BusinessManagementApi.Models;
using MediatR;

namespace BusinessManagement.Handlers;

public class GetAllClientsHandler: IRequestHandler<GetAllClientsQuery, PagedList<ClientDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllClientsHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<PagedList<ClientDto>> Handle(GetAllClientsQuery request, CancellationToken cancellationToken)
    {
        var validFilter = new PaginationFilter(request.Filter.PageNumber, request.Filter.PageSize);
        var clients = await _unitOfWork.ClientRepository.GetAllBy(request.UserId, validFilter, request.SearchTerm);
        var clientDtos = clients.Items.Select(c => c.ToDto()).ToList();
        return new PagedList<ClientDto>(clientDtos, clients.Page, clients.PageSize, clients.TotalCount);
    }
}