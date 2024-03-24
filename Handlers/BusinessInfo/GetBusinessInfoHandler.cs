using BusinessManagement.DAL;
using BusinessManagement.Queries;
using BusinessManagementApi.DAL;
using BusinessManagementApi.Dto;
using BusinessManagementApi.Models;
using MediatR;

namespace BusinessManagement.Handlers;

public class GetBusinessInfoHandler: IRequestHandler<GetBusinessInfoQuery, BusinessInfoDto?>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetBusinessInfoHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<BusinessInfoDto?> Handle(GetBusinessInfoQuery request, CancellationToken cancellationToken)
    {
        BusinessInfo? businessInfo = await _unitOfWork.BusinessInfoRepository.GetBy(b => b.UserId == request.UserId);
        return businessInfo.ToDto();
    }
}