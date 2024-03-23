
using BusinessManagement.Commands;
using BusinessManagement.Handlers;
using BusinessManagement.Queries;
using BusinessManagementApi.DAL;
using BusinessManagementApi.Dto;
using BusinessManagementApi.Models;

namespace BusinessManagement.UnitTests.Handlers
{
    [TestFixture]
    public class BusinessInfoHandlers
    {
        private Mock<IBusinessInfoRepository> _businessInfoRepository;
        private Fixture _fixture;

        [SetUp]
        public void SetUp()
        {
            _businessInfoRepository = new Mock<IBusinessInfoRepository>();
            _fixture = new Fixture();

        }

        [Test]
        public void GetBusinessInfoHandler_InputIsValid_ReturnsBusinessInfo()
        {
            var businessInfo = _fixture.Create<BusinessInfo>();
            _businessInfoRepository.Setup(x => x.GetBusinessUserByUserId("1")).ReturnsAsync(businessInfo);
            var handler = new GetBusinessInfoHandler(_businessInfoRepository.Object);
            var result = handler.Handle(new GetBusinessInfoQuery("1"), CancellationToken.None).Result;
            Assert.That(result.Id, Is.EqualTo(businessInfo.Id));
        }
        
        
        [Test]
        public void UpdateBusinessInfoHandler_InvalidBusinessInfo_ReturnsError()
        {
            _businessInfoRepository.Setup(x => x.GetBusinessUserByUserId("1")).ReturnsAsync(null as BusinessInfo);
            var handler = new UpdateBusinessInfoHandler(_businessInfoRepository.Object);
            async Task Code() => await handler.Handle(new UpdateBusinessInfoRequest(new UpdateBusinessInfoDto(),"1"), CancellationToken.None);
            var ex = Assert.ThrowsAsync<Exception>(Code);
            Assert.That(ex.Message, Is.EqualTo("Business Info not found"));
        }
        [Test]
        public void UpdateBusinessInfoHandler_ValidInput_ReturnsTrue()
        {
            var businessInfo = _fixture.Build<BusinessInfo>().With(x => x.UserId, "1").Create();
            _businessInfoRepository.Setup(x => x.GetBusinessUserByUserId("1")).ReturnsAsync(businessInfo);
            var handler = new UpdateBusinessInfoHandler(_businessInfoRepository.Object);
            var result = handler.Handle(new UpdateBusinessInfoRequest(new UpdateBusinessInfoDto(),"1"), CancellationToken.None);
            Assert.That(result.Result, Is.True);
        
        }
        [Test]
        public async Task CreateBusinessInfoHandler_ValidInput_Success()
        {
            var businessInfo = _fixture.Build<BusinessInfo>().With(x => x.UserId, "1").Create();
            var businessInfoDto = _fixture.Build<CreateBusinessInfoDto>().Create();
            _businessInfoRepository.Setup(x => x.InsertBusinessInfo(businessInfo)).Returns(Task.CompletedTask);
            _businessInfoRepository.Setup(x => x.Save()).Returns(Task.CompletedTask);
            var handler = new CreateBusinessInfoHandler(_businessInfoRepository.Object);
            await handler.Handle(new CreateBusinessInfoRequest(businessInfoDto, "1"), CancellationToken.None);
            // TODO: consider refactoring to not use It.IsAny
            _businessInfoRepository.Verify(x => x.InsertBusinessInfo(It.IsAny<BusinessInfo>()), Times.Once);
            _businessInfoRepository.Verify(x => x.Save(), Times.Once);
        
        }
    }
}