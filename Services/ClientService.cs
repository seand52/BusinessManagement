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

        public Client? GetClientById(int clientId)
        {
            return _clientRepository.GetClientById(clientId);
        }

        public bool CreateClient(Client client, ModelStateDictionary modelState)
        {
            if (!modelState.IsValid)
            {
                Console.WriteLine("inside model not valid");
                return false;
            }

            Console.WriteLine("inside model is valid");
            _clientRepository.InsertClient(client);
            _clientRepository.Save();
            return true;
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

            _clientRepository.UpdateClient(client);
            _clientRepository.Save();
        }

        public void DeleteClient(Client client)
        {
            _clientRepository.DeleteClient(client);
            _clientRepository.Save();
        }
    }
}
