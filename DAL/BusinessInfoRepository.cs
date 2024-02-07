using BusinessManagement.Database;
using BusinessManagement.Filter;
using BusinessManagement.Helpers;
using BusinessManagementApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BusinessManagementApi.DAL

{
    public class BusinessInfoRepository : IBusinessInfoRepository, IDisposable
    {
        private readonly ApplicationContext _context;

        public BusinessInfoRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<BusinessInfo?> GetBusinessUserByUserId(string userId)
        {
            return await _context.BusinessInfo.Where(x => x.UserId == userId).FirstOrDefaultAsync();
        }

        public async Task InsertBusinessInfo(BusinessInfo businessInfo)
        {
            await _context.BusinessInfo.AddAsync(businessInfo);
        }

        public void UpdateBusinessInfo(BusinessInfo businessInfo, BusinessInfo newBusinessInfo)
        {
            businessInfo.Name = newBusinessInfo.Name;
            businessInfo.Cif = newBusinessInfo.Cif;
            businessInfo.Address = newBusinessInfo.Address;
            businessInfo.City = newBusinessInfo.City;
            businessInfo.Country = newBusinessInfo.Country;
            businessInfo.Postcode = newBusinessInfo.Postcode;
            businessInfo.Telephone = newBusinessInfo.Telephone;
            businessInfo.Email = newBusinessInfo.Email;
            _context.BusinessInfo.Update(businessInfo);
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }

            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}