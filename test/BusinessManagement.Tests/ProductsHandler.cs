
using System.Linq.Expressions;
using BusinessManagement.Commands;
using BusinessManagement.DAL;
using BusinessManagement.Filter;
using BusinessManagement.Handlers;
using BusinessManagement.Helpers;
using BusinessManagement.Queries;
using BusinessManagementApi.Dto;
using BusinessManagementApi.Models;

namespace BusinessManagement.UnitTests.Handlers
{
    [TestFixture]
    public class ProductHandlers
    {
        private Mock<IUnitOfWork> _unitOfWork;
        private Fixture _fixture;

        [SetUp]
        public void SetUp()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _fixture = new Fixture();

        }

        [Test]
        public void GetProductHandler_InputIsValid_ReturnsProduct()
        {
            var product = _fixture.Create<Product>();
            _unitOfWork.Setup(x => x.ProductRepository.GetBy(It.IsAny<Expression<Func<Product, bool>>>()))
                .ReturnsAsync(product);
            var handler = new GetProductHandler(_unitOfWork.Object);
            var result = handler.Handle(new GetProductQuery(1, "1"), CancellationToken.None).Result;
            Assert.That(result.Id, Is.EqualTo(product.Id));
        }
        
        [Test]
        public void GetAllProductsHandler_InputIsValid_ReturnsProducts()
        {
            var products = _fixture.CreateMany<Product>(2).ToList();
            
            _unitOfWork.Setup(x => x.ProductRepository.GetAllBy(It.IsAny<Expression<Func<Product, bool>>>(), It.IsAny<PaginationFilter>(), It.IsAny<Expression<Func<Product, bool>>>(), It.IsAny<string>()))
                .ReturnsAsync(new PagedList<Product>(products, 1, 2, 2));
            var handler = new GetAllProductsHandler(_unitOfWork.Object);
            var result = handler.Handle(new GetAllProductsQuery(new PaginationFilter(1, 2), "Test", "1"), CancellationToken.None).Result;
            Assert.That(result.Items.Count, Is.EqualTo(2));
            Assert.That(result, Is.TypeOf<PagedList<ProductDto>>());
            
        }
        //
        [Test]
        public void DeleteProductsHandler_InputIsValid_ReturnsTrue()
        {
            var product = _fixture.Build<Product>().With(x => x.UserId, "1").Create();
            _unitOfWork.Setup(x => x.ProductRepository.GetBy(It.IsAny<Expression<Func<Product, bool>>>())).ReturnsAsync(product);
            var handler = new DeleteProductHandler(_unitOfWork.Object);
            var result = handler.Handle(new DeleteProductRequest(1, "1"), CancellationToken.None).Result;
            Assert.That(result, Is.True);
        }
        
        [Test]
        public void DeleteProductsHandler_InvalidUserId_ReturnsException()
        {
            var product = _fixture.Build<Product>().With(x => x.UserId, "5").Create();
            _unitOfWork.Setup(x => x.ProductRepository.GetBy(It.IsAny<Expression<Func<Product, bool>>>())).ReturnsAsync(product);
            var handler = new DeleteProductHandler(_unitOfWork.Object);
            async Task Code() => await handler.Handle(new DeleteProductRequest(1, "1"), CancellationToken.None);
            var ex = Assert.ThrowsAsync<UnauthorizedAccessException>(Code);
            Assert.That(ex.Message, Is.EqualTo("Insufficient Permissions"));
            
        }
        [Test]
        public void DeleteProductsHandler_InvalidProduct_ReturnsFalse()
        {
            _unitOfWork.Setup(x => x.ProductRepository.GetBy(It.IsAny<Expression<Func<Product, bool>>>())).ReturnsAsync(null as Product);            
            var handler = new DeleteProductHandler(_unitOfWork.Object);
            var result = handler.Handle(new DeleteProductRequest(1, "1"), CancellationToken.None).Result;
            Assert.That(result, Is.False);
            
        }
        [Test]
        public void UpdateProductHandler_InvalidProduct_ReturnsError()
        {
            _unitOfWork.Setup(x => x.ProductRepository.GetBy(It.IsAny<Expression<Func<Product, bool>>>())).ReturnsAsync(null as Product);
            var handler = new UpdateProductHandler(_unitOfWork.Object);
            async Task Code() => await handler.Handle(new UpdateProductRequest(new UpdateProductDto(), 1, "1"), CancellationToken.None);
            var ex = Assert.ThrowsAsync<Exception>(Code);
            Assert.That(ex.Message, Is.EqualTo("Product not found"));
        }
        [Test]
        public void UpdateProductHandler_InvalidUserId_ReturnsError()
        {
            var product = _fixture.Build<Product>().With(x => x.UserId, "2").Create();
            _unitOfWork.Setup(x => x.ProductRepository.GetBy(It.IsAny<Expression<Func<Product, bool>>>())).ReturnsAsync(product);
            var handler = new UpdateProductHandler(_unitOfWork.Object);
            async Task Code() => await handler.Handle(new UpdateProductRequest(new UpdateProductDto(), 1, "1"), CancellationToken.None);
            var ex = Assert.ThrowsAsync<UnauthorizedAccessException>(Code);
            Assert.That(ex.Message, Is.EqualTo("Insufficient Permissions"));
        }
        [Test]
        public void UpdateProductHandler_ValidInput_ReturnsTrue()
        {
            var product = _fixture.Build<Product>().With(x => x.UserId, "1").Create();
            _unitOfWork.Setup(x => x.ProductRepository.GetBy(It.IsAny<Expression<Func<Product, bool>>>())).ReturnsAsync(product);
            var handler = new UpdateProductHandler(_unitOfWork.Object);
            var result = handler.Handle(new UpdateProductRequest(new UpdateProductDto(), 1, "1"), CancellationToken.None);
            Assert.That(result.Result, Is.True);
        
        }
        [Test]
        public async Task CreateProductHandler_ValidInput_Success()
        {
            var product = _fixture.Build<Product>().With(x => x.UserId, "1").Create();
            var productDto = _fixture.Build<CreateProductDto>().Create();
            _unitOfWork.Setup(x => x.ProductRepository.Insert(product)).Returns(Task.CompletedTask);
            _unitOfWork.Setup(x => x.Save()).Returns(Task.CompletedTask);
            var handler = new CreateProductHandler(_unitOfWork.Object);
            await handler.Handle(new CreateProductRequest(productDto, "1"), CancellationToken.None);
            // TODO: consider refactoring to not use It.IsAny
            _unitOfWork.Verify(x => x.ProductRepository.Insert(It.IsAny<Product>()), Times.Once);
            _unitOfWork.Verify(x => x.Save(), Times.Once);
        
        }
    }
}