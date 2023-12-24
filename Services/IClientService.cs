using BusinessManagementApi.Dto;
using BusinessManagementApi.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BusinessManagementApi.Services {
    public interface IClientService {
        Client? GetClientById(int clientId);
        bool CreateClient(Client client, ModelStateDictionary modelState);
        void UpdateClient(Client client, Client? newData);
        void DeleteClient(Client client);
    }
}