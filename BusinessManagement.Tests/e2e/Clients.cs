using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using BusinessManagement.Tests;
using BusinessManagementApi.Dto;
using Newtonsoft.Json;

namespace BusinessManagement.ClientTests;

[TestFixture]
public class Clients: IntegrationTestWebAppFactory
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
    public async Task CreateClientSuccess()
    {
        var fixture = new Fixture();
        var client = fixture.Create<CreateClientDto>();
        client.Email = "test@gmail.com";
        var response = await _client.PostAsJsonAsync("/api/Clients", client);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
    }
    
    [Test]
    public async Task RetrieveClientByIdSuccess()
    {
        var response = await _client.GetAsync("/api/Clients/2");
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }
    
    [Test]
    public async Task RetrieveClientSuccess()
    {
        var response = await _client.GetAsync("/api/Clients?PageNumber=1&PageSize=10");
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }
    
    [Test]
    public async Task DeleteClientSuccess()
    {
        var response = await _client.DeleteAsync("/api/Clients/1");
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
    }
    
    [Test]
    public async Task UpdateClientSuccess()
    {
        var fixture = new Fixture();
        var client = fixture.Create<CreateClientDto>();
        client.Email = "test2@gmail.com";
        var response = await _client.PutAsJsonAsync("/api/Clients/4", client);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
    }
    
    [Test]
    public async Task GetInvoicesForClient()
    {
        var response = await _client.GetAsync("/api/Clients/1/invoices?PageNumber=1&PageSize=10");
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }
    // [Test]
    // public async Task RetrieveClientByIdFromAnotherUserNotFound()
    // {
    //     var response = await _client.PostAsJsonAsync("/login", new { email = "admin2", password = "Pass_123456" });
    //     var token = await response.Content.ReadAsStringAsync();
    //     dynamic jsonResponse = JsonConvert.DeserializeObject(token);
    //     token = jsonResponse.accessToken;
    //     // _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    //     var clientResponse = await _client.GetAsync($"api/Clients/7");
    //     Assert.That(clientResponse.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    // }
    //
    // [Test]
    // public async Task DeleteClientFromAnotherUserBadRequest()
    // {
    //     var response = await _client.PostAsJsonAsync("/login", new { email = "admin2", password = "Pass_123456" });
    //     var token = await response.Content.ReadAsStringAsync();
    //     dynamic jsonResponse = JsonConvert.DeserializeObject(token);
    //     token = jsonResponse.accessToken;
    //     _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    //     var clientResponse = await _client.DeleteAsync($"api/Clients/5");
    //     Assert.That(clientResponse.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    // }
}