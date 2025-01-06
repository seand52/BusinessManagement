
using System.Linq.Expressions;
using BusinessManagement.Commands;
using BusinessManagement.DAL;
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
    public class SalesOrderHandlers
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
        public void GetSalesOrderHandler_InputIsValid_ReturnsSalesOrder()
        {
            var salesOrder = _fixture.Create<SalesOrder>();
            _unitOfWork.Setup(x => x.SalesOrderRepository.GetBy(1, "1")).ReturnsAsync(salesOrder);
            var handler = new GetSalesOrderHandler(_unitOfWork.Object);
            var result = handler.Handle(new GetSalesOrderQuery(1, "1"), CancellationToken.None).Result;
            Assert.That(result.Id, Is.EqualTo(salesOrder.Id));
        }
        
        [Test]
        public void GetAllSalesOrdersHandler_InputIsValid_ReturnsSalesOrders()
        {
            var salesOrders = _fixture.CreateMany<SalesOrder>(2).ToList();
            _unitOfWork.Setup(x => x.SalesOrderRepository.GetAllBy(It.IsAny<Expression<Func<SalesOrder, bool>>>(), It.IsAny<PaginationFilter>(), null, "Client"))
                .ReturnsAsync(new PagedList<SalesOrder>(salesOrders, 1, 2, 5));
            var handler = new GetAllSalesOrdersHandler(_unitOfWork.Object);
            var result = handler.Handle(new GetAllSalesOrdersQuery(new PaginationFilter(1, 2), "Test", "1"), CancellationToken.None).Result;
            Assert.That(result.TotalCount, Is.EqualTo(5));
            Assert.That(result.HasNextPage, Is.EqualTo(true));
            Assert.That(result.PageCount, Is.EqualTo(3));
            Assert.That(result.Items.Count, Is.EqualTo(2));
            Assert.That(result, Is.TypeOf<PagedList<SalesOrderDto>>());
            
        }
        
        [Test]
        public void DeleteSalesOrdersHandler_InputIsValid_ReturnsTrue()
        {
            var salesOrder = _fixture.Build<SalesOrder>().With(x => x.UserId, "1").Create();
            _unitOfWork.Setup(x => x.SalesOrderRepository.GetBy(1, "1")).ReturnsAsync(salesOrder);
            var handler = new DeletSalesOrderHandler(_unitOfWork.Object);
            var result = handler.Handle(new DeleteSalesOrderRequest(1, "1"), CancellationToken.None).Result;
            Assert.That(result, Is.True);
        }
        
        [Test]
        public void DeleteSalesOrdersHandler_InvalidUserId_ReturnsException()
        {
            var salesOrder = _fixture.Build<SalesOrder>().With(x => x.UserId, "5").Create();
            _unitOfWork.Setup(x => x.SalesOrderRepository.GetBy(1, "1")).ReturnsAsync(salesOrder);
            var handler = new DeletSalesOrderHandler(_unitOfWork.Object);
            async Task Code() => await handler.Handle(new DeleteSalesOrderRequest(1, "1"), CancellationToken.None);
            var ex = Assert.ThrowsAsync<UnauthorizedAccessException>(Code);
            Assert.That(ex.Message, Is.EqualTo("Insufficient Permissions"));
            
        }
        [Test]
        public void DeleteSalesOrdersHandler_InvalidSalesOrder_ReturnsFalse()
        {
            _unitOfWork.Setup(x => x.SalesOrderRepository.GetBy(1, "1")).ReturnsAsync(null as SalesOrder);
            var handler = new DeletSalesOrderHandler(_unitOfWork.Object);
            var result = handler.Handle(new DeleteSalesOrderRequest(1, "1"), CancellationToken.None).Result;
            Assert.That(result, Is.False);
            
        }
        [Test]
        public void UpdateSalesOrderHandler_InvalidSalesOrder_ReturnsError()
        {
            _unitOfWork.Setup(x => x.SalesOrderRepository.GetBy(1, "1")).ReturnsAsync(null as SalesOrder);
            var handler = new UpdateSalesOrderHandler(_unitOfWork.Object);
            async Task Code() => await handler.Handle(new UpdateSalesOrderRequest(new UpdateSalesOrderDto(), 1, "1"), CancellationToken.None);
            var ex = Assert.ThrowsAsync<Exception>(Code);
            Assert.That(ex.Message, Is.EqualTo("SalesOrder not found"));
        }
        [Test]
        public void UpdateSalesOrderHandler_InvalidUserId_ReturnsError()
        {
            var salesOrder = _fixture.Build<SalesOrder>().With(x => x.UserId, "2").Create();
            _unitOfWork.Setup(x => x.SalesOrderRepository.GetBy(1, "1")).ReturnsAsync(salesOrder);
            var handler = new UpdateSalesOrderHandler(_unitOfWork.Object);
            async Task Code() => await handler.Handle(new UpdateSalesOrderRequest(new UpdateSalesOrderDto(), 1, "1"), CancellationToken.None);
            var ex = Assert.ThrowsAsync<UnauthorizedAccessException>(Code);
            Assert.That(ex.Message, Is.EqualTo("Insufficient Permissions"));
        }
        [Test]
        public void UpdateSalesOrderHandler_ValidInput_ReturnsTrue()
        {
            var salesOrder = _fixture.Build<SalesOrder>().With(x => x.UserId, "1").Create();
            _unitOfWork.Setup(x => x.SalesOrderRepository.GetBy(1, "1")).ReturnsAsync(salesOrder);
            var handler = new UpdateSalesOrderHandler(_unitOfWork.Object);
            var result = handler.Handle(new UpdateSalesOrderRequest(new UpdateSalesOrderDto(), 1, "1"), CancellationToken.None);
            Assert.That(result.Result, Is.True);
        
        }
        [Test]
        public async Task CreateSalesOrderHandler_ValidInput_Success()
        {
            var salesOrder = _fixture.Build<SalesOrder>().With(x => x.UserId, "1").Create();
            var salesOrderDto = _fixture.Build<CreateSalesOrderDto>().With(x => x.ClientId, salesOrder.ClientId).Create();
            _unitOfWork.Setup(x => x.SalesOrderRepository.Insert(salesOrder)).Returns(Task.CompletedTask);
            _unitOfWork.Setup(x => x.ClientRepository.GetBy(It.IsAny<Expression<Func<Client, bool>>>()))
                .ReturnsAsync(salesOrder.Client);
            _unitOfWork.Setup(x => x.Save()).Returns(Task.CompletedTask);
            var handler = new CreateSalesOrderHandler(_unitOfWork.Object);
            var result = await handler.Handle(new CreateSalesOrderRequest(salesOrderDto, "1"), CancellationToken.None);
            // TODO: consider refactoring to not use It.IsAny
            _unitOfWork.Verify(x => x.SalesOrderRepository.Insert(It.IsAny<SalesOrder>()), Times.Once);
            _unitOfWork.Verify(x => x.Save(), Times.Once);
            Assert.That(result.Client.Id, Is.EqualTo(salesOrder.Client.Id));
            Assert.That(result, Is.TypeOf<SalesOrderDetailDto>());
        
        }
    }
}