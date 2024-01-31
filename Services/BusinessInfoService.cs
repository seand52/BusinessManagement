using BusinessManagementApi.Models;
using BusinessManagementApi.DAL;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BusinessManagementApi.Services
{
    public class BusinessInfoService : IBusinessInfoService
    {
        private IBusinessInfoRepository _businessInfoRepository;

        public BusinessInfoService(IBusinessInfoRepository businessInfoRepository)
        {
            _businessInfoRepository = businessInfoRepository;
        }

        public async Task<BusinessInfo?> GetBusinessInfoByUserId(int userId)
        {
            return await _businessInfoRepository.GetBusinessUserByUserId(userId);
        }

        public async Task<bool> CreateBusinessInfo(BusinessInfo businessInfo, ModelStateDictionary modelState)
        {
            if (!modelState.IsValid)
            {
                Console.WriteLine("inside model not valid");
                return false;
            }

            Console.WriteLine("inside model is valid");
            await _businessInfoRepository.InsertBusinessInfo(businessInfo);
            await _businessInfoRepository.Save();
            return true;
        }

        public async Task UpdateBusinessInfo(BusinessInfo businessInfo, BusinessInfo newData)
        {
            businessInfo.Name = newData.Name;
            businessInfo.Cif = newData.Cif;
            businessInfo.Address = newData.Address;
            businessInfo.City = newData.City;
            businessInfo.Country = newData.Country;
            businessInfo.Postcode = newData.Postcode;
            businessInfo.Telephone = newData.Telephone;
            businessInfo.Email = newData.Email;

            _businessInfoRepository.UpdateBusinessInfo(businessInfo);
            await _businessInfoRepository.Save();
        }
        
    }
}
