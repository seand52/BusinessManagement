using BusinessManagementApi.Services;
using BusinessManagementApi.DAL;
using BusinessManagementApi.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BusinessManagement.Tests.ClientService;

public class Tests
{
    private Mock<IClientRepository> _mockRepository;
    private IClientService _clientService;
    private Fixture _fixture;

    [SetUp]
    public void Setup()
    {
        _mockRepository = new Mock<IClientRepository>();
        _clientService = new BusinessManagementApi.Services.ClientService(_mockRepository.Object);
        _fixture = new Fixture();
        _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    [Test]
    public async Task GetClientById_ValidId_ReturnsClient()
    {
        var client = _fixture.Create<Client>();
        _mockRepository.Setup(p => p.GetClientById(1)).ReturnsAsync(client);

        var result = await _clientService.GetClientById(1);
        Assert.That(client, Is.EqualTo(result));
        
    }

    [Test]
    public async Task GetClientById_InvalidId_ReturnsNull()
    {
        var client = _fixture.Create<Client>();

        _mockRepository.Setup(p => p.GetClientById(1)).ReturnsAsync(client);

        var result = await _clientService.GetClientById(2);
        Assert.That(result, Is.Null);
    }
    
    [Test]
    public async Task CreateClient_InvalidModel_ReturnsFalse()
    {
        var client = new Client();
        var modelState = new ModelStateDictionary();
        modelState.AddModelError("Name", "Is Invalid");
        var result = await _clientService.CreateClient(client, modelState);
        Assert.IsFalse(result);
    }
    
    [Test]
    public async Task CreateClient_ValidModel_ReturnsTrue()
    {
        var client = _fixture.Create<Client>();
        var modelState = new ModelStateDictionary();
        var result = await _clientService.CreateClient(client, modelState);
        Assert.IsTrue(result);
        _mockRepository.Verify(p => p.InsertClient(client), Times.Once);
        _mockRepository.Verify(p => p.Save(), Times.Once);
    }
    
    [Test]
    public async Task UpdateClient_ValidClient_ReplacesWithNewDataCorrectly()
    {
        var client = _fixture.Create<Client>();
        var newClient = _fixture.Create<Client>();
        
        await _clientService.UpdateClient(client, newClient);
        //
        // TODO: Refactor this
        Assert.That(client.Name, Is.EqualTo(newClient.Name));
        Assert.That(client.ShopName, Is.EqualTo(newClient.ShopName));
        Assert.That(client.Address, Is.EqualTo(newClient.Address));
        Assert.That(client.City, Is.EqualTo(newClient.City));
        Assert.That(client.Province, Is.EqualTo(newClient.Province));
        Assert.That(client.Postcode, Is.EqualTo(newClient.Postcode));
        Assert.That(client.DocumentNum, Is.EqualTo(newClient.DocumentNum));
        Assert.That(client.DocumentType, Is.EqualTo(newClient.DocumentType));
        Assert.That(client.Telephone, Is.EqualTo(newClient.Telephone));
        Assert.That(client.Email, Is.EqualTo(newClient.Email));
        _mockRepository.Verify(p => p.UpdateClient(client), Times.Once);
        _mockRepository.Verify(p => p.Save(), Times.Once);
    }
    
    [Test]
    public async Task DeleteClient_ValidClient_CallsTheDALMethodsCorrectly()
    {
        var client = _fixture.Create<Client>();
        await _clientService.DeleteClient(client);
        _mockRepository.Verify(p => p.DeleteClient(client), Times.Once);
        _mockRepository.Verify(p => p.Save(), Times.Once);
    }
}