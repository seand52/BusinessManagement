
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
    public class ClientHandlers
    {
        private Mock<IClientRepository> _clientRepository;
        private Fixture _fixture;

        [SetUp]
        public void SetUp()
        {
            _clientRepository = new Mock<IClientRepository>();
            _fixture = new Fixture();

        }

        [Test]
        public void GetClientHandler_InputIsValid_ReturnsClient()
        {
            var client = _fixture.Create<Client>();
            _clientRepository.Setup(x => x.GetClientById(1, "1")).ReturnsAsync(client);
            var handler = new GetClientHandler(_clientRepository.Object);
            var result = handler.Handle(new GetClientQuery(1, "1"), CancellationToken.None).Result;
            Assert.That(result.Id, Is.EqualTo(client.Id));
        }
        
        [Test]
        public void GetAllClientsHandler_InputIsValid_ReturnsClients()
        {
            var clients = _fixture.CreateMany<Client>(2).ToList();
            _clientRepository.Setup(x => x.GetClients(It.IsAny<PaginationFilter>(), "Test", "1")).ReturnsAsync(new PagedList<Client>(clients, 1, 2, 2));
            var handler = new GetAllClientsHandler(_clientRepository.Object);
            var result = handler.Handle(new GetAllClientsQuery(new PaginationFilter(1, 2), "Test", "1"), CancellationToken.None).Result;
            Assert.That(result.Items.Count, Is.EqualTo(2));
            Assert.That(result, Is.TypeOf<PagedList<Client>>());
            
        }
        
        [Test]
        public void DeleteClientsHandler_InputIsValid_ReturnsTrue()
        {
            var client = _fixture.Build<Client>().With(x => x.UserId, "1").Create();
            _clientRepository.Setup(x => x.GetClientById(1, "1")).ReturnsAsync(client);
            var handler = new DeleteClientHandler(_clientRepository.Object);
            var result = handler.Handle(new DeleteClientRequest(1, "1"), CancellationToken.None).Result;
            Assert.That(result, Is.True);
        }
        
        [Test]
        public void DeleteClientsHandler_InvalidUserId_ReturnsException()
        {
            var client = _fixture.Build<Client>().With(x => x.UserId, "5").Create();
            _clientRepository.Setup(x => x.GetClientById(1, "1")).ReturnsAsync(client);
            var handler = new DeleteClientHandler(_clientRepository.Object);
            async Task Code() => await handler.Handle(new DeleteClientRequest(1, "1"), CancellationToken.None);
            var ex = Assert.ThrowsAsync<UnauthorizedAccessException>(Code);
            Assert.That(ex.Message, Is.EqualTo("Insufficient Permissions"));
            
        }
        [Test]
        public void DeleteClientsHandler_InvalidClient_ReturnsFalse()
        {
            _clientRepository.Setup(x => x.GetClientById(1, "1")).ReturnsAsync(null as Client);
            var handler = new DeleteClientHandler(_clientRepository.Object);
            var result = handler.Handle(new DeleteClientRequest(1, "1"), CancellationToken.None).Result;
            Assert.That(result, Is.False);
            
        }
        [Test]
        public void UpdateClientHandler_InvalidClient_ReturnsError()
        {
            _clientRepository.Setup(x => x.GetClientById(1, "1")).ReturnsAsync(null as Client);
            var handler = new UpdateClientHandler(_clientRepository.Object);
            async Task Code() => await handler.Handle(new UpdateClientRequest(new UpdateClientDto(), 1, "1"), CancellationToken.None);
            var ex = Assert.ThrowsAsync<Exception>(Code);
            Assert.That(ex.Message, Is.EqualTo("Client not found"));
        }
        [Test]
        public void UpdateClientHandler_InvalidUserId_ReturnsError()
        {
            var client = _fixture.Build<Client>().With(x => x.UserId, "2").Create();
            _clientRepository.Setup(x => x.GetClientById(1, "1")).ReturnsAsync(client);
            var handler = new UpdateClientHandler(_clientRepository.Object);
            async Task Code() => await handler.Handle(new UpdateClientRequest(new UpdateClientDto(), 1, "1"), CancellationToken.None);
            var ex = Assert.ThrowsAsync<UnauthorizedAccessException>(Code);
            Assert.That(ex.Message, Is.EqualTo("Insufficient Permissions"));
        }
        [Test]
        public void UpdateClientHandler_ValidInput_ReturnsTrue()
        {
            var client = _fixture.Build<Client>().With(x => x.UserId, "1").Create();
            _clientRepository.Setup(x => x.GetClientById(1, "1")).ReturnsAsync(client);
            var handler = new UpdateClientHandler(_clientRepository.Object);
            var result = handler.Handle(new UpdateClientRequest(new UpdateClientDto(), 1, "1"), CancellationToken.None);
            Assert.That(result.Result, Is.True);
        
        }
        [Test]
        public async Task CreateClientHandler_ValidInput_Success()
        {
            var client = _fixture.Build<Client>().With(x => x.UserId, "1").Create();
            var clientDto = _fixture.Build<CreateClientDto>().Create();
            _clientRepository.Setup(x => x.InsertClient(client)).Returns(Task.CompletedTask);
            _clientRepository.Setup(x => x.Save()).Returns(Task.CompletedTask);
            var handler = new CreateClientHandler(_clientRepository.Object);
            await handler.Handle(new CreateClientRequest(clientDto, "1"), CancellationToken.None);
            // TODO: consider refactoring to not use It.IsAny
            _clientRepository.Verify(x => x.InsertClient(It.IsAny<Client>()), Times.Once);
            _clientRepository.Verify(x => x.Save(), Times.Once);
        
        }
    }
}