using BusinessManagementApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BusinessManagement.Database;

public class ApplicationContext : DbContext
{
    public DbSet<Client> Clients { get; set; }
    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<SalesOrder> SalesOrders { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<BusinessInfo> BusinessInfo { get; set; }

    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<Invoice>()
            .HasMany(e => e.Products)
            .WithMany(e => e.Invoices)
            .UsingEntity<InvoiceProduct>();

        modelBuilder
            .Entity<SalesOrder>()
            .HasMany(e => e.Products)
            .WithMany(e => e.SalesOrders)
            .UsingEntity<SalesOrderProduct>();
    }
}
