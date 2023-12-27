namespace BusinessManagementApi.Dto
{
    public class ProductDto
    {
        public int Id { get; set; }
        public required string Reference { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public int UserId { get; set; }
    }

    public class CreateProductDto
    {
        public string Reference { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public int UserId { get; set; }
    }

    public class UpdateProductDto : CreateProductDto
    {
    };
}