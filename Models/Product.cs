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

    [Required]
    public int Stock { get; set; }

    [Required]
    public int UserId { get; set; }

    [Required]
    public List<Invoice> Invoices { get; } = new();

    [Required]
    public List<SalesOrder> SalesOrders { get; } = new();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
