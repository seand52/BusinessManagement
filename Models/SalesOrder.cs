using System.ComponentModel.DataAnnotations;

namespace BusinessManagementApi.Models
{
    public class SalesOrder
    {
        public int Id { get; set; }

        [Required]
        public double TotalPrice { get; set; }

        [Required]
        public double Re { get; set; }

        [Required]
        public double Tax { get; set; }

        [Required]
        public double TransportPrice { get; set; }

        [Required]
        public PaymentType PaymentType { get; set; }

        // [Required]
        public string UserId { get; set; }
        // public User User { get; set; } = null!;

        [Required]
        public int ClientId { get; set; }
        public Client Client { get; set; } = null!;


        [Required]
        public DateTime Date { get; set; }
        public byte Expired { get; set; } = 0;

        [Required]
        public List<Product> Products { get; } = new();
        public List<SalesOrderProduct> SalesOrderProducts { get; } = [];
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
