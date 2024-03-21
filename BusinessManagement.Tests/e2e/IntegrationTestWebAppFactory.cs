using BusinessManagement.Database;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;

namespace BusinessManagement.Tests;
public class IntegrationTestWebAppFactory: WebApplicationFactory<Program>
{
    private static readonly string uniqueDatabaseName = $"testdb_{Guid.NewGuid()}";
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres:latest")
        .WithDatabase(uniqueDatabaseName)
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
                var connectionString = $"Host=localhost;Port=5432;Username=root;Password=root;Database={uniqueDatabaseName};Include Error Detail=True";
                options.UseNpgsql("Host=localhost;Port=5432;Username=root;Password=root;Database=dev_business_management;Include Error Detail=True");
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
        await _dbContainer.DisposeAsync().AsTask();
    }
}