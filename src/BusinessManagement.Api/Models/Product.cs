using System.ComponentModel.DataAnnotations;

namespace BusinessManagementApi.Models;

public class Product
{
    public int Id { get; set; }

    [Required] 
    public string Reference { get; set; }

    [Required]
    public string Description { get; set; }

    [Required]
    public decimal Price { get; set; }

    public int? Stock { get; set; }

    // [Required]
    public string UserId { get; set; }

    public List<Invoice> Invoices { get; } = [];
    public List<InvoiceProduct> InvoiceProducts { get; } = [];

    [Required]
    public List<SalesOrder> SalesOrders { get; } = new();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
