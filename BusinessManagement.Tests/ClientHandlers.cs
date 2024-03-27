
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
    public class ClientHandlers
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
        public void GetClientHandler_InputIsValid_ReturnsClient()
        {
            var client = _fixture.Create<Client>();
            _unitOfWork.Setup(x => x.ClientRepository.GetBy(It.IsAny<Expression<Func<Client, bool>>>()))
                .ReturnsAsync(client);
            var handler = new GetClientHandler(_unitOfWork.Object);
            var result = handler.Handle(new GetClientQuery(1, "1"), CancellationToken.None).Result;
            Assert.That(result.Id, Is.EqualTo(client.Id));
        }
        
        [Test]
        public void GetAllClientsHandler_InputIsValid_ReturnsClients()
        {
            var clients = _fixture.CreateMany<Client>(2).ToList();
            
            _unitOfWork.Setup(x => x.ClientRepository.GetAllBy(It.IsAny<Expression<Func<Client, bool>>>(), It.IsAny<PaginationFilter>(), It.IsAny<Expression<Func<Client, bool>>>(), It.IsAny<string>()))
                .ReturnsAsync(new PagedList<Client>(clients, 1, 2, 2));
            var handler = new GetAllClientsHandler(_unitOfWork.Object);
            var result = handler.Handle(new GetAllClientsQuery(new PaginationFilter(1, 2), "Test", "1"), CancellationToken.None).Result;
            Assert.That(result.Items.Count, Is.EqualTo(2));
            Assert.That(result, Is.TypeOf<PagedList<ClientDto>>());
            
        }
        //
        [Test]
        public void DeleteClientsHandler_InputIsValid_ReturnsTrue()
        {
            var client = _fixture.Build<Client>().With(x => x.UserId, "1").Create();
            _unitOfWork.Setup(x => x.ClientRepository.GetBy(It.IsAny<Expression<Func<Client, bool>>>())).ReturnsAsync(client);
            var handler = new DeleteClientHandler(_unitOfWork.Object);
            var result = handler.Handle(new DeleteClientRequest(1, "1"), CancellationToken.None).Result;
            Assert.That(result, Is.True);
        }
        
        [Test]
        public void DeleteClientsHandler_InvalidUserId_ReturnsException()
        {
            var client = _fixture.Build<Client>().With(x => x.UserId, "5").Create();
            _unitOfWork.Setup(x => x.ClientRepository.GetBy(It.IsAny<Expression<Func<Client, bool>>>())).ReturnsAsync(client);
            var handler = new DeleteClientHandler(_unitOfWork.Object);
            async Task Code() => await handler.Handle(new DeleteClientRequest(1, "1"), CancellationToken.None);
            var ex = Assert.ThrowsAsync<UnauthorizedAccessException>(Code);
            Assert.That(ex.Message, Is.EqualTo("Insufficient Permissions"));
            
        }
        [Test]
        public void DeleteClientsHandler_InvalidClient_ReturnsFalse()
        {
            _unitOfWork.Setup(x => x.ClientRepository.GetBy(It.IsAny<Expression<Func<Client, bool>>>())).ReturnsAsync(null as Client);            
            var handler = new DeleteClientHandler(_unitOfWork.Object);
            var result = handler.Handle(new DeleteClientRequest(1, "1"), CancellationToken.None).Result;
            Assert.That(result, Is.False);
            
        }
        [Test]
        public void UpdateClientHandler_InvalidClient_ReturnsError()
        {
            _unitOfWork.Setup(x => x.ClientRepository.GetBy(It.IsAny<Expression<Func<Client, bool>>>())).ReturnsAsync(null as Client);
            var handler = new UpdateClientHandler(_unitOfWork.Object);
            async Task Code() => await handler.Handle(new UpdateClientRequest(new UpdateClientDto(), 1, "1"), CancellationToken.None);
            var ex = Assert.ThrowsAsync<Exception>(Code);
            Assert.That(ex.Message, Is.EqualTo("Client not found"));
        }
        [Test]
        public void UpdateClientHandler_InvalidUserId_ReturnsError()
        {
            var client = _fixture.Build<Client>().With(x => x.UserId, "2").Create();
            _unitOfWork.Setup(x => x.ClientRepository.GetBy(It.IsAny<Expression<Func<Client, bool>>>())).ReturnsAsync(client);
            var handler = new UpdateClientHandler(_unitOfWork.Object);
            async Task Code() => await handler.Handle(new UpdateClientRequest(new UpdateClientDto(), 1, "1"), CancellationToken.None);
            var ex = Assert.ThrowsAsync<UnauthorizedAccessException>(Code);
            Assert.That(ex.Message, Is.EqualTo("Insufficient Permissions"));
        }
        [Test]
        public void UpdateClientHandler_ValidInput_ReturnsTrue()
        {
            var client = _fixture.Build<Client>().With(x => x.UserId, "1").Create();
            _unitOfWork.Setup(x => x.ClientRepository.GetBy(It.IsAny<Expression<Func<Client, bool>>>())).ReturnsAsync(client);
            var handler = new UpdateClientHandler(_unitOfWork.Object);
            var result = handler.Handle(new UpdateClientRequest(new UpdateClientDto(), 1, "1"), CancellationToken.None);
            Assert.That(result.Result, Is.True);
        
        }
        [Test]
        public async Task CreateClientHandler_ValidInput_Success()
        {
            var client = _fixture.Build<Client>().With(x => x.UserId, "1").Create();
            var clientDto = _fixture.Build<CreateClientDto>().Create();
            _unitOfWork.Setup(x => x.ClientRepository.Insert(client)).Returns(Task.CompletedTask);
            _unitOfWork.Setup(x => x.Save()).Returns(Task.CompletedTask);
            var handler = new CreateClientHandler(_unitOfWork.Object);
            await handler.Handle(new CreateClientRequest(clientDto, "1"), CancellationToken.None);
            // TODO: consider refactoring to not use It.IsAny
            _unitOfWork.Verify(x => x.ClientRepository.Insert(It.IsAny<Client>()), Times.Once);
            _unitOfWork.Verify(x => x.Save(), Times.Once);
        
        }
        
        [Test]
        public async Task GetInvoicesForClient_ValidInput_ReturnsInvoices()
        {
            var invoices = _fixture.CreateMany<Invoice>(2).ToList();
            _unitOfWork.Setup(x => x.InvoiceRepository.GetAllBy(It.IsAny<Expression<Func<Invoice, bool>>>() , It.IsAny<PaginationFilter>(), It.IsAny<Expression<Func<Invoice, bool>>>() , It.IsAny<string>()))
                .ReturnsAsync(new PagedList<Invoice>(invoices, 1, 2, 2));
            var handler = new GetInvoicesForClientHandler(_unitOfWork.Object);
            var result = await handler.Handle(new GetInvoicesForClientQuery(1, "1", new PaginationFilter(1, 2)), CancellationToken.None);
            Assert.That(result.Items.Count, Is.EqualTo(2));
            Assert.That(result, Is.TypeOf<PagedList<InvoiceDto>>());
        }
        
        [Test]
        public async Task GetSalesOrdersForClient_ValidInput_ReturnsSalesOrders()
        {
            var salesOrders = _fixture.CreateMany<SalesOrder>(2).ToList();
            _unitOfWork.Setup(x => x.SalesOrderRepository.GetAllBy(It.IsAny<Expression<Func<SalesOrder, bool>>>() , It.IsAny<PaginationFilter>(), It.IsAny<Expression<Func<SalesOrder, bool>>>() , It.IsAny<string>()))
                .ReturnsAsync(new PagedList<SalesOrder>(salesOrders, 1, 2, 2));
            var handler = new GetSalesOrdersForClientHandler(_unitOfWork.Object);
            var result = await handler.Handle(new GetSalesOrdersForClientQuery(1, "1", new PaginationFilter(1, 2)), CancellationToken.None);
            Assert.That(result.Items.Count, Is.EqualTo(2));
            Assert.That(result, Is.TypeOf<PagedList<SalesOrderDto>>());
        }
    }
}