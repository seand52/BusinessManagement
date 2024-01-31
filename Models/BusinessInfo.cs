namespace BusinessManagementApi.Models;

using System.ComponentModel.DataAnnotations;

public class BusinessInfo
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    [Required]
    [StringLength(11)]
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
    [StringLength(55)]

    public string Email { get; set; }
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
