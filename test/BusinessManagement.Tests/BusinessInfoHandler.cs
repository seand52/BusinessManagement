
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
    public class BusinessInfoHandlers
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
        public void GetBusinessInfoHandler_InputIsValid_ReturnsBusinessInfo()
        {
            var businessInfo = _fixture.Create<BusinessInfo>();
            _unitOfWork.Setup(x => x.BusinessInfoRepository.GetBy(It.IsAny<Expression<Func<BusinessInfo, bool>>>()))
                .ReturnsAsync(businessInfo);
            var handler = new GetBusinessInfoHandler(_unitOfWork.Object);
            var result = handler.Handle(new GetBusinessInfoQuery("1"), CancellationToken.None).Result;
            Assert.That(result.Id, Is.EqualTo(businessInfo.Id));
        }
        
        [Test]
        public void UpdateBusinessInfoHandler_InvalidBusinessInfo_ReturnsError()
        {
            _unitOfWork.Setup(x => x.BusinessInfoRepository.GetBy(It.IsAny<Expression<Func<BusinessInfo, bool>>>())).ReturnsAsync(null as BusinessInfo);
            var handler = new UpdateBusinessInfoHandler(_unitOfWork.Object);
            async Task Code() => await handler.Handle(new UpdateBusinessInfoRequest(new UpdateBusinessInfoDto(), "1"), CancellationToken.None);
            var ex = Assert.ThrowsAsync<Exception>(Code);
            Assert.That(ex.Message, Is.EqualTo("Business Info not found"));
        }

        [Test]
        public void UpdateBusinessInfoHandler_ValidInput_ReturnsTrue()
        {
            var businessInfo = _fixture.Build<BusinessInfo>().With(x => x.UserId, "1").Create();
            _unitOfWork.Setup(x => x.BusinessInfoRepository.GetBy(It.IsAny<Expression<Func<BusinessInfo, bool>>>())).ReturnsAsync(businessInfo);
            var handler = new UpdateBusinessInfoHandler(_unitOfWork.Object);
            var result = handler.Handle(new UpdateBusinessInfoRequest(new UpdateBusinessInfoDto(), "1"), CancellationToken.None);
            Assert.That(result.Result, Is.True);
        
        }
        [Test]
        public async Task CreateBusinessInfoHandler_ValidInput_Success()
        {
            var businessInfo = _fixture.Build<BusinessInfo>().With(x => x.UserId, "1").Create();
            var businessInfoDto = _fixture.Build<CreateBusinessInfoDto>().Create();
            _unitOfWork.Setup(x => x.BusinessInfoRepository.Insert(businessInfo)).Returns(Task.CompletedTask);
            _unitOfWork.Setup(x => x.Save()).Returns(Task.CompletedTask);
            var handler = new CreateBusinessInfoHandler(_unitOfWork.Object);
            await handler.Handle(new CreateBusinessInfoRequest(businessInfoDto, "1"), CancellationToken.None);
            // TODO: consider refactoring to not use It.IsAny
            _unitOfWork.Verify(x => x.BusinessInfoRepository.Insert(It.IsAny<BusinessInfo>()), Times.Once);
            _unitOfWork.Verify(x => x.Save(), Times.Once);
        
        }
    }
}