using BusinessManagementApi.Dto;
using BusinessManagementApi.Models;
using MediatR;

namespace BusinessManagement.Commands;

public class UpdateBusinessInfoRequest: IRequest<bool>
{
    public UpdateBusinessInfoDto BusinessInfo { get; }
    public string UserId { get; }
    
    public UpdateBusinessInfoRequest(UpdateBusinessInfoDto businessInfo, string userId)
    {
        BusinessInfo = businessInfo;
        UserId = userId;
    }
}