using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using BusinessManagement.Tests;
using BusinessManagementApi.Dto;
using Newtonsoft.Json;
using NuGet.Protocol;

namespace BusinessManagement.BusinessInfoTests;

[TestFixture]
public class BusinessInfos: IntegrationTestWebAppFactory
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
    public async Task CreateBusinessInfoSuccess()
    {
        var fixture = new Fixture();
        var businessInfo = fixture.Create<CreateBusinessInfoDto>();
        businessInfo.Email = "business@gmail.com";
        var response = await _client.PostAsJsonAsync("/api/BusinessInfo", businessInfo);
        Console.WriteLine(response.ToJson());
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
    }
    
    [Test]
    public async Task RetrieveBusinessInfoByIdSuccess()
    {
        var response = await _client.GetAsync("/api/BusinessInfo");
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }
    
    
    [Test]
    public async Task UpdateBusinessInfoSuccess()
    {
        var fixture = new Fixture();
        var businessInfo = fixture.Create<CreateBusinessInfoDto>();
        businessInfo.Email = "business@gmail.com";
        var response = await _client.PutAsJsonAsync("/api/BusinessInfo", businessInfo);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
    }
}