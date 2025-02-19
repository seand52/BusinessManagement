using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace BusinessManagementApi.Models;

public class User: IdentityUser
{
    public ICollection<Client> Clients { get; set; } = new List<Client>();
    public ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
    public ICollection<SalesOrder> SalesOrders { get; set; } = new List<SalesOrder>();
    public ICollection<Product> Products { get; set; } = new List<Product>();
    public InvoiceCount InvoiceCount { get; set; } = new InvoiceCount();
}