using System.ComponentModel.DataAnnotations;
using BusinessManagementApi.Models;

namespace BusinessManagementApi.Dto
{
    public class ClientDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string ShopName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string Postcode { get; set; }
        public string DocumentNum { get; set; }
        public string DocumentType { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }
        public string UserId { get; set; }
    }

    public class CreateClientDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string ShopName { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Province { get; set; }
        [Required]
        [StringLength(7)]
        public string Postcode { get; set; }
        [Required]
        public string DocumentNum { get; set; }
        [Required]
        [EnumDataType(typeof(DocumentType), ErrorMessage = "DocumentType must be one of the following: Nif, Nie, Cif")]  
        public string DocumentType { get; set; }
        [Required]
        public string Telephone { get; set; }
        [EmailAddress]
        public string Email { get; set; }
    }

    public class UpdateClientDto : CreateClientDto
    {
    };
}

