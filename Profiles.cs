using AutoMapper;
using BusinessManagementApi.Models;
using BusinessManagementApi.Dto;

namespace BusinessManagementApi.Profiles
{
    public class BusinessManagementProfile : Profile
    {
        public BusinessManagementProfile()
        {
            CreateMap<Client, ClientDto>().ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<CreateClientDto, Client>();
            CreateMap<Client, CreateClientDto>();
            CreateMap<UpdateClientDto, Client>();
            CreateMap<ClientDto, Client>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}