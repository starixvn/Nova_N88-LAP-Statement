using System.Dynamic;
using System.Threading.Tasks;
using AutoFixture;
using Fanex.Athena.ExtService.BetList.Controllers;
using Fanex.Athena.ExtService.BetList.Models.MetaModel;
using Fanex.Athena.ExtService.Common.Models.ViewModels;

using Microsoft.AspNetCore.Mvc;

using Moq;

using Xunit;

namespace Fanex.Athena.ExtService.BetList.UnitTests.Controllers
{
    public class ResultReportControllerTest : BaseControllerTest
    {
        private static readonly Fixture _fixture = new Fixture();

        [Fact]
        public async Task ResultPopup_WhenCall_RetrunCorrectViewModelAsync()
        {
            //Arrange
            const string Title = "Result";
            var metalModel = _fixture.Create<ResultPopupMetalModel>();
            var fakeResult = "Any result data";
            IActionResult result = null;
            _betListApiClient.Setup(x => x.PostAsyncString(
                "/ResultReport/ResultPopup", It.IsAny<ResultPopupMetalModel>(),
                It.IsAny<string>()))
                .Returns(Task.FromResult(fakeResult));

            //Action
            using (var controller = new ResultReportController(_betListApiClient.Object))
            {
                controller.ControllerContext = _controllerContext.Object;
                result = await controller.ResultPopup(metalModel).ConfigureAwait(false);
            }

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var viewModel = Assert.IsType<BaseViewModel>(viewResult.Model);

            var title = Assert.IsAssignableFrom<string>(viewResult.ViewData["Title"]);
            var actualResult = Assert.IsAssignableFrom<string>(viewResult.ViewData["Data"]);
            Assert.Equal(fakeResult, actualResult);
            Assert.Equal(title, Title);
            Assert.Equal(viewModel.Lang, metalModel.Lang);
            Assert.Equal(viewModel.AccessToken, AccessToken);
            _betListApiClient.Verify(x => 
            x.PostAsyncString("/ResultReport/ResultPopup", metalModel, 
            metalModel.Lang), Times.Once);
        }

        [Fact]
        public async Task ShowRngCasinoSingleWalletResult_WhenCall_RetrunCorrectJsonResult()
        {
            //Arrange
            const string refNo = "Any";
            const string lang = "en-US";
            var fakeResult = "Any result data";
            IActionResult result = null;
            _betListApiClient.Setup(x => x.PostAsyncString(
                "/ResultReport/ShowRngCasinoSingleWalletResult", It.IsAny<object>(),
                It.IsAny<string>()))
                .Returns(Task.FromResult(fakeResult));

            //Action
            using (var controller = new ResultReportController(_betListApiClient.Object))
            {
                dynamic input = new ExpandoObject();
                input.refNo = refNo;
                input.lang = lang;
                controller.ControllerContext = _controllerContext.Object;
                result = await controller.ShowRngCasinoSingleWalletResult(input).ConfigureAwait(false);
            }

            //Assert
            var viewResult = Assert.IsType<ContentResult>(result);
            var actualResult = Assert.IsAssignableFrom<string>(viewResult.Content);
            Assert.Equal("application/json", viewResult.ContentType);
            Assert.Equal(fakeResult, actualResult);
        }

        [Fact]
        public async Task VoidbridgeResult_WhenCall_RetrunStringResult()
        {
            //Arrange
            const string custId = "Any";
            const string lang = "en-US";
            const string gamePlayId = "Any";
            var fakeResult = "Any result data";
            string result = null;
            _betListApiClient.Setup(x => x.PostAsyncString(
                "/VoidbridgeResultPopup/Index", It.IsAny<object>(),
                It.IsAny<string>()))
                .Returns(Task.FromResult(fakeResult));

            //Action
            using (var controller = new ResultReportController(_betListApiClient.Object))
            {
                controller.ControllerContext = _controllerContext.Object;
                result = await controller.VoidbridgeResult(custId, gamePlayId, lang).ConfigureAwait(false);
            }

            //Assert
            var viewResult = Assert.IsType<string>(result);
            Assert.Equal(fakeResult, viewResult);
        }

        [Fact]
        public async Task PPResultPopup_WhenCall_RetrunStringResult()
        {
            //Arrange
            const string custId = "Any";
            const string lang = "en-US";
            const string gamePlayId = "Any";
            const string fakeResult = "Any result data";
            string result = null;
            _betListApiClient.Setup(x => x.PostAsyncString(
                "/PPResultPopup/Index", It.IsAny<object>(),
                It.IsAny<string>()))
                .Returns(Task.FromResult(fakeResult));

            //Action
            using (var controller = new ResultReportController(_betListApiClient.Object))
            {
                controller.ControllerContext = _controllerContext.Object;
                result = await controller.PPResultPopup(custId, gamePlayId, lang).ConfigureAwait(false);
            }

            //Assert
            var viewResult = Assert.IsType<string>(result);
            Assert.Equal(fakeResult, viewResult);
        }

        [Fact]
        public async Task SGResultPopup_WhenCall_RetrunStringResult()
        {
            //Arrange
            const string custId = "Any";
            const string lang = "en-US";
            const string gamePlayId = "Any SG";
            const string fakeResult = "Any SG result data";
            string result = null;
            _betListApiClient.Setup(x => x.PostAsyncString(
                "/SGResultPopup/Index", It.IsAny<object>(),
                It.IsAny<string>()))
                .Returns(Task.FromResult(fakeResult));

            //Action
            using (var controller = new ResultReportController(_betListApiClient.Object))
            {
                controller.ControllerContext = _controllerContext.Object;
                result = await controller.SGResultPopup(custId, gamePlayId, lang).ConfigureAwait(false);
            }

            //Assert
            var viewResult = Assert.IsType<string>(result);
            Assert.Equal(fakeResult, viewResult);

        }

        [Fact]
        public async Task VRGResultPopup_WhenCall_RetrunStringResult()
        {
            //Arrange
            const string platfrom = "Any";
            const string lang = "en-US";
            const string gamePlayId = "Any VRG";
            const string fakeResult = "Any VRG result data";
            string result = null;
            _betListApiClient.Setup(x => x.PostAsyncString(
                "/VirtualGamesResultPopup/Index", It.IsAny<object>(),
                It.IsAny<string>()))
                .Returns(Task.FromResult(fakeResult));

            //Action
            using (var controller = new ResultReportController(_betListApiClient.Object))
            {
                controller.ControllerContext = _controllerContext.Object;
                result = await controller.VirtualGamesResultPopup(gamePlayId, platfrom, lang).ConfigureAwait(false);
            }

            //Assert
            var viewResult = Assert.IsType<string>(result);
            Assert.Equal(fakeResult, viewResult);

        }

        [Fact]
        public async Task CCResultPopup_WhenCall_RetrunStringResult()
        {
            //Arrange
            const string custId = "Any";
            const string lang = "en-US";
            const string gameTypeId = "gameTypeId";
            const string transId3rd = "transId3rd";
            const string fakeResult = "Any result data";
            string result = null;
            _betListApiClient.Setup(
                x => x.PostAsyncString(
                "/CardClubResultPopup/Index",
                It.IsAny<object>(),
                It.IsAny<string>()))
            .Returns(Task.FromResult(fakeResult));

            //Action
            using (var controller = new ResultReportController(_betListApiClient.Object))
            {
                controller.ControllerContext = _controllerContext.Object;
                result = await controller.CCResultPopup(custId, gameTypeId, transId3rd, lang).ConfigureAwait(false);
            }

            //Assert
            var viewResult = Assert.IsType<string>(result);
            Assert.Equal(fakeResult, viewResult);
        }

        [Fact]
        public async Task AllBetResult_WhenCall_RetrunCorrectViewModelAsync()
        {
            //Arrange
            const string Title = "Result";

            const string transDesc = "Any";
            const string lang = "en-US";
            const string betCheck = "Any";
            const string betType = "Any";

            const string fakeResult = "Any result data{rootUrl}/";
            const string fakeRootUrl = "RootURL";
            IActionResult result = null;
            _betListApiClient.Setup(x => x.PostAsyncString(
                "/AllBetResultPopup/Index", It.IsAny<object>(),
                It.IsAny<string>()))
                .Returns(Task.FromResult(fakeResult));
            var mockUrl = new Mock<IUrlHelper>();
            mockUrl.Setup(x => x.Content(It.IsAny<string>())).Returns(fakeRootUrl);

            //Action
            using (var controller = new ResultReportController(_betListApiClient.Object))
            {
                controller.ControllerContext = _controllerContext.Object;
                controller.Url = mockUrl.Object;
                result = await controller.AllBetResult(transDesc, betCheck, betType, lang).ConfigureAwait(false);
            }

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var viewModel = Assert.IsType<BaseViewModel>(viewResult.Model);

            var title = Assert.IsAssignableFrom<string>(viewResult.ViewData["Title"]);
            var actualResult = Assert.IsAssignableFrom<string>(viewResult.ViewData["Data"]);
            Assert.Contains(fakeRootUrl, actualResult);
            Assert.Equal(title, Title);
            Assert.Equal(viewModel.Lang, lang);
            Assert.Equal(viewModel.AccessToken, AccessToken);
        }

        [Fact]
        public async Task GlobalGamingResult_WhenCall_RetrunStringResult()
        {
            //Arrange
            const string custId = "Any";
            const string lang = "en-US";
            const string gameId = "Any";
            const string fakeResult = "Any result data";
            string result = null;
            _betListApiClient.Setup(x => x.PostAsyncString(
                "/GlobalGamingResultPopup/Index", It.IsAny<object>(),
                It.IsAny<string>()))
                .Returns(Task.FromResult(fakeResult));

            //Action
            using (var controller = new ResultReportController(_betListApiClient.Object))
            {
                controller.ControllerContext = _controllerContext.Object;
                result = await controller.GlobalGamingResult(custId, gameId, lang).ConfigureAwait(false);
            }

            //Assert
            var viewResult = Assert.IsType<string>(result);
            Assert.Equal(fakeResult, viewResult);
        }
    }
}