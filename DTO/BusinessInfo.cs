namespace BusinessManagementApi.Dto
{
    public class BusinessInfoDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Cif { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public string? Telephone { get; set; }
        public string? Postcode { get; set; }
        public string? Email { get; set; }
        public int UserId { get; set; }
    }

    public class CreateBusinessInfoDto {
        public required string Name { get; set; }
        public string? Cif { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public string? Telephone { get; set; }
        public string? Postcode { get; set; }
        public string? Email { get; set; }
        public int UserId { get; set; }
    }

    public class UpdateBusinessInfoDto : CreateBusinessInfoDto;
}