using BusinessManagement.Database;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.Web.CodeGeneration.Design;
using Testcontainers.PostgreSql;

namespace BusinessManagement.Tests;
public class IntegrationTestWebAppFactory: WebApplicationFactory<Program>
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres:latest")
        .WithDatabase("dev_business_management")
        .WithUsername("root")
        .WithPassword("root")
        .WithPortBinding(5432)
        .Build();
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            var descriptor =
                services.SingleOrDefault(s => s.ServiceType == typeof(DbContextOptions<ApplicationContext>));
            if (descriptor is not null)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<ApplicationContext>(options =>
            {
                // var connectionString = _dbContainer.GetConnectionString();
                options.UseNpgsql("Host=localhost;Port=5432;Username=root;Password=root;Database=dev_business_management");
            });
        });
    }
    
    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        await _dbContainer.StartAsync();
        using var scope = Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
        await db.Database.MigrateAsync();
    }
    
    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        await _dbContainer.StopAsync();
    }
}