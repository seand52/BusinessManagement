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

    [Required]
    public string Name { get; set; }
    [Required]
    public string ShopName { get; set; }
    [Required]
    public string Address { get; set; }
    [Required]
    public string City { get; set; } 
    [Required]
    public string? Province { get; set; }
    [Required]
    [StringLength(7)]
    public string Postcode { get; set; }
    
    [Required]
    public string DocumentNum { get; set; }
    
    [Required]
    [EnumDataType(typeof(DocumentType), ErrorMessage = "DocumentType must be one of the following: Nif, Nie, Cif")]  
    public DocumentType DocumentType { get; set; }

    [Required]
    [StringLength(12)]
    public string Telephone { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    
    public string UserId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
