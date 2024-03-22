using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using BusinessManagement.Tests;
using BusinessManagementApi.Dto;
using Newtonsoft.Json;

namespace BusinessManagement.ProductTests;

[TestFixture]
public class Products: IntegrationTestWebAppFactory
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
    public async Task CreateProductSuccess()
    {
        var fixture = new Fixture();
        var product = fixture.Create<CreateProductDto>();
        var response = await _client.PostAsJsonAsync("/api/Products", product);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
    }
    
    [Test]
    public async Task RetrieveProductByIdSuccess()
    {
        var response = await _client.GetAsync("/api/Products/2");
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }
    
    [Test]
    public async Task RetrieveProductSuccess()
    {
        var response = await _client.GetAsync("/api/Products?PageNumber=1&PageSize=10");
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }
    
    [Test]
    public async Task DeleteProductSuccess()
    {
        var response = await _client.DeleteAsync("/api/Products/1");
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
    }
    
    [Test]
    public async Task UpdateProductSuccess()
    {
        var fixture = new Fixture();
        var client = fixture.Create<CreateProductDto>();
        var response = await _client.PutAsJsonAsync("/api/Products/4", client);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
    }
    // [Test]
    // public async Task RetrieveProductByIdFromAnotherUserNotFound()
    // {
    //     var response = await _client.PostAsJsonAsync("/login", new { email = "admin2", password = "Pass_123456" });
    //     var token = await response.Content.ReadAsStringAsync();
    //     dynamic jsonResponse = JsonConvert.DeserializeObject(token);
    //     token = jsonResponse.accessToken;
    //     // _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    //     var clientResponse = await _client.GetAsync($"api/Products/7");
    //     Assert.That(clientResponse.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    // }
    //
    // [Test]
    // public async Task DeleteProductFromAnotherUserBadRequest()
    // {
    //     var response = await _client.PostAsJsonAsync("/login", new { email = "admin2", password = "Pass_123456" });
    //     var token = await response.Content.ReadAsStringAsync();
    //     dynamic jsonResponse = JsonConvert.DeserializeObject(token);
    //     token = jsonResponse.accessToken;
    //     _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    //     var clientResponse = await _client.DeleteAsync($"api/Products/5");
    //     Assert.That(clientResponse.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    // }
}