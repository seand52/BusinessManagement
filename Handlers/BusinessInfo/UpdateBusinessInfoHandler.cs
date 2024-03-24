using BusinessManagement.Commands;
using BusinessManagement.DAL;
using BusinessManagementApi.DAL;
using BusinessManagementApi.Models;
using MediatR;

namespace BusinessManagement.Handlers;

public class UpdateBusinessInfoHandler: IRequestHandler<UpdateBusinessInfoRequest, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateBusinessInfoHandler (IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<bool> Handle(UpdateBusinessInfoRequest request, CancellationToken cancellationToken)
    {
        var businessInfoToUpdate = await _unitOfWork.BusinessInfoRepository.GetBy(b => b.UserId == request.UserId);

        if (businessInfoToUpdate == null)
        {
            throw new Exception("Business Info not found");
        }

        var businessInfoEntity = request.BusinessInfo.ToModel();
        businessInfoEntity.Id = businessInfoToUpdate.Id;
        businessInfoEntity.UserId = businessInfoToUpdate.UserId;
        _unitOfWork.BusinessInfoRepository.Update(businessInfoEntity);
        await _unitOfWork.Save();
        return true;
    }
}