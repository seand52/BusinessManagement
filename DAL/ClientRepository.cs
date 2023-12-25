using BusinessManagement.Database;
using BusinessManagementApi.Dto;
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

        public async Task<Client?> GetClientById(int clientId)
        {
            return await _context.Clients.FindAsync(clientId);
        }

        public async Task InsertClient(Client client)
        {
            await _context.Clients.AddAsync(client);
        }

        public void UpdateClient(Client client)
        {
            _context.Clients.Update(client);
        }

        public void DeleteClient (Client client)
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
