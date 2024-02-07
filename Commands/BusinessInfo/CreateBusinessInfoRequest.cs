using BusinessManagementApi.Dto;
using MediatR;

namespace BusinessManagement.Commands;

public class CreateBusinessInfoRequest: IRequest<BusinessInfoDto>
{
    public CreateBusinessInfoDto BusinessInfo { get; }
    public string UserId { get; }
    
    public CreateBusinessInfoRequest(CreateBusinessInfoDto businessInfo, string userId)
    {
        BusinessInfo = businessInfo;
        UserId = userId;
    }
}