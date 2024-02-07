using BusinessManagementApi.Models;

namespace BusinessManagementApi.DAL
{
    public interface IBusinessInfoRepository : IDisposable
    {
        Task<BusinessInfo?> GetBusinessUserByUserId(string userId);
        Task InsertBusinessInfo(BusinessInfo businessInfo);
        void UpdateBusinessInfo(BusinessInfo businessInfo, BusinessInfo newBusinessInfo);


        Task Save();

    }
}