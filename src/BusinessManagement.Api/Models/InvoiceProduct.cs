using System.ComponentModel.DataAnnotations;

namespace BusinessManagementApi.Models;

public class InvoiceProduct: ICalculableItem {
    public int Id { get; set; }
    public int InvoiceId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    
    [Range(0, 1, ErrorMessage = "The Discount must be between 0 and 1.")]
    public decimal Discount { get; set; }
    public decimal Price { get; set; }
    public string Reference { get; set; }
    public string Description { get; set; }
}