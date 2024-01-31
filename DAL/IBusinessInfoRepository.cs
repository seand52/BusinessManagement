using BusinessManagementApi.Models;

namespace BusinessManagementApi.DAL
{
    public interface IBusinessInfoRepository : IDisposable
    {
        Task<BusinessInfo?> GetBusinessUserByUserId(int userId);
        Task InsertBusinessInfo(BusinessInfo businessInfo);
        void UpdateBusinessInfo(BusinessInfo businessInfo);


        Task Save();

    }
}