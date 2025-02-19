using System.ComponentModel.DataAnnotations;


namespace BusinessManagementApi.Models;

public class InvoiceCount
{
    [Required]
    public int Id { get; set; }
    
    [Required]
    public string UserId { get; set; }
    
    [Required]
    public int count { get; set; }
    
    public User User { get; set; }
}