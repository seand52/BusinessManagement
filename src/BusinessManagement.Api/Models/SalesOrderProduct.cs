using System.ComponentModel.DataAnnotations;

namespace BusinessManagementApi.Models;

public class SalesOrderProduct: ICalculableItem{
    public int Id { get; set; }
    [Required]
    public int SalesOrderId { get; set; }
    public int? ProductId { get; set; }
    [Required]
    public int Quantity { get; set; }
    [Required]
    [Range(0, 1, ErrorMessage = "The Discount must be between 0 and 1.")]
    public decimal Discount { get; set; }
    [Required]
    public decimal Price { get; set; }
    
    [Required]
    public string Reference { get; set; } = null!;

    public string? Description { get; set; } = null!;
}