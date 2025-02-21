namespace BusinessManagementApi.Models;

using System.ComponentModel.DataAnnotations;

public class BusinessInfo: BaseEntity
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string Cif { get; set; }

    [Required]
    public string Address { get; set; }

    [Required]
    [StringLength(7)]
    public string Postcode { get; set; }

    [Required]
    public string City { get; set; }

    [Required]
    [StringLength(30)]
    public string Country { get; set; }

    [Required]
    [StringLength(12)]
    public string Telephone { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }
    public string UserId { get; set; }
}
