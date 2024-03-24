using BusinessManagement.Commands;
using BusinessManagement.DAL;
using BusinessManagementApi.DAL;
using BusinessManagementApi.Dto;
using BusinessManagementApi.Models;
using MediatR;

namespace BusinessManagement.Handlers;

public class CreateBusinessInfoHandler: IRequestHandler<CreateBusinessInfoRequest, BusinessInfoDto> 
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateBusinessInfoHandler (IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
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
        await _unitOfWork.BusinessInfoRepository.Insert(businessInfoEntity);
        await _unitOfWork.Save();
        return businessInfoEntity.ToDto();
    }
}