using BusinessManagement.Database;
using BusinessManagementApi.Dto;
using BusinessManagementApi.Models;

namespace BusinessManagementApi.DAL
{
    public class ClientRepository : IClientRepository, IDisposable
    {
        private readonly ApplicationContext _context;

        public ClientRepository(ApplicationContext context)
        {
            _context = context;
        }

        public Client? GetClientById(int clientId)
        {
            return _context.Clients.Find(clientId);
        }

        public void InsertClient(Client client)
        {
            _context.Clients.Add(client);
        }

        public void UpdateClient(Client client)
        {
            _context.Clients.Update(client);
        }

        public void DeleteClient (Client client)
        {
            _context.Clients.Remove(client);
        }

        public void Save()
        {
            _context.SaveChanges();
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
