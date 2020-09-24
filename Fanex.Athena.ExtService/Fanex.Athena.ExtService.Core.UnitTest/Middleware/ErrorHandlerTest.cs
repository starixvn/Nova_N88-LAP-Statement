using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Fanex.Athena.ExtService.Core.Middleware;
using Fanex.Data;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace Fanex.Athena.ExtService.Core.UnitTests.Middleware
{
    public class ErrorHandlerTest
    {
        private Mock<HttpContext> _mockHttpContext;
        private Mock<IExceptionHandlerFeature> _mockHandlerException;
        private Mock<HttpResponse> _mockResponse;

        public ErrorHandlerTest()
        {
            _mockHttpContext = new Mock<HttpContext>();
            _mockResponse = new Mock<HttpResponse>();
            _mockHttpContext.Setup(x => x.Response).Returns(_mockResponse.Object);
            _mockHttpContext.Setup(x => x.Request).Returns(new Mock<HttpRequest>().Object);
            _mockHandlerException = new Mock<IExceptionHandlerFeature>();
        }

        private void SetUp()
        {
            _mockHttpContext = new Mock<HttpContext>();
            _mockResponse = new Mock<HttpResponse>();
            _mockHttpContext.Setup(x => x.Response).Returns(_mockResponse.Object);
            _mockHttpContext.Setup(x => x.Request).Returns(new Mock<HttpRequest>().Object);
            _mockHandlerException = new Mock<IExceptionHandlerFeature>();
        }

        [Fact]
        public async Task Invoke_RequestError_ReturnErrorMessageAsync()
        {
            //Arrange
            var actualContent = string.Empty;
            var actualStatus = 0;
            var actualContentType = string.Empty;
            _mockHandlerException.Setup(x => x.Error).Returns(new Exception());
            _mockHttpContext.Setup(x => x.Features[typeof(IExceptionHandlerFeature)])
                .Returns(_mockHandlerException.Object);

            _mockResponse.Setup(x => x.Body.WriteAsync(It.IsAny<byte[]>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
               .Callback<byte[], int, int, CancellationToken>((data, offset, length, cancel) =>
               {
                   if (length > 0)
                       actualContent = Encoding.UTF8.GetString(data);
               });
            _mockResponse.SetupSet(x => x.StatusCode = It.IsAny<int>()).Callback<int>(x => { actualStatus = x; });
            _mockResponse.SetupSet(x => x.ContentType = It.IsAny<string>()).Callback<string>(x => { actualContentType = x; });

            var expectedResult = JsonConvert.SerializeObject(new
            {
                State = "Internal Server Error",
                Msg = "Sorry, an error occurred while processing your request."
            });

            var errorHandler = new ErrorHandler(
                async (x) => await Task.CompletedTask,
                new Mock<ILogger<ErrorHandler>>().Object);

            //Action
            await errorHandler.Invoke(_mockHttpContext.Object).ConfigureAwait(false);

            //Assert
            Assert.Equal(500, actualStatus);
            Assert.Equal("application/json", actualContentType);
            Assert.Equal(expectedResult, actualContent);
        }

        [Fact]
        public async Task Invoke_RequestError_WriteLog()
        {
            //Arrange
            _mockResponse.SetupGet(x => x.Body).Returns(Stream.Null);
            _mockHandlerException.Setup(x => x.Error).Returns(new Exception());
            _mockHttpContext.Setup(x => x.Features[typeof(IExceptionHandlerFeature)])
                .Returns(_mockHandlerException.Object);

            var mockLogger = new Mock<ILogger<ErrorHandler>>();
            var errorHandler = new ErrorHandler(async (x) => await Task.CompletedTask, mockLogger.Object);

            //Action
            await errorHandler.Invoke(_mockHttpContext.Object).ConfigureAwait(false);

            //Assert
            mockLogger.Verify(r =>
            r.Log(LogLevel.Critical, It.IsAny<EventId>(), It.IsAny<object>(), It.IsAny<Exception>(), It.IsAny<Func<object, Exception, string>>()),
            Times.Once);
        }

        [Fact]
        public async Task Invoke_Exception_ReturnErrorMessageAsync()
        {
            //Arrange
            var actualContent = string.Empty;
            var actualStatus = 0;
            var actualContentType = string.Empty;

            _mockHttpContext.Setup(x => x.Features[typeof(IExceptionHandlerFeature)])
                .Returns(new List<IExceptionHandlerFeature>());

            _mockResponse.Setup(x => x.Body.WriteAsync(It.IsAny<byte[]>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
               .Callback<byte[], int, int, CancellationToken>((data, offset, length, cancel) =>
               {
                   if (length > 0)
                       actualContent = Encoding.UTF8.GetString(data);
               });
            _mockResponse.SetupSet(x => x.StatusCode = It.IsAny<int>()).Callback<int>(x => { actualStatus = x; });
            _mockResponse.SetupSet(x => x.ContentType = It.IsAny<string>()).Callback<string>(x => { actualContentType = x; });

            var expectedResult = JsonConvert.SerializeObject(new
            {
                State = "Internal Server Error",
                Msg = "Sorry, an error occurred while processing your request."
            });

            var errorHandler = new ErrorHandler(async (x) =>
            {
                await Task.CompletedTask;
                throw new Exception();
            }, new Mock<ILogger<ErrorHandler>>().Object);

            //Action
            await errorHandler.Invoke(_mockHttpContext.Object).ConfigureAwait(false);

            //Assert

            Assert.Equal(500, actualStatus);
            Assert.Equal("application/json", actualContentType);
            Assert.Equal(expectedResult, actualContent);
        }

        [Fact]
        public async Task Invoke_Exception_WriteLog()
        {
            //Arrange
            _mockResponse.SetupGet(x => x.Body).Returns(Stream.Null);
            _mockHttpContext.Setup(x => x.Features[typeof(IExceptionHandlerFeature)])
                .Returns(new List<IExceptionHandlerFeature>());

            var mockLogger = new Mock<ILogger<ErrorHandler>>();

            var errorHandler = new ErrorHandler(async (x) =>
            {
                await Task.CompletedTask;
                throw new Exception();
            }, mockLogger.Object);

            //Action
            await errorHandler.Invoke(_mockHttpContext.Object).ConfigureAwait(false);

            //Assert
            mockLogger.Verify(r =>
            r.Log(LogLevel.Critical, It.IsAny<EventId>(), It.IsAny<object>(), It.IsAny<Exception>(), It.IsAny<Func<object, Exception, string>>()),
            Times.Once);
        }

        [Fact]
        public async Task Invoke_SqlException_WriteLog()
        {
            //Arrange
            _mockResponse.SetupGet(x => x.Body).Returns(Stream.Null);
            _mockHttpContext.Setup(x => x.Features[typeof(IExceptionHandlerFeature)])
                .Returns(new List<IExceptionHandlerFeature>());

            var mockLogger = new Mock<ILogger<ErrorHandler>>();
            var exception = FormatterServices.GetUninitializedObject(typeof(SqlException))
                as SqlException;

            var errorHandler = new ErrorHandler(async (x) =>
            {
                await Task.CompletedTask;
                throw (exception);
            }, mockLogger.Object);

            //Action
            await errorHandler.Invoke(_mockHttpContext.Object).ConfigureAwait(false);

            //Assert
            mockLogger.Verify(r =>
            r.Log(LogLevel.Critical, It.IsAny<EventId>(), It.IsAny<object>(), It.IsAny<Exception>(), It.IsAny<Func<object, Exception, string>>()),
            Times.Once);
        }

        [Fact]
        public async Task Invoke_FanexDataException_WriteLog()
        {
            //Arrange
            _mockResponse.SetupGet(x => x.Body).Returns(Stream.Null);
            _mockHttpContext.Setup(x => x.Features[typeof(IExceptionHandlerFeature)])
                .Returns(new List<IExceptionHandlerFeature>());

            var mockLogger = new Mock<ILogger<ErrorHandler>>();
            var exception = FormatterServices.GetUninitializedObject(typeof(ObjectDbException))
                as ObjectDbException;

            var errorHandler = new ErrorHandler(async (x) =>
            {
                await Task.CompletedTask;
                throw (exception);
            }, mockLogger.Object);

            //Action
            await errorHandler.Invoke(_mockHttpContext.Object).ConfigureAwait(false);

            //Assert
            mockLogger.Verify(r =>
            r.Log(LogLevel.Critical, It.IsAny<EventId>(), It.IsAny<object>(), It.IsAny<Exception>(), It.IsAny<Func<object, Exception, string>>()),
            Times.Once);
        }
    }
}