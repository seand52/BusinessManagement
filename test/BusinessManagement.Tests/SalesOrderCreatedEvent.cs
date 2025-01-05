using System.Linq.Expressions;
using BusinessManagement.DAL;
using BusinessManagement.Handlers;
using BusinessManagementApi.Extensions.Events;
using BusinessManagement.Templates;
using BusinessManagementApi.Dto;
using BusinessManagementApi.Models;

namespace BusinessManagement.UnitTests.Handlers
{
    [TestFixture]
    public class SalesOrderCreatedEventTest
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
        public void SalesOrderCreatedHandler_ValidInput_Successful()
        {
            var businessInfo = _fixture.Create<BusinessInfo>();
            var salesOrderDto = _fixture.Create<SalesOrderDetailDto>();
            var salesOrderDocumentBuilder = new Mock<ISalesOrderBuilder>();
            _unitOfWork.Setup(x => x.BusinessInfoRepository.GetBy(It.IsAny<Expression<Func<BusinessInfo, bool>>>())).ReturnsAsync(businessInfo);
            var handler = new SalesOrderCreatedEventHandler(salesOrderDocumentBuilder.Object, _unitOfWork.Object);
            var result = handler.Handle(new SalesOrderCreatedEvent(salesOrderDto), CancellationToken.None);
            _unitOfWork.Verify(x => x.BusinessInfoRepository.GetBy(It.IsAny<Expression<Func<BusinessInfo, bool>>>()), Times.Once);
            salesOrderDocumentBuilder.Verify(x => x.Build(), Times.Once);
        }
    }
}