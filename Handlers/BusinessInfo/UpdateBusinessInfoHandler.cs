using BusinessManagement.Commands;
using BusinessManagementApi.DAL;
using BusinessManagementApi.Models;
using MediatR;

namespace BusinessManagement.Handlers;

public class UpdateBusinessInfoHandler: IRequestHandler<UpdateBusinessInfoRequest, bool>
{
    private readonly IBusinessInfoRepository _businessInfoRepository;

    public UpdateBusinessInfoHandler (IBusinessInfoRepository businessInfoRepository)
    {
        _businessInfoRepository = businessInfoRepository;
    }
    public async Task<bool> Handle(UpdateBusinessInfoRequest request, CancellationToken cancellationToken)
    {
        var businessInfoToUpdate = await _businessInfoRepository.GetBusinessUserByUserId(request.UserId);

        if (businessInfoToUpdate == null)
        {
            throw new Exception("Business Info not found");
        }

        var businessInfoEntity = request.BusinessInfo.ToModel();
        _businessInfoRepository.UpdateBusinessInfo(businessInfoToUpdate, businessInfoEntity);
        await _businessInfoRepository.Save();
        return true;
    }
}