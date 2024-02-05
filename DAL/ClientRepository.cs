using BusinessManagement.Database;
using BusinessManagement.Filter;
using BusinessManagement.Helpers;
using BusinessManagementApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BusinessManagementApi.DAL

{
    public class ClientRepository : IClientRepository, IDisposable
    {
        private readonly ApplicationContext _context;

        public ClientRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<Client?> GetClientById(int clientId, string userId)
        {
            return await _context.Clients.Where(p => p.UserId == userId && p.Id == clientId).FirstOrDefaultAsync();
        }

        public async Task<PagedList<Client>> GetClients(PaginationFilter filter, string searchTerm, string userId)
        {
            IQueryable<Client> clientsQuery = _context.Clients.Where(p => p.UserId == userId);

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                clientsQuery = clientsQuery.Where(p => p.Name.Contains(searchTerm));
            }

            return await PagedList<Client>.CreateAsync(clientsQuery, filter.PageNumber, filter.PageSize);
        }

        public async Task InsertClient(Client client)
        {
            await _context.Clients.AddAsync(client);
        }

        public void UpdateClient(Client client, Client newData)
        {
            client.Name = newData.Name;
            client.ShopName = newData.ShopName;
            client.Address = newData.Address;
            client.City = newData.City;
            client.Province = newData.Province;
            client.Postcode = newData.Postcode;
            client.DocumentNum = newData.DocumentNum;
            client.DocumentType = newData.DocumentType;
            client.Telephone = newData.Telephone;
            client.Email = newData.Email;
            _context.Clients.Update(client);
        }

        public void DeleteClient(Client client)
        {
            _context.Clients.Remove(client);
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