using System.Linq.Expressions;
using BusinessManagement.Commands;
using BusinessManagement.Commands.SalesOrders;
using BusinessManagement.DAL;
using BusinessManagement.Handlers;
using BusinessManagementApi.Models;

namespace BusinessManagement.UnitTests.Handlers
{
    [TestFixture]
    public class ConvertInvoiceToSalesOrderHandlerTest
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
        public void SalesOrderConvertedSuccessful()
        {
            var salesOrder = _fixture.Create<SalesOrder>();
            _unitOfWork.Setup(x => x.SalesOrderRepository.GetBy(salesOrder.Id, salesOrder.UserId))
                .ReturnsAsync(salesOrder);
            _unitOfWork.Setup(x => x.InvoiceRepository.Insert(It.IsAny<Invoice>()));
            _unitOfWork.Setup(x => x.Save()).Returns(Task.CompletedTask);
            var handler = new ConvertSalesOrderToInvoiceHandler(_unitOfWork.Object);
            var result = handler.Handle(new ConvertSalesOrderToInvoiceRequest(salesOrder.Id, salesOrder.UserId), CancellationToken.None).Result;
            Assert.IsTrue(result);
        }
        
        [Test]
        public void ConvertSalesOrderNotFound()
        {
            var salesOrder = _fixture.Create<SalesOrder>();
            _unitOfWork.Setup(x => x.SalesOrderRepository.GetBy(salesOrder.Id, salesOrder.UserId))
                .ReturnsAsync(null as SalesOrder);
            var handler = new ConvertSalesOrderToInvoiceHandler(_unitOfWork.Object);
            var result = handler.Handle(new ConvertSalesOrderToInvoiceRequest(salesOrder.Id, salesOrder.UserId), CancellationToken.None).Result;
            Assert.IsFalse(result);
        }
        
        [Test]
        public void ConvertSalesOrderExpired()
        {
            var salesOrder = _fixture.Create<SalesOrder>();
            salesOrder.Expired = 1;
            _unitOfWork.Setup(x => x.SalesOrderRepository.GetBy(salesOrder.Id, salesOrder.UserId))
                .ReturnsAsync(null as SalesOrder);
            var handler = new ConvertSalesOrderToInvoiceHandler(_unitOfWork.Object);
            var result = handler.Handle(new ConvertSalesOrderToInvoiceRequest(salesOrder.Id, salesOrder.UserId), CancellationToken.None).Result;
            Assert.IsFalse(result);
        }
        
        [Test]
        public void ConvertSalesOrderNoProducts()
        {
            var salesOrder = _fixture.Create<SalesOrder>();
            salesOrder.SalesOrderProducts = [];
            _unitOfWork.Setup(x => x.SalesOrderRepository.GetBy(salesOrder.Id, salesOrder.UserId))
                .ReturnsAsync(null as SalesOrder);
            var handler = new ConvertSalesOrderToInvoiceHandler(_unitOfWork.Object);
            var result = handler.Handle(new ConvertSalesOrderToInvoiceRequest(salesOrder.Id, salesOrder.UserId), CancellationToken.None).Result;
            Assert.IsFalse(result);
        }
    }
}