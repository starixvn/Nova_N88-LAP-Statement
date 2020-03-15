using System.Threading.Tasks;
using AutoFixture;
using Fanex.Athena.ExtService.BetList.Controllers;
using Fanex.Athena.ExtService.BetList.Models.MetaModel;
using Fanex.Athena.ExtService.BetList.Models.ViewModel;
using Fanex.Athena.ExtService.Common;

using Microsoft.AspNetCore.Mvc;

using Moq;

using Xunit;

namespace Fanex.Athena.ExtService.BetList.UnitTests.Controllers
{
    public class SystemParlayBetListControllerTest : BaseControllerTest
    {
        private static readonly Fixture _fixture = new Fixture();

        [Fact]
        public async Task Detail_WhenCall_RetrunCorrectViewModelAsync()
        {
            //Arrange
            var metalModel = _fixture.Create<SystemParlayDetailMetaModel>();
            metalModel.Lang = "en-US";
            const string fakeResult = "Any Data String";
            IActionResult result = null;
            _betListApiClient.Setup(x => x.PostAsyncString(
                "/SystemParlayBetList/Detail", 
                It.IsAny<SystemParlayDetailMetaModel>(),
                It.IsAny<string>()))
                .Returns(Task.FromResult(fakeResult));
            var licenseeInfo = await _commonService.Object.GetLicenseeInfoAsync(SiteName, 0);

            //Action
            using (var controller = new SystemParlayBetListController(_commonService.Object
                , _betListApiClient.Object))
            {
                controller.ControllerContext = _controllerContext.Object;
                result = await controller.Detail(metalModel).ConfigureAwait(false);
            }

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var viewModel = Assert.IsType<SystemParlayDetailViewModel>(viewResult.Model);

            Assert.Equal(SystemHelper.GenerateAgentListXml(licenseeInfo), metalModel.AgentListXml);

            Assert.NotNull(viewModel);

            Assert.Equal(viewModel.DetailText, fakeResult);
        }
    }
}