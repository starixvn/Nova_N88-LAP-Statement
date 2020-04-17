using System;
using System.Collections.Generic;
using System.Net;
using Fanex.Athena.ExtService.Core.Attributes;
using Fanex.Athena.ExtService.Core.Models.DTO;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;

using Moq;

using Xunit;

namespace Fanex.Athena.ExtService.Core.UnitTests.Attributes
{
    public class IPFilterAttributeTest
    {
        private readonly Mock<IOptions<ApplicationOptions>> _options;
        private readonly ActionExecutingContext _actionExecutingContext;
        private readonly Mock<HttpContext> _mockHttpContext;
        public IPFilterAttributeTest()
        {
            var modelState = new ModelStateDictionary();
            _mockHttpContext = new Mock<HttpContext>();

            var actionContext = new ActionContext(
                  _mockHttpContext.Object,
                  Mock.Of<RouteData>(),
                  Mock.Of<ActionDescriptor>(),
                  modelState
              );
            _actionExecutingContext = new ActionExecutingContext(
           actionContext,
           new List<IFilterMetadata>(),
           new Dictionary<string, object>(),
           Mock.Of<Controller>());
            _options = new Mock<IOptions<ApplicationOptions>>();
        }

        [Fact]
        public void OnActionExecuting_WhenEmptyWhiteListIP_NoChangeResult()
        {
            //Arrange
            var fakeContent = "NOCHANGE";
            var fakeResult = new ContentResult { Content = fakeContent };
            _options.Setup(x => x.Value).Returns(new ApplicationOptions { WhiteList = new List<string>() });
            _actionExecutingContext.Result = fakeResult;
            var connectionInfo = new Mock<ConnectionInfo>();
            connectionInfo.Setup(x => x.RemoteIpAddress).Returns(new IPAddress(0x2414188f));
            _mockHttpContext.Setup(x => x.Connection).Returns(connectionInfo.Object);
            //Action
            var filter = new IPFilterAttribute(_options.Object);

            filter.OnActionExecuting(_actionExecutingContext);

            //Assert
            var actualResult = Assert.IsAssignableFrom<ContentResult>(_actionExecutingContext.Result);

            Assert.Equal(fakeContent, actualResult.Content);
        }

        [Fact]
        public void OnActionExecuting_WhenAllowIP_WhiteListSingle_NoChangeResult()
        {
            //Arrange
            const int requestIP = 0x2414188f;
            const string WhitelistIP = "143.24.20.36";

            const string fakeContent = "NOCHANGE";
            var fakeResult = new ContentResult { Content = fakeContent };
            _options.Setup(x => x.Value).Returns(new ApplicationOptions { WhiteList = new List<string> { WhitelistIP } });
            _actionExecutingContext.Result = fakeResult;
            var connectionInfo = new Mock<ConnectionInfo>();
            connectionInfo.Setup(x => x.RemoteIpAddress).Returns(new IPAddress(requestIP));
            _mockHttpContext.Setup(x => x.Connection).Returns(connectionInfo.Object);
            //Action
            var filter = new IPFilterAttribute(_options.Object);

            filter.OnActionExecuting(_actionExecutingContext);

            //Assert
            var actualResult = Assert.IsAssignableFrom<ContentResult>(_actionExecutingContext.Result);

            Assert.Equal(fakeContent, actualResult.Content);
        }

        [Fact]
        public void OnActionExecuting_WhenNotAllowIP_WhiteListSingle_ReturnResultWithDeniedMessage()
        {
            //Arrange
            const long requestIPNumber = 169003217;
            const string requestIP = "209.200.18.10";
            const string WhitelistIP = "143.24.20.36";

            var expectedResult = $"The IP Address: {requestIP} was blocked";
            _options.Setup(x => x.Value).Returns(new ApplicationOptions { WhiteList = new List<string> { WhitelistIP } });

            var connectionInfo = new Mock<ConnectionInfo>();
            connectionInfo.Setup(x => x.RemoteIpAddress).Returns(new IPAddress(requestIPNumber));

            _mockHttpContext.Setup(x => x.Connection).Returns(connectionInfo.Object);


            //Action
            var filter = new IPFilterAttribute(_options.Object);

            filter.OnActionExecuting(_actionExecutingContext);

            //Assert
            var actualResult = Assert.IsAssignableFrom<ContentResult>(_actionExecutingContext.Result);

            Assert.Equal(expectedResult, actualResult.Content);
        }

        [Theory]
        [InlineData("119.81.247.201")]
        [InlineData("119.81.247.202")]
        [InlineData("119.81.247.203")]
        [InlineData("119.81.247.204")]
        [InlineData("119.81.247.205")]
        [InlineData("119.81.247.206")]
        [InlineData("119.81.247.207")]
        public void OnActionExecuting_WhenAllowIP_WhiteListRange_NoChangeResult(string requestIP)
        {
            //Arrange
            const string WhitelistIP = "119.81.247.200/29";

            const string fakeContent = "NOCHANGE";
            var fakeResult = new ContentResult { Content = fakeContent };
            _options.Setup(x => x.Value).Returns(new ApplicationOptions { WhiteList = new List<string> { WhitelistIP } });
            _actionExecutingContext.Result = fakeResult;
            var connectionInfo = new Mock<ConnectionInfo>();
            connectionInfo.Setup(x => x.RemoteIpAddress).Returns(IPAddress.Parse(requestIP));
            _mockHttpContext.Setup(x => x.Connection).Returns(connectionInfo.Object);
            //Action
            var filter = new IPFilterAttribute(_options.Object);

            filter.OnActionExecuting(_actionExecutingContext);

            //Assert
            var actualResult = Assert.IsAssignableFrom<ContentResult>(_actionExecutingContext.Result);

            Assert.Equal(fakeContent, actualResult.Content);
        }

        [Theory]
        [InlineData("119.81.247.199")]
        [InlineData("119.81.247.208")]
        public void OnActionExecuting_WhenNotAllowIP_WhiteListRange_ReturnResultWithDeniedMessage(string requestIP)
        {
            //Arrange
            const string WhitelistIP = "119.81.247.200/29";

            var expectedResult = $"The IP Address: {requestIP} was blocked";
            _options.Setup(x => x.Value).Returns(new ApplicationOptions { WhiteList = new List<string> { WhitelistIP } });

            var connectionInfo = new Mock<ConnectionInfo>();
            connectionInfo.Setup(x => x.RemoteIpAddress).Returns(IPAddress.Parse(requestIP));

            _mockHttpContext.Setup(x => x.Connection).Returns(connectionInfo.Object);


            //Action
            var filter = new IPFilterAttribute(_options.Object);

            filter.OnActionExecuting(_actionExecutingContext);

            //Assert
            var actualResult = Assert.IsAssignableFrom<ContentResult>(_actionExecutingContext.Result);

            Assert.Equal(expectedResult, actualResult.Content);
        }

        [Theory]
        [InlineData("119.81.247.256")]
        [InlineData("119.81.247.200/100")]
        public void OnActionExecuting_WhiteListIPInvalid_ThrowExceptionWithMessage(string whiteListIP)
        {
            //Arrange
            const string requestIP = "119.81.247.200";

            var expectedResult = $"The WhiteList IP: {whiteListIP} was invalid - client IP: ${requestIP}";
            _options.Setup(x => x.Value).Returns(new ApplicationOptions { WhiteList = new List<string> { whiteListIP } });

            var connectionInfo = new Mock<ConnectionInfo>();
            connectionInfo.Setup(x => x.RemoteIpAddress).Returns(IPAddress.Parse(requestIP));

            _mockHttpContext.Setup(x => x.Connection).Returns(connectionInfo.Object);

            //Action
            var filter = new IPFilterAttribute(_options.Object);

            var ex = Assert.Throws<Exception>(() => filter.OnActionExecuting(_actionExecutingContext));

            //Assert
            Assert.Equal(expectedResult, ex.Message);
        }
    }
}
