using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using BusinessManagement.Tests;
using BusinessManagementApi.Dto;
using BusinessManagementApi.Models;
using Newtonsoft.Json;

namespace BusinessManagement.InvoiceTests;

[TestFixture]
public class Invoices: IntegrationTestWebAppFactory
{
    private HttpClient _client;
    
    private static HttpClient CreateHttpClient()
    {
        var factory = new IntegrationTestWebAppFactory();
        var client = factory.CreateClient();
        return client;
    }

    [OneTimeSetUp]
    public async Task Init()
    {
        var client = CreateHttpClient();
        client.BaseAddress = new Uri("http://localhost:5206");
        await client.GetAsync("/seed");
        var response = await client.PostAsJsonAsync("/login", new { email = "admin", password = "Pass_123456" });
        var token = await response.Content.ReadAsStringAsync();
        dynamic jsonResponse = JsonConvert.DeserializeObject(token);
        token = jsonResponse.accessToken;
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
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