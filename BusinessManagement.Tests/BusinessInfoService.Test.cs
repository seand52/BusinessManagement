using BusinessManagementApi.Services;
using BusinessManagementApi.DAL;
using BusinessManagementApi.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BusinessManagement.Tests.BusinessInfoTests;

public class Tests
{
    private Mock<IBusinessInfoRepository> _mockRepository;
    private IBusinessInfoService _businessInfoService;
    private Fixture _fixture;

    [SetUp]
    public void Setup()
    {
        _mockRepository = new Mock<IBusinessInfoRepository>();
        _businessInfoService = new BusinessManagementApi.Services.BusinessInfoService(_mockRepository.Object);
        _fixture = new Fixture();
        _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    [Test]
    public async Task GetBusinessInfoById_ValidId_ReturnsBusinessInfo()
    {
        var businessInfo = _fixture.Create<BusinessInfo>();
        _mockRepository.Setup(p => p.GetBusinessUserByUserId(1)).ReturnsAsync(businessInfo);

        var result = await _businessInfoService.GetBusinessInfoByUserId(1);
        Assert.That(businessInfo, Is.EqualTo(result));
        
    }

    [Test]
    public async Task GetBusinessInfoById_InvalidId_ReturnsNull()
    {
        var businessInfo = _fixture.Create<BusinessInfo>();

        _mockRepository.Setup(p => p.GetBusinessUserByUserId(1)).ReturnsAsync(businessInfo);

        var result = await _businessInfoService.GetBusinessInfoByUserId(2);
        Assert.That(result, Is.Null);
    }
    
    [Test]
    public async Task CreateBusinessInfo_InvalidModel_ReturnsFalse()
    {
        var businessInfo = new BusinessInfo();
        var modelState = new ModelStateDictionary();
        modelState.AddModelError("Name", "Is Invalid");
        var result = await _businessInfoService.CreateBusinessInfo(businessInfo, modelState);
        Assert.IsFalse(result);
    }
    
    [Test]
    public async Task CreateBusinessInfo_ValidModel_ReturnsTrue()
    {
        var businessInfo = _fixture.Create<BusinessInfo>();
        var modelState = new ModelStateDictionary();
        var result = await _businessInfoService.CreateBusinessInfo(businessInfo, modelState);
        Assert.IsTrue(result);
        _mockRepository.Verify(p => p.InsertBusinessInfo(businessInfo), Times.Once);
        _mockRepository.Verify(p => p.Save(), Times.Once);
    }

    [Test]
    public async Task UpdateBusinessInfo_ValidBusinessInfo_ReplacesWithNewDataCorrectly()
    {
        var businessInfo = _fixture.Create<BusinessInfo>();
        var newBusinessInfo = _fixture.Create<BusinessInfo>();

        await _businessInfoService.UpdateBusinessInfo(businessInfo, newBusinessInfo);
        //
        // TODO: Refactor this
        Assert.That(businessInfo.Name, Is.EqualTo(newBusinessInfo.Name));
        Assert.That(businessInfo.Address, Is.EqualTo(newBusinessInfo.Address));
        Assert.That(businessInfo.City, Is.EqualTo(newBusinessInfo.City));
        Assert.That(businessInfo.Postcode, Is.EqualTo(newBusinessInfo.Postcode));
        Assert.That(businessInfo.Telephone, Is.EqualTo(newBusinessInfo.Telephone));
        Assert.That(businessInfo.Email, Is.EqualTo(newBusinessInfo.Email));
        _mockRepository.Verify(p => p.UpdateBusinessInfo(businessInfo), Times.Once);
        _mockRepository.Verify(p => p.Save(), Times.Once);
    }
}