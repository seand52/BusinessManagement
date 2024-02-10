using AutoMapper;
using BusinessManagementApi.Models;
using BusinessManagementApi.Dto;

namespace BusinessManagementApi.Profiles
{
    public class BusinessManagementProfile : Profile
    {
        public BusinessManagementProfile()
        {
            CreateMap<Client, ClientDto>().ReverseMap();
            CreateMap<CreateClientDto, Client>().ReverseMap();
            CreateMap<UpdateClientDto, Client>();
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<CreateProductDto, Product>().ReverseMap();
            CreateMap<UpdateProductDto, Product>();
            CreateMap<BusinessInfo, BusinessInfoDto>().ReverseMap();
            CreateMap<CreateBusinessInfoDto, BusinessInfo>().ReverseMap();
            CreateMap<UpdateBusinessInfoDto, BusinessInfo>();
            CreateMap<Invoice, InvoiceDetailDto>().ReverseMap();
            CreateMap<CreateInvoiceDto, Invoice>().ReverseMap();
            CreateMap<UpdateInvoiceDto, Invoice>();
            CreateMap<CreateInvoiceProductDto, InvoiceProduct>().ReverseMap();
        }
    }
}