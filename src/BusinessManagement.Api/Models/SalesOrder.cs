using System.ComponentModel.DataAnnotations;

namespace BusinessManagementApi.Models
{
    public class SalesOrder: IPriceCalculable
    {
        public int Id { get; set; }

        [Required]
        public decimal TotalPrice { get; set; }

        [Required]
        [Range(0, 1, ErrorMessage = "The Re must be between 0 and 1.")]
        public decimal Re { get; set; }

        [Required]
        [Range(0, 1, ErrorMessage = "The Tax must be between 0 and 1.")]
        public decimal Tax { get; set; }

        [Required]
        public decimal TransportPrice { get; set; }

        [Required]
        public PaymentType PaymentType { get; set; }

        // [Required]
        public string UserId { get; set; }
        // public User User { get; set; } = null!;

        [Required]
        public int ClientId { get; set; }
        public Client Client { get; set; } = null!;


        [Required]
        public DateTime DateIssued { get; set; }
        public byte Expired { get; set; } = 0;

        [Required]
        public List<Product> Products { get; } = new();
        
        public int SalesOrderNumber { get; set; } = 0;
        public List<SalesOrderProduct> SalesOrderProducts { get; set; } = [];
        IEnumerable<ICalculableItem> IPriceCalculable.Items => SalesOrderProducts;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
