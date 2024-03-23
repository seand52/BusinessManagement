using BusinessManagement.Commands;
using BusinessManagementApi.DAL;
using BusinessManagementApi.Dto;
using BusinessManagementApi.Models;
using MediatR;

namespace BusinessManagement.Handlers;

public class CreateBusinessInfoHandler: IRequestHandler<CreateBusinessInfoRequest, BusinessInfoDto> 
{
    private readonly IBusinessInfoRepository _businessInfoRepository;

    public CreateBusinessInfoHandler (IBusinessInfoRepository businessInfoRepository)
    {
        _businessInfoRepository = businessInfoRepository;
    }
    public async Task<BusinessInfoDto> Handle(CreateBusinessInfoRequest request, CancellationToken cancellationToken)
    {
        // TODO: This should be handled in the model validation, userId should be unique in this table
        // if (existingBusinessInfo != null)
        // {
        //     return Forbid("Only allowed to have one business associated");
        // }
        var businessInfoEntity = request.BusinessInfo.ToModel();
        businessInfoEntity.UserId = request.UserId;
        await _businessInfoRepository.InsertBusinessInfo(businessInfoEntity);
        await _businessInfoRepository.Save();
        return businessInfoEntity.ToDto();
    }
}