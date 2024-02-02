using System.ComponentModel.DataAnnotations;

namespace BusinessManagementApi.Models;

public enum DocumentType
{
    Nif,
    Nie,
    Cif
}

public class Client
{
    public int Id { get; set; }

    [StringLength(80)]
    [Required]
    public string Name { get; set; } = string.Empty;

    [StringLength(80)]
    public string ShopName { get; set; } = string.Empty;

    [StringLength(255)]
    public string Address { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string? Province { get; set; } = string.Empty;

    [StringLength(7)]
    public string? Postcode { get; set; } = string.Empty;
    public string DocumentNum { get; set; } = string.Empty;
    public DocumentType DocumentType { get; set; }

    [StringLength(12)]
    public string? Telephone { get; set; } = string.Empty;

    [EmailAddress]
    public string? Email { get; set; } = string.Empty;
    public string UserId { get; set; }
    public ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
    public ICollection<SalesOrder> SalesOrders { get; set; } = new List<SalesOrder>();

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
