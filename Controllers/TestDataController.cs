using BusinessManagement.Database;
using BusinessManagementApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace BusinessManagement.Controllers;

public class TestDataController : ControllerBase
{
    private readonly ApplicationContext _context;
    private readonly IWebHostEnvironment _env;

    public TestDataController(ApplicationContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }

    [HttpGet("seed", Name = "SeedData")]
    public IActionResult SeedData()
    {
        if (_env.IsProduction())
        {
            return NotFound();
        }

        var clients = new List<Client>();
        var products = new  List<Product>();
        for (var i = 0; i < 10; i++)
        {
            clients.Add(new Client
            {
                Name = Faker.CompanyFaker.Name(),
                Address = Faker.LocationFaker.Street(),
                Email = Faker.InternetFaker.Email(),
                ShopName = Faker.CompanyFaker.Name(),
                City = Faker.LocationFaker.City(),
                Province = "Barcelona",
                Postcode = "08001",
                DocumentNum = "479725158",
                DocumentType = DocumentType.Nif,
                Telephone = Faker.PhoneFaker.Phone().Substring(0,12),
                //  TODO: fix this hardcoded value
                UserId = "a8f33f28-8dff-49eb-b779-585c02960ab9"
                
            });
        }
        for (var i = 0; i < 10; i++)
        {
            products.Add(new Product
            {
                Reference = Faker.StringFaker.AlphaNumeric(5),
                Description = Faker.TextFaker.Sentence(),
                Price = Faker.NumberFaker.Number(1, 500),
                Stock = Faker.NumberFaker.Number(1, 100),
                UserId = "a8f33f28-8dff-49eb-b779-585c02960ab9"
            });
        }
        
        var businessInfo = new BusinessInfo
        {
            Name = "My Business",
            Address = Faker.LocationFaker.Street(),
            City = Faker.LocationFaker.City(),
            Postcode = "08001",
            Country = "Spain",
            Telephone = Faker.PhoneFaker.Phone().Substring(0,12),
            Email = Faker.InternetFaker.Email(),
            Cif = "B12345678",
            UserId = "a8f33f28-8dff-49eb-b779-585c02960ab9"
        };
        
        _context.Clients.AddRange(clients);
        _context.Products.AddRange(products);
        _context.BusinessInfo.Add(businessInfo);
        _context.SaveChanges();
        return Ok(clients);
    }
}