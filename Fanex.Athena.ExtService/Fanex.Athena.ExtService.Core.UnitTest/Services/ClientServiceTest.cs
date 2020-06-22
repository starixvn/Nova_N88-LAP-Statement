using Fanex.Athena.ExtService.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Moq;
using System.Net;
using Xunit;

namespace Fanex.Athena.ExtService.Core.UnitTests.Services
{
    public class ClientServiceTest
    {
        private readonly ClientService _target;
        private readonly Mock<IHttpContextAccessor> _mockContextAccessor;
        private readonly Mock<HttpContext> _mockHttpContext;
        private readonly Mock<ConnectionInfo> _mockConnection;

        public ClientServiceTest()
        {
            _mockContextAccessor = new Mock<IHttpContextAccessor>();
            _mockHttpContext = new Mock<HttpContext>();
            _mockConnection = new Mock<ConnectionInfo>();

            _mockHttpContext.Setup(x => x.Connection).Returns(_mockConnection.Object);
            _mockContextAccessor.Setup(_ => _.HttpContext).Returns(_mockHttpContext.Object);

            _target = new ClientService(_mockContextAccessor.Object);
        }

        [Fact]
        public void RealClientIP_HeaderIsEmpty_ReturnResultFromConnectionInfo()
        {
            //Arrange
            const string requestIP = "209.200.18.10";

            var mockRequest = new Mock<HttpRequest>();
            var mockHeaders = new Mock<IHeaderDictionary>();

            mockHeaders.Setup(_ => _.TryGetValue("X-Forwarded-For", out It.Ref<StringValues>.IsAny)).Returns(false);
            mockRequest.Setup(_ => _.Headers).Returns(mockHeaders.Object);

            _mockHttpContext.Setup(_ => _.Request).Returns(mockRequest.Object);
            _mockConnection.Setup(_ => _.RemoteIpAddress).Returns(IPAddress.Parse(requestIP));

            //Act
            var expectedIp = _target.RealClientIP;

            //Assert
            Assert.Equal(expectedIp.ToString(), requestIP);
        }

        [Fact]
        public void RealClientIP_HeaderHasValue_ReturnResultFromForwardedHeader()
        {
            //Arrange
            const string requestIP = "209.200.18.10";

            var mockRequest = new Mock<HttpRequest>();
            var mockHeaders = new Mock<IHeaderDictionary>();

            var expectedValue = new StringValues(new[] { requestIP });

            mockHeaders.Setup(_ => _.TryGetValue("X-Forwarded-For", out expectedValue)).Returns(true);
            mockRequest.Setup(_ => _.Headers).Returns(mockHeaders.Object);

            _mockHttpContext.Setup(_ => _.Request).Returns(mockRequest.Object);
            _mockConnection.Setup(_ => _.RemoteIpAddress).Returns(IPAddress.Parse(requestIP));

            //Act
            var expectedIp = _target.RealClientIP;

            //Assert
            Assert.Equal(expectedIp.ToString(), requestIP);
        }
    }
}