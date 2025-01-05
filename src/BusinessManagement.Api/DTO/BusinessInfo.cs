using System.ComponentModel.DataAnnotations;

namespace BusinessManagementApi.Dto
{
    public class BusinessInfoDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string Cif { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Telephone { get; set; }
        public string Postcode { get; set; }
        public string Email { get; set; }
        public string UserId { get; set; }
    }

    public class CreateBusinessInfoDto {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Cif { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        [StringLength(30)]
        public string Country { get; set; }
        [Required]
        [StringLength(12)]
        public string Telephone { get; set; }
        [Required]
        [StringLength(7)]
        public string Postcode { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }

    public class UpdateBusinessInfoDto : CreateBusinessInfoDto;
}