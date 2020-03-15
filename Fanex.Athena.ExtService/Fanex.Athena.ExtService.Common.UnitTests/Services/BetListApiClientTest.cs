using Fanex.Athena.ExtService.Common.Models;
using Fanex.Athena.ExtService.Common.Services;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Fanex.Athena.ExtService.Common.UnitTests.Services
{
    public class BetListApiClientTest : IDisposable
    {
        private Mock<IHttpClientFactory> _mockClientFactory;
        private Mock<HttpClient> _mockClient;
        private Mock<HttpMessageHandler> _fakeHttpMessageHandler;
        public BetListApiClientTest()
        {
            _fakeHttpMessageHandler = new Mock<HttpMessageHandler>(MockBehavior.Loose);
            _mockClientFactory = new Mock<IHttpClientFactory>();
            _mockClient = new Mock<HttpClient>(_fakeHttpMessageHandler.Object);
            _mockClientFactory.SetReturnsDefault(_mockClient.Object);
            _mockClientFactory.Setup(r => r.CreateClient(It.IsAny<string>())).Returns(_mockClient.Object);
            _mockClient.Object.BaseAddress = new Uri("http://test.com");
        }

        [Fact]
        public async Task HandleAsync_WhenCallSuccess_ReturnResult()
        {
            //Arrange
            const string language = "en-US";
            const string FakeContent = "ANY";

            using (var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(FakeContent) })
            {
                FakeSendAsyncMethod(httpResponseMessage);

                var apiClient = new BetListApiClient("any", _mockClientFactory.Object);

                //Action
                var result = await apiClient.PostAsyncString("/test", "test-data", language).ConfigureAwait(false);

                //Assert
                Assert.Equal(FakeContent, result);
            }
        }

        private void FakeSendAsyncMethod(HttpResponseMessage httpResponseMessage)
        {
            _fakeHttpMessageHandler.Protected()
           // Setup the PROTECTED method to mock
           .Setup<Task<HttpResponseMessage>>(
              "SendAsync",
              ItExpr.IsAny<HttpRequestMessage>(),
              ItExpr.IsAny<CancellationToken>()
           )
           // prepare the expected response of the mocked http call
           .ReturnsAsync(httpResponseMessage).Verifiable();
        }

        [Fact]
        public async Task HandleAsync_RequestMessage_ConfigCorrectly()
        {
            //Arrange
            const string BetListApiClientKey = "test-bet-list-api-client";
            var expectedUrl = new Uri("http://test/base-address");
            var requestContent = new { data = "test" };
            using (var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("any") })
            {
                _mockClient.Object.BaseAddress = expectedUrl;

                FakeSendAsyncMethod(httpResponseMessage);

                var apiClient = new BetListApiClient(BetListApiClientKey, _mockClientFactory.Object);

                //Action
                var result = await apiClient.PostAsyncString("/test-path", requestContent, "en-US").ConfigureAwait(false);

                //Assert
                _fakeHttpMessageHandler.Protected().Verify(
                    "SendAsync",
                    Times.Exactly(1),
                    ItExpr.Is<HttpRequestMessage>(req =>

                         req != null
                         && req.Method == HttpMethod.Post
                         && req.RequestUri == new Uri(expectedUrl + "/test-path")
                         && req.Content != null
                    ),
                    ItExpr.IsAny<CancellationToken>()
                 );
            }
        }

        [Fact]
        public async Task PostAsync_WhenCall_ReturnCorrectDataTypeAsync()
        {
            //Arrange
            const string language = "en-US";

            using (var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("{\"ID\":1,\"Name\":\"alex\"}")
            })
            {
                FakeSendAsyncMethod(httpResponseMessage);

                var apiClient = new BetListApiClient("any", _mockClientFactory.Object);

                //Action
                var result = await apiClient.PostAsync<dynamic>("/test", new { data = "test" }, language).ConfigureAwait(false);

                //Assert
                Assert.IsAssignableFrom<JObject>(result);
                Assert.Equal(
                    JsonConvert.SerializeObject(new { ID = 1, Name = "alex" }),
                    JsonConvert.SerializeObject(result));
            }
        }

        public void Dispose()
        {
            _mockClient?.Object?.Dispose();
            _mockClient = null;
            _mockClientFactory = null;
            _fakeHttpMessageHandler?.Object?.Dispose();
            _fakeHttpMessageHandler = null;
            GC.SuppressFinalize(this);
        }
    }
}