using System.Linq.Expressions;
using BusinessManagement.DAL;
using BusinessManagement.Filter;
using BusinessManagement.Helpers;
using BusinessManagement.Queries;
using BusinessManagementApi.DAL;
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

    private Expression<Func<Client, bool>>?  BuildSearchTerm(GetAllClientsQuery request)
    {
        if (request.SearchTerm == null)
        {
            return null;
        }

        return p => p.Name.Contains(request.SearchTerm);
    }
    public async Task<PagedList<ClientDto>> Handle(GetAllClientsQuery request, CancellationToken cancellationToken)
    {
        var validFilter = new PaginationFilter(request.Filter.PageNumber, request.Filter.PageSize);
        var searchTerm = BuildSearchTerm(request);
        var clients = await _unitOfWork.ClientRepository.GetAllBy(p => p.UserId == request.UserId, validFilter, searchTerm);
        var clientDtos = clients.Items.Select(c => c.ToDto()).ToList();
        return new PagedList<ClientDto>(clientDtos, clients.Page, clients.PageSize, clients.TotalCount);
    }
}