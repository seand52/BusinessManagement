using System.ComponentModel.DataAnnotations;

namespace BusinessManagementApi.Models;

public enum PaymentType
{
    CASH,
    CARD,
    TRANSFER
}

public class Invoice
{
    public int Id { get; set; }

    [Required]
    public double TotalPrice { get; set; }

    [Required]
    public double Re { get; set; }

    [Required]
    public double Tax { get; set; }

    [Required]
    public double TransportPrice { get; set; } = 0;

    [Required]
    public PaymentType PaymentType { get; set; }

    [Required]
    public int UserId { get; set; }

    public User User { get; set; } = null!;

    [Required]
    public int ClientId { get; set; }
    public Client Client { get; set; } = null!;

    [Required]
    public List<Product> Products { get; } = new();
    public DateTime Date { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
