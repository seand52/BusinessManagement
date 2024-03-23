using BusinessManagement.Queries;
using BusinessManagementApi.DAL;
using BusinessManagementApi.Dto;
using BusinessManagementApi.Models;
using MediatR;

namespace BusinessManagement.Handlers;

public class GetBusinessInfoHandler: IRequestHandler<GetBusinessInfoQuery, BusinessInfoDto?>
{
    private readonly IBusinessInfoRepository _businessInfoRepository;

    public GetBusinessInfoHandler(IBusinessInfoRepository businessInfoRepository)
    {
        _businessInfoRepository = businessInfoRepository;
    }
    
    public async Task<BusinessInfoDto?> Handle(GetBusinessInfoQuery request, CancellationToken cancellationToken)
    {
        BusinessInfo? businessInfo = await _businessInfoRepository.GetBusinessUserByUserId(request.UserId);
        return businessInfo.ToDto();
    }
}