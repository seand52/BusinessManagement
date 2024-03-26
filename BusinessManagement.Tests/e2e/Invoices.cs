using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using BusinessManagement.Templates;
using BusinessManagement.Tests;
using BusinessManagementApi.Dto;
using BusinessManagementApi.Models;
using Newtonsoft.Json;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace BusinessManagement.InvoiceTests;


[TestFixture]
public class Invoices: IntegrationTestWebAppFactory
{
    private HttpClient _client;

    [OneTimeSetUp]
    public async Task Init()
    {
        var client = HttpClientSetup();
        await SeedData(client);
        await AuthenticateUser(client, "admin", "Pass_123456");
        _client = client;
    }
    
    [Test]
    public async Task CreateInvoiceSuccess()
    {
        var fixture = new Fixture();
        fixture.Customize<CreateInvoiceProductDto>(composer => composer
            .With(p => p.ProductId, fixture.Create<int>() % 10 + 1));
        var invoice = fixture.Create<CreateInvoiceDto>();
        invoice.ClientId = 1;
        var response = await _client.PostAsJsonAsync("/api/Invoices", invoice);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
    }
    
    [Test]
    public async Task RetrieveInvoiceByIdSuccess()
    {
        var response = await _client.GetAsync($"/api/Invoices/2");
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }
    
    [Test]
    public async Task RetrieveInvoicesSuccess()
    {
        var response = await _client.GetAsync("/api/Invoices?PageNumber=1&PageSize=10");
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }
    
    [Test]
    public async Task DeleteInvoiceSuccess()
    {
        var response = await _client.DeleteAsync("/api/Invoices/1");
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
    }
    
    [Test]
    public async Task UpdateInvoiceSuccess()
    {
        var fixture = new Fixture();
        fixture.Customize<CreateInvoiceProductDto>(composer => composer
            .With(p => p.ProductId, fixture.Create<int>() % 10 + 1));
        var invoice = fixture.Create<CreateInvoiceDto>();
        invoice.ClientId = 1;
        var response = await _client.PutAsJsonAsync($"/api/Invoices/4", invoice);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
    }
}