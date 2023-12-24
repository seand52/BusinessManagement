using BusinessManagementApi.Dto;
using BusinessManagementApi.Models;

namespace BusinessManagementApi.DAL
{
    public interface IClientRepository : IDisposable
    {
        Client? GetClientById(int clientId);
        void InsertClient(Client client);
        void UpdateClient(Client client);

        void DeleteClient(Client client);

        void Save();

    }
}