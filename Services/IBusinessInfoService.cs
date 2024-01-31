using BusinessManagementApi.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BusinessManagementApi.Services {
    public interface IBusinessInfoService {
        Task<BusinessInfo?> GetBusinessInfoByUserId(int userId);
        Task<bool> CreateBusinessInfo(BusinessInfo businessInfo, ModelStateDictionary modelState);
        Task UpdateBusinessInfo(BusinessInfo business, BusinessInfo? newData);
    }
}