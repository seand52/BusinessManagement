
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
    public class InvoiceHandlers
    {
        private Mock<IInvoiceRepository> _invoiceRepository;
        private Fixture _fixture;

        [SetUp]
        public void SetUp()
        {
            _invoiceRepository = new Mock<IInvoiceRepository>();
            _fixture = new Fixture();

        }

        [Test]
        public void GetInvoiceHandler_InputIsValid_ReturnsInvoice()
        {
            var invoice = _fixture.Create<Invoice>();
            _invoiceRepository.Setup(x => x.GetInvoiceById(1, "1")).ReturnsAsync(invoice);
            var handler = new GetInvoiceHandler(_invoiceRepository.Object);
            var result = handler.Handle(new GetInvoiceQuery(1, "1"), CancellationToken.None).Result;
            Assert.That(result.Id, Is.EqualTo(invoice.Id));
        }
        
        [Test]
        public void GetAllInvoicesHandler_InputIsValid_ReturnsInvoices()
        {
            var invoices = _fixture.CreateMany<Invoice>(2).ToList();
            _invoiceRepository.Setup(x => x.GetInvoices(It.IsAny<PaginationFilter>(), "Test", "1")).ReturnsAsync(new PagedList<Invoice>(invoices, 1, 2, 2));
            var handler = new GetAllInvoicesHandler(_invoiceRepository.Object);
            var result = handler.Handle(new GetAllInvoicesQuery(new PaginationFilter(1, 2), "Test", "1"), CancellationToken.None).Result;
            Assert.That(result.Items.Count, Is.EqualTo(2));
            Assert.That(result, Is.TypeOf<PagedList<InvoiceDto>>());
            
        }
        
        [Test]
        public void DeleteInvoicesHandler_InputIsValid_ReturnsTrue()
        {
            var invoice = _fixture.Build<Invoice>().With(x => x.UserId, "1").Create();
            _invoiceRepository.Setup(x => x.GetInvoiceById(1, "1")).ReturnsAsync(invoice);
            var handler = new DeletInvoiceHandler(_invoiceRepository.Object);
            var result = handler.Handle(new DeleteInvoiceRequest(1, "1"), CancellationToken.None).Result;
            Assert.That(result, Is.True);
        }
        
        [Test]
        public void DeleteInvoicesHandler_InvalidUserId_ReturnsException()
        {
            var invoice = _fixture.Build<Invoice>().With(x => x.UserId, "5").Create();
            _invoiceRepository.Setup(x => x.GetInvoiceById(1, "1")).ReturnsAsync(invoice);
            var handler = new DeletInvoiceHandler(_invoiceRepository.Object);
            async Task Code() => await handler.Handle(new DeleteInvoiceRequest(1, "1"), CancellationToken.None);
            var ex = Assert.ThrowsAsync<UnauthorizedAccessException>(Code);
            Assert.That(ex.Message, Is.EqualTo("Insufficient Permissions"));
            
        }
        [Test]
        public void DeleteInvoicesHandler_InvalidInvoice_ReturnsFalse()
        {
            _invoiceRepository.Setup(x => x.GetInvoiceById(1, "1")).ReturnsAsync(null as Invoice);
            var handler = new DeletInvoiceHandler(_invoiceRepository.Object);
            var result = handler.Handle(new DeleteInvoiceRequest(1, "1"), CancellationToken.None).Result;
            Assert.That(result, Is.False);
            
        }
        [Test]
        public void UpdateInvoiceHandler_InvalidInvoice_ReturnsError()
        {
            _invoiceRepository.Setup(x => x.GetInvoiceById(1, "1")).ReturnsAsync(null as Invoice);
            var handler = new UpdateInvoiceHandler(_invoiceRepository.Object);
            async Task Code() => await handler.Handle(new UpdateInvoiceRequest(new UpdateInvoiceDto(), 1, "1"), CancellationToken.None);
            var ex = Assert.ThrowsAsync<Exception>(Code);
            Assert.That(ex.Message, Is.EqualTo("Invoice not found"));
        }
        [Test]
        public void UpdateInvoiceHandler_InvalidUserId_ReturnsError()
        {
            var invoice = _fixture.Build<Invoice>().With(x => x.UserId, "2").Create();
            _invoiceRepository.Setup(x => x.GetInvoiceById(1, "1")).ReturnsAsync(invoice);
            var handler = new UpdateInvoiceHandler(_invoiceRepository.Object);
            async Task Code() => await handler.Handle(new UpdateInvoiceRequest(new UpdateInvoiceDto(), 1, "1"), CancellationToken.None);
            var ex = Assert.ThrowsAsync<UnauthorizedAccessException>(Code);
            Assert.That(ex.Message, Is.EqualTo("Insufficient Permissions"));
        }
        [Test]
        public void UpdateInvoiceHandler_ValidInput_ReturnsTrue()
        {
            var invoice = _fixture.Build<Invoice>().With(x => x.UserId, "1").Create();
            _invoiceRepository.Setup(x => x.GetInvoiceById(1, "1")).ReturnsAsync(invoice);
            var handler = new UpdateInvoiceHandler(_invoiceRepository.Object);
            var result = handler.Handle(new UpdateInvoiceRequest(new UpdateInvoiceDto(), 1, "1"), CancellationToken.None);
            Assert.That(result.Result, Is.True);
        
        }
        [Test]
        public async Task CreateInvoiceHandler_ValidInput_Success()
        {
            var invoice = _fixture.Build<Invoice>().With(x => x.UserId, "1").Create();
            var invoiceDto = _fixture.Build<CreateInvoiceDto>().With(x => x.ClientId, invoice.ClientId).Create();
            var clientRepository = new Mock<IClientRepository>();
            _invoiceRepository.Setup(x => x.InsertInvoice(invoice)).Returns(Task.CompletedTask);
            clientRepository.Setup(x => x.GetClientById(invoice.ClientId, "1")).ReturnsAsync(invoice.Client);
            _invoiceRepository.Setup(x => x.Save()).Returns(Task.CompletedTask);
            var handler = new CreateInvoiceHandler(_invoiceRepository.Object, clientRepository.Object);
            var result = await handler.Handle(new CreateInvoiceRequest(invoiceDto, "1"), CancellationToken.None);
            // TODO: consider refactoring to not use It.IsAny
            _invoiceRepository.Verify(x => x.InsertInvoice(It.IsAny<Invoice>()), Times.Once);
            _invoiceRepository.Verify(x => x.Save(), Times.Once);
            Assert.That(result.Client.Id, Is.EqualTo(invoice.Client.Id));
            Assert.That(result, Is.TypeOf<InvoiceDetailDto>());
        
        }
    }
}