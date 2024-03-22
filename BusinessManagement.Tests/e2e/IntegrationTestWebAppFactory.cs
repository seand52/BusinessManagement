using System.Net.Http.Headers;
using System.Net.Http.Json;
using BusinessManagement.Database;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
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
                var connectionString = $"Host=localhost;Port=5432;Username=root;Password=root;Database={uniqueDatabaseName};Include Error Detail=True";
                options.UseNpgsql("Host=localhost;Port=5432;Username=root;Password=root;Database=dev_business_management;Include Error Detail=True");
            });
        });
    }

    public HttpClient HttpClientSetup()
    {
        var client  = base.CreateClient();
        client.BaseAddress = new Uri("http://localhost:5206");
        return client;
    }
    
    public async Task SeedData(HttpClient client)
    {
        await client.GetAsync("/seed");
    }
    
    public async Task AuthenticateUser(HttpClient client, string email, string password)
    {
        var response = await client.PostAsJsonAsync("/login", new { email, password});
        var token = await response.Content.ReadAsStringAsync();
        dynamic jsonResponse = JsonConvert.DeserializeObject(token);
        token = jsonResponse.accessToken;
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
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