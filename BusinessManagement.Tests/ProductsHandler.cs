
using BusinessManagement.Commands;
using BusinessManagement.Filter;
using BusinessManagement.Handlers;
using BusinessManagement.Helpers;
using BusinessManagement.Queries;
using BusinessManagementApi.DAL;
using BusinessManagementApi.Dto;
using BusinessManagementApi.Models;

namespace BusinessManagement.UnitTests.Handlers
{
    [TestFixture]
    public class ProductHandlers
    {
        private Mock<IProductRepository> _productRepository;
        private Fixture _fixture;

        [SetUp]
        public void SetUp()
        {
            _productRepository = new Mock<IProductRepository>();
            _fixture = new Fixture();

        }

        [Test]
        public void GetProductHandler_InputIsValid_ReturnsProduct()
        {
            var product = _fixture.Create<Product>();
            _productRepository.Setup(x => x.GetProductById(1, "1")).ReturnsAsync(product);
            var handler = new GetProductHandler(_productRepository.Object);
            var result = handler.Handle(new GetProductQuery(1, "1"), CancellationToken.None).Result;
            Assert.That(result.Id, Is.EqualTo(product.Id));
        }
        
        [Test]
        public void GetAllProductsHandler_InputIsValid_ReturnsProducts()
        {
            var products = _fixture.CreateMany<Product>(2).ToList();
            _productRepository.Setup(x => x.GetProducts(It.IsAny<PaginationFilter>(), "Test", "1")).ReturnsAsync(new PagedList<Product>(products, 1, 2, 2));
            var handler = new GetAllProductsHandler(_productRepository.Object);
            var result = handler.Handle(new GetAllProductsQuery(new PaginationFilter(1, 2), "Test", "1"), CancellationToken.None).Result;
            Assert.That(result.Items.Count, Is.EqualTo(2));
            Assert.That(result, Is.TypeOf<PagedList<ProductDto>>());
            
        }
        
        [Test]
        public void DeleteProductsHandler_InputIsValid_ReturnsTrue()
        {
            var product = _fixture.Build<Product>().With(x => x.UserId, "1").Create();
            _productRepository.Setup(x => x.GetProductById(1, "1")).ReturnsAsync(product);
            var handler = new DeleteProductHandler(_productRepository.Object);
            var result = handler.Handle(new DeleteProductRequest(1, "1"), CancellationToken.None).Result;
            Assert.That(result, Is.True);
        }
        
        [Test]
        public void DeleteProductsHandler_InvalidUserId_ReturnsException()
        {
            var product = _fixture.Build<Product>().With(x => x.UserId, "5").Create();
            _productRepository.Setup(x => x.GetProductById(1, "1")).ReturnsAsync(product);
            var handler = new DeleteProductHandler(_productRepository.Object);
            async Task Code() => await handler.Handle(new DeleteProductRequest(1, "1"), CancellationToken.None);
            var ex = Assert.ThrowsAsync<UnauthorizedAccessException>(Code);
            Assert.That(ex.Message, Is.EqualTo("Insufficient Permissions"));
            
        }
        [Test]
        public void DeleteProductsHandler_InvalidProduct_ReturnsFalse()
        {
            _productRepository.Setup(x => x.GetProductById(1, "1")).ReturnsAsync(null as Product);
            var handler = new DeleteProductHandler(_productRepository.Object);
            var result = handler.Handle(new DeleteProductRequest(1, "1"), CancellationToken.None).Result;
            Assert.That(result, Is.False);
            
        }
        [Test]
        public void UpdateProductHandler_InvalidProduct_ReturnsError()
        {
            _productRepository.Setup(x => x.GetProductById(1, "1")).ReturnsAsync(null as Product);
            var handler = new UpdateProductHandler(_productRepository.Object);
            async Task Code() => await handler.Handle(new UpdateProductRequest(new UpdateProductDto(), 1, "1"), CancellationToken.None);
            var ex = Assert.ThrowsAsync<Exception>(Code);
            Assert.That(ex.Message, Is.EqualTo("Product not found"));
        }
        [Test]
        public void UpdateProductHandler_InvalidUserId_ReturnsError()
        {
            var product = _fixture.Build<Product>().With(x => x.UserId, "2").Create();
            _productRepository.Setup(x => x.GetProductById(1, "1")).ReturnsAsync(product);
            var handler = new UpdateProductHandler(_productRepository.Object);
            async Task Code() => await handler.Handle(new UpdateProductRequest(new UpdateProductDto(), 1, "1"), CancellationToken.None);
            var ex = Assert.ThrowsAsync<UnauthorizedAccessException>(Code);
            Assert.That(ex.Message, Is.EqualTo("Insufficient Permissions"));
        }
        [Test]
        public void UpdateProductHandler_ValidInput_ReturnsTrue()
        {
            var product = _fixture.Build<Product>().With(x => x.UserId, "1").Create();
            _productRepository.Setup(x => x.GetProductById(1, "1")).ReturnsAsync(product);
            var handler = new UpdateProductHandler(_productRepository.Object);
            var result = handler.Handle(new UpdateProductRequest(new UpdateProductDto(), 1, "1"), CancellationToken.None);
            Assert.That(result.Result, Is.True);
        
        }
        [Test]
        public async Task CreateProductHandler_ValidInput_Success()
        {
            var product = _fixture.Build<Product>().With(x => x.UserId, "1").Create();
            var productDto = _fixture.Build<CreateProductDto>().Create();
            _productRepository.Setup(x => x.InsertProduct(product)).Returns(Task.CompletedTask);
            _productRepository.Setup(x => x.Save()).Returns(Task.CompletedTask);
            var handler = new CreateProductHandler(_productRepository.Object);
            await handler.Handle(new CreateProductRequest(productDto, "1"), CancellationToken.None);
            // TODO: consider refactoring to not use It.IsAny
            _productRepository.Verify(x => x.InsertProduct(It.IsAny<Product>()), Times.Once);
            _productRepository.Verify(x => x.Save(), Times.Once);
        
        }
    }
}