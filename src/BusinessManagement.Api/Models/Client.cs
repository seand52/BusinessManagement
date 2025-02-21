using System.ComponentModel.DataAnnotations;

namespace BusinessManagementApi.Models;

public enum DocumentType
{
    Nif,
    Cif,
    Intra,
    Passport
}

public class Client: BaseEntity
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }
    public string? ShopName { get; set; }
    [Required]
    public string Address { get; set; }
    [Required]
    public string City { get; set; } 
    public string? Province { get; set; }
    
    public string? Country { get; set; }
    
    [Required]
    [StringLength(7)]
    public string Postcode { get; set; }

    public string? DocumentNum { get; set; }

    [EnumDataType(typeof(DocumentType), ErrorMessage = "DocumentType must be one of the following: Nif, Nie, Cif")]
    public DocumentType? DocumentType { get; set; }

    [StringLength(12)]
    public string? Telephone1 { get; set; } = string.Empty;
    
    [StringLength(12)]
    public string? Telephone2 { get; set; } = string.Empty;

    [EmailAddress]
    public string? Email { get; set; } = string.Empty;
    
    public string UserId { get; set; }
}
