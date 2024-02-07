using AutoMapper;
using BusinessManagement.Queries;
using BusinessManagementApi.DAL;
using BusinessManagementApi.Dto;
using BusinessManagementApi.Models;
using MediatR;

namespace BusinessManagement.Handlers;

public class GetBusinessInfoHandler: IRequestHandler<GetBusinessInfoQuery, BusinessInfoDto?>
{
    private readonly IBusinessInfoRepository _businessInfoRepository;
    private readonly IMapper _mapper;

    public GetBusinessInfoHandler(IBusinessInfoRepository businessInfoRepository, IMapper mapper)
    {
        _businessInfoRepository = businessInfoRepository;
        _mapper = mapper;
    }
    
    public async Task<BusinessInfoDto?> Handle(GetBusinessInfoQuery request, CancellationToken cancellationToken)
    {
        BusinessInfo? businessInfo = await _businessInfoRepository.GetBusinessUserByUserId(request.UserId);
        return _mapper.Map<BusinessInfoDto>(businessInfo);
    }
}