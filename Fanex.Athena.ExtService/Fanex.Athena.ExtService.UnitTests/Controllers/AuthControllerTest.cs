using System;
using Fanex.Athena.ExtService.Controllers;
using Fanex.Athena.ExtService.Core.Models;
using Fanex.Athena.ExtService.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Fanex.Athena.ExtService.UnitTests.Controllers
{
    public class AuthControllerTest
    {
        private Mock<IAuthenticateService> _mockAuthService;

        [Fact]
        public void RequestToken_InvalidModel_ReturnBadRequest()
        {
            // Arrange
            var tokenRequest = new TokenRequest();
            _mockAuthService = new Mock<IAuthenticateService>();

            var controller = new AuthController(_mockAuthService.Object);
            controller.ModelState.AddModelError("Error1", "Invalid User name");

            // Action
            var result = controller.RequestToken(tokenRequest) as BadRequestObjectResult;

            // Assert
            Assert.Equal("Invalid Request", result.Value.ToString());
            _mockAuthService.Verify(x => x.IsAuthenticated(It.IsAny<TokenRequest>()), Times.Never);
        }

        [Fact]
        public void RequestToken_ValidModelAndAuthenticated_ReturnOk()
        {
            // Arrange
            var tokenRequest = new TokenRequest();
            var expectedToken = new Token();
            var expectedResult = Tuple.Create(true, expectedToken);
            _mockAuthService = new Mock<IAuthenticateService>();
            _mockAuthService.Setup(s => s.IsAuthenticated(tokenRequest)).Returns(expectedResult);

            var controller = new AuthController(_mockAuthService.Object);

            // Action
            var result = controller.RequestToken(tokenRequest) as OkObjectResult;

            // Assert
            Assert.Equal(expectedToken, result.Value as Token);
        }

        [Fact]
        public void RequestToken_ValidModelButNotAuthenticated_ReturnBadRequest()
        {
            // Arrange
            var tokenRequest = new TokenRequest();
            var expectedResult = Tuple.Create(false, new Token());
            _mockAuthService = new Mock<IAuthenticateService>();
            _mockAuthService.Setup(s => s.IsAuthenticated(tokenRequest)).Returns(expectedResult);

            var controller = new AuthController(_mockAuthService.Object);

            // Action
            var result = controller.RequestToken(tokenRequest) as BadRequestObjectResult;

            // Assert
            Assert.Equal("Wrong username or password", result.Value.ToString());
        }
    }
}