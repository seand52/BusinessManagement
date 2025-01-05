
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
    public class InvoiceCreatedEventTest
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
        public void InvoiceCreatedHandler_ValidInput_Successful()
        {
            var businessInfo = _fixture.Create<BusinessInfo>();
            var invoiceDto = _fixture.Create<InvoiceDetailDto>();
            var invoiceDocumentBuilder = new Mock<IInvoiceDocumentBuilder>();
            _unitOfWork.Setup(x => x.BusinessInfoRepository.GetBy(It.IsAny<Expression<Func<BusinessInfo, bool>>>())).ReturnsAsync(businessInfo);
            var handler = new InvoiceCreatedEventHandler(invoiceDocumentBuilder.Object, _unitOfWork.Object);
            var result = handler.Handle(new InvoiceCreatedEvent(invoiceDto), CancellationToken.None);
            _unitOfWork.Verify(x => x.BusinessInfoRepository.GetBy(It.IsAny<Expression<Func<BusinessInfo, bool>>>()), Times.Once);
            invoiceDocumentBuilder.Verify(x => x.Build(), Times.Once);
        }
    }
}