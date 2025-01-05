using BusinessManagement.Database;
using BusinessManagementApi.Dto;
using BusinessManagementApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BusinessManagement.Controllers;

public class TestDataController : ControllerBase
{
    private readonly ApplicationContext _context;
    private readonly IWebHostEnvironment _env;
    private readonly UserManager<User> _userManager;

    public TestDataController(ApplicationContext context, IWebHostEnvironment env, UserManager<User> userManager)
    {
        _context = context;
        _env = env;
        _userManager = userManager;
    }

    [HttpGet("seed", Name = "SeedData")]
    public async Task<ActionResult> SeedData()
    {
        if (_env.IsProduction())
        {
            return NotFound();
        }
        var user1 = new User() { UserName = "admin", Email = "admin@localhost.com" };
        var user2 = new User() { UserName = "admin2", Email = "admin2@localhost.com" };
        await _userManager.CreateAsync(user1, "Pass_123456");
        await _userManager.CreateAsync(user2, "Pass_123456");
        var clients = new List<Client>();
        var products = new  List<Product>();
        var businessInfos  = new List<BusinessInfo>();
        var invoices = new List<Invoice>();
        var salesOrders = new List<SalesOrder>();
        
        User[] users = { user1, user2 };


        int invoiceId = 0;
        int salesOrderId = 0;
        foreach (var user in users)
        {
            for (var j = 0; j < 10; j++)
            {
                clients.Add(new Client
                {
                    Name = Faker.LocationFaker.Street(),
                    Address = Faker.LocationFaker.Street(),
                    Email = Faker.InternetFaker.Email(),
                    ShopName = Faker.CompanyFaker.Name(),
                    City = Faker.LocationFaker.City(),
                    Province = "Barcelona",
                    Postcode = "08001",
                    DocumentNum = "479725158",
                    DocumentType = DocumentType.Nif,
                    Telephone = Faker.PhoneFaker.Phone().Substring(0,12  ),
                    UserId = user.Id
                
                });
            }   
            
            for (var j = 0; j < 10; j++)
            {
                products.Add(new Product
                {
                    Reference = Faker.StringFaker.AlphaNumeric(5),
                    Description = Faker.TextFaker.Sentence(),
                    Price = Faker.NumberFaker.Number(1, 500),
                    Stock = Faker.NumberFaker.Number(1, 100),
                    UserId = user.Id
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
                UserId = user.Id
            };
            businessInfos.Add(businessInfo);
            
            for (var j = 0; j < 10; j++)
            {
                invoiceId += 1;
                var clientId = Faker.NumberFaker.Number(1, clients.Count - 1);
                var invoice = new Invoice
                {
                    ClientId = clientId,
                    UserId = user.Id,
                    Re = 0.05m,
                    Tax = 0.21m,
                    TransportPrice = 0,
                    DateIssued = DateTime.UtcNow,
                    PaymentType = PaymentType.CASH,
                };
                invoice.InvoiceProducts.Add(new InvoiceProduct
                {
                    ProductId = Faker.NumberFaker.Number(1, products.Count - 1),
                    InvoiceId = invoiceId,
                    Discount = 0,
                    Price = Faker.NumberFaker.Number(1, 500),
                    Quantity = Faker.NumberFaker.Number(1, 10),
                    Reference = Faker.StringFaker.AlphaNumeric(5),
                    Description = Faker.TextFaker.Sentence()
                });
                invoice.InvoiceProducts.Add(new InvoiceProduct
                {
                    ProductId = Faker.NumberFaker.Number(1, products.Count - 1),
                    InvoiceId = invoiceId,
                    Discount = 0,
                    Price = Faker.NumberFaker.Number(1, 500),
                    Quantity = Faker.NumberFaker.Number(1, 10),
                    Reference = Faker.StringFaker.AlphaNumeric(5),
                    Description = Faker.TextFaker.Sentence(),
                });
                invoice.TotalPrice = invoice.CalculateTotalPrice();
                invoices.Add(invoice);
            }
            
            for (var j = 0; j < 10; j++)
            {
                salesOrderId += 1;
                var clientId = Faker.NumberFaker.Number(1, clients.Count - 1);
                var salesOrder = new SalesOrder()
                {
                    ClientId = clientId,
                    UserId = user.Id,
                    Re = 0.05m,
                    Tax = 0.21m,
                    TransportPrice = 0,
                    DateIssued = DateTime.UtcNow,
                    PaymentType = PaymentType.CASH,
                };
                salesOrder.SalesOrderProducts.Add(new SalesOrderProduct()
                {
                    ProductId = Faker.NumberFaker.Number(1, products.Count - 1),
                    SalesOrderId = salesOrderId,
                    Discount = 0,
                    Price = Faker.NumberFaker.Number(1, 500),
                    Quantity = Faker.NumberFaker.Number(1, 10),
                    Reference = Faker.StringFaker.AlphaNumeric(5),
                    Description = Faker.TextFaker.Sentence()
                });
                salesOrder.SalesOrderProducts.Add(new SalesOrderProduct()
                {
                    ProductId = Faker.NumberFaker.Number(1, products.Count - 1),
                    SalesOrderId = salesOrderId,
                    Discount = 0,
                    Price = Faker.NumberFaker.Number(1, 500),
                    Quantity = Faker.NumberFaker.Number(1, 10),
                    Reference = Faker.StringFaker.AlphaNumeric(5),
                    Description = Faker.TextFaker.Sentence()
                });
                salesOrder.TotalPrice = salesOrder.CalculateTotalPrice();
                salesOrders.Add(salesOrder);
            }
        }

        
        
        _context.Clients.AddRange(clients);
        _context.Products.AddRange(products);
        _context.BusinessInfo.AddRange(businessInfos);
        _context.Invoices.AddRange(invoices);
        _context.SalesOrders.AddRange(salesOrders);
        _context.SaveChanges();
        return Ok();
    }
}
