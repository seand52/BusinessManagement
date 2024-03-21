using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using BusinessManagement.Tests;
using BusinessManagementApi.Dto;
using BusinessManagementApi.Models;
using Newtonsoft.Json;

namespace BusinessManagement.SalesOrderTests;

[TestFixture]
public class SalesOrders: IntegrationTestWebAppFactory
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
    public async Task CreateSalesOrderSuccess()
    {
        var fixture = new Fixture();
        fixture.Customize<CreateSalesOrderProductDto>(composer => composer
            .With(p => p.ProductId, fixture.Create<int>() % 10 + 1));
        var salesOrder = fixture.Create<CreateSalesOrderDto>();
        salesOrder.ClientId = 1;
        var response = await _client.PostAsJsonAsync("/api/SalesOrders", salesOrder);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
    }
    
    [Test]
    public async Task RetrieveSalesOrderByIdSuccess()
    {
        var response = await _client.GetAsync($"/api/SalesOrders/2");
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }
    
    [Test]
    public async Task RetrieveSalesOrdersSuccess()
    {
        var response = await _client.GetAsync("/api/SalesOrders?PageNumber=1&PageSize=10");
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }
    
    [Test]
    public async Task DeleteSalesOrderSuccess()
    {
        var response = await _client.DeleteAsync("/api/SalesOrders/1");
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
    }
    
    [Test]
    public async Task UpdateSalesOrderSuccess()
    {
        var fixture = new Fixture();
        fixture.Customize<CreateSalesOrderProductDto>(composer => composer
            .With(p => p.ProductId, fixture.Create<int>() % 10 + 1));
        var salesOrder = fixture.Create<CreateSalesOrderDto>();
        salesOrder.ClientId = 1;
        var response = await _client.PutAsJsonAsync($"/api/SalesOrders/4", salesOrder);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
    }
}