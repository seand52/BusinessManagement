using AutoMapper;
using BusinessManagement.Commands;
using BusinessManagementApi.DAL;
using BusinessManagementApi.Models;
using MediatR;

namespace BusinessManagement.Handlers;

public class UpdateBusinessInfoHandler: IRequestHandler<UpdateBusinessInfoRequest, bool>
{
    private readonly IBusinessInfoRepository _businessInfoRepository;
    private readonly IMapper _mapper;

    public UpdateBusinessInfoHandler (IBusinessInfoRepository businessInfoRepository, IMapper mapper)
    {
        _businessInfoRepository = businessInfoRepository;
        _mapper = mapper;
    }
    public async Task<bool> Handle(UpdateBusinessInfoRequest request, CancellationToken cancellationToken)
    {
        var businessInfoToUpdate = await _businessInfoRepository.GetBusinessUserByUserId(request.UserId);

        if (businessInfoToUpdate == null)
        {
            throw new Exception("Business Info not found");
        }

        var businessInfoEntity = _mapper.Map<BusinessInfo>(request.BusinessInfo);
        _businessInfoRepository.UpdateBusinessInfo(businessInfoToUpdate, businessInfoEntity);
        await _businessInfoRepository.Save();
        return true;
    }
}