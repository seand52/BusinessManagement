using BusinessManagementApi.Dto;
using BusinessManagementApi.Models;

namespace BusinessManagementApi.DAL
{
    public interface IClientRepository : IDisposable
    {
        Task<Client?> GetClientById(int clientId);
        Task InsertClient(Client client);
        void UpdateClient(Client client);

        void DeleteClient(Client client);

        Task Save();

    }
}