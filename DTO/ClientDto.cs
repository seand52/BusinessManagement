namespace BusinessManagementApi.Dto
{
    public class ClientDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? ShopName { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Province { get; set; }
        public string? Postcode { get; set; }
        public string? DocumentNum { get; set; }
        public string? Telephone { get; set; }
        public string? Email { get; set; }
        public int UserId { get; set; }
    }

    public class CreateClientDto
    {
        public required string Name { get; set; }
        public string? ShopName { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Province { get; set; }
        public string? Postcode { get; set; }
        public string? DocumentNum { get; set; }
        public string? Telephone { get; set; }
        public string? Email { get; set; }
        public int UserId { get; set; }
    }

    public class UpdateClientDto : CreateClientDto
    {
    };
}

