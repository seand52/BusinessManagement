using BusinessManagementApi.Services;
using BusinessManagementApi.DAL;
using BusinessManagementApi.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BusinessManagement.Tests.ProductService;

public class Tests
{
    private Mock<IProductRepository> _mockRepository;
    private IProductService _productService;
    private Fixture _fixture;

    [SetUp]
    public void Setup()
    {
        _mockRepository = new Mock<IProductRepository>();
        _productService = new BusinessManagementApi.Services.ProductService(_mockRepository.Object);
        _fixture = new Fixture();
        _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    [Test]
    public async Task GetProductById_ValidId_ReturnsProduct()
    {
        var product = _fixture.Create<Product>();
        _mockRepository.Setup(p => p.GetProductById(1)).ReturnsAsync(product);

        var result = await _productService.GetProductById(1);
        Assert.That(product, Is.EqualTo(result));
        
    }

    [Test]
    public async Task GetProductById_InvalidId_ReturnsNull()
    {
        var product = _fixture.Create<Product>();

        _mockRepository.Setup(p => p.GetProductById(1)).ReturnsAsync(product);

        var result = await _productService.GetProductById(2);
        Assert.That(result, Is.Null);
    }
    
    [Test]
    public async Task CreateProduct_InvalidModel_ReturnsFalse()
    {
        var product = new Product();
        var modelState = new ModelStateDictionary();
        modelState.AddModelError("Name", "Is Invalid");
        var result = await _productService.CreateProduct(product, modelState);
        Assert.IsFalse(result);
    }
    
    [Test]
    public async Task CreateProduct_ValidModel_ReturnsTrue()
    {
        var product = _fixture.Create<Product>();
        var modelState = new ModelStateDictionary();
        var result = await _productService.CreateProduct(product, modelState);
        Assert.IsTrue(result);
        _mockRepository.Verify(p => p.InsertProduct(product), Times.Once);
        _mockRepository.Verify(p => p.Save(), Times.Once);
    }
    
    [Test]
    public async Task UpdateProduct_ValidProduct_ReplacesWithNewDataCorrectly()
    {
        var product = _fixture.Create<Product>();
        var newProduct = _fixture.Create<Product>();
        
        await _productService.UpdateProduct(product, newProduct);
        // TODO: Refactor this
        Assert.That(product.Reference, Is.EqualTo(newProduct.Reference));
        Assert.That(product.Description, Is.EqualTo(newProduct.Description));
        Assert.That(product.Price, Is.EqualTo(newProduct.Price));
        Assert.That(product.Stock, Is.EqualTo(newProduct.Stock));
        _mockRepository.Verify(p => p.UpdateProduct(product), Times.Once);
        _mockRepository.Verify(p => p.Save(), Times.Once);
    }
    
    [Test]
    public async Task DeleteProduct_ValidProduct_CallsTheDALMethodsCorrectly()
    {
        var product = _fixture.Create<Product>();
        await _productService.DeleteProduct(product);
        _mockRepository.Verify(p => p.DeleteProduct(product), Times.Once);
        _mockRepository.Verify(p => p.Save(), Times.Once);
    }
}