using BusinessManagementApi.Models;
using BusinessManagementApi.DAL;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BusinessManagementApi.Services
{
    public class ClientService : IClientService
    {
        private IClientRepository _clientRepository;

        public ClientService(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public async Task<Client?> GetClientById(int clientId)
        {
            return await _clientRepository.GetClientById(clientId);
        }

        public async Task<bool> CreateClient(Client client, ModelStateDictionary modelState)
        {
            if (!modelState.IsValid)
            {
                Console.WriteLine("inside model not valid");
                return false;
            }

            Console.WriteLine("inside model is valid");
            await _clientRepository.InsertClient(client);
            await _clientRepository.Save();
            return true;
        }

        public async Task UpdateClient(Client client, Client newData)
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

            _clientRepository.UpdateClient(client);
            await _clientRepository.Save();
        }

        public async Task DeleteClient(Client client)
        {
            _clientRepository.DeleteClient(client);
            await _clientRepository.Save();
        }
    }
}
