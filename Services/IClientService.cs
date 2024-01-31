using BusinessManagement.Filter;
using BusinessManagement.Helpers;
using BusinessManagementApi.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BusinessManagementApi.Services {
    public interface IClientService {
        Task<Client?> GetClientById(int clientId);
        Task<PagedList<Client>> GetClients(PaginationFilter filter, string searchTerm);
        Task<bool> CreateClient(Client client, ModelStateDictionary modelState);
        Task UpdateClient(Client client, Client? newData);
        Task DeleteClient(Client client);
    }
}