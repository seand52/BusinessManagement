using BusinessManagement.Filter;
using BusinessManagement.Helpers;
using BusinessManagementApi.Models;

namespace BusinessManagementApi.DAL
{
    public interface IClientRepository : IDisposable
    {
        Task<Client?> GetClientById(int clientId, string userId);
        Task<PagedList<Client>> GetClients(PaginationFilter filter, string searchTerm, string userId);
        Task InsertClient(Client client);
        void UpdateClient(Client client, Client newData);

        void DeleteClient(Client client);

        Task Save();

    }
}