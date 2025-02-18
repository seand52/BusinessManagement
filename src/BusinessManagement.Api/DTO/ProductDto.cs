using Microsoft.Build.Framework;

namespace BusinessManagementApi.Dto
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Reference { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int? Stock { get; set; }
        public string UserId { get; set; }
    }

    public class CreateProductDto
    {
        [Required] 
        public string Reference { get; set; }
        [Required] 
        public string Description { get; set; }
        [Required] 
        public decimal Price { get; set; }
        public int? Stock { get; set; }
    }

    public class UpdateProductDto : CreateProductDto
    {
    };
}