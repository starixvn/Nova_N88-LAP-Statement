using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture;
using Fanex.Athena.ExtService.BetList.Controllers;
using Fanex.Athena.ExtService.BetList.Models.DTO;
using Fanex.Athena.ExtService.BetList.Models.MetaModel;
using Fanex.Athena.ExtService.BetList.Models.ViewModel;

using Microsoft.AspNetCore.Mvc;

using Moq;

using Xunit;

namespace Fanex.Athena.ExtService.BetList.UnitTests.Controllers
{
    public class MixParlayBetListControllerTest : BaseControllerTest
    {
        private static readonly Fixture _fixture = new Fixture();

        [Fact]
        public async Task Detail_WhenCall_RetrunCorrectViewModelAsync()
        {
            //Arrange
            var metalModel = _fixture.Create<MixParlayDetailMetaModel>();
            metalModel.Lang = "en-US";
            IList<TicketText> fakeResult = new List<TicketText>();
            IActionResult result = null;
            _betListApiClient.Setup(x => x.PostAsync<IList<TicketText>>(
                "/MixParlayBetList/Detail", It.IsAny<MixParlayDetailMetaModel>(),
                It.IsAny<string>()))
                .Returns(Task.FromResult(fakeResult));

            //Action
            using (var controller = new MixParlayBetListController(_betListApiClient.Object))
            {
                controller.ControllerContext = _controllerContext.Object;
                result = await controller.Detail(metalModel).ConfigureAwait(false);
            }

            //Assert
            var viewResult = Assert.IsType<JsonResult>(result);
            var viewModel = Assert.IsAssignableFrom<IList<TicketText>>(viewResult.Value);

            Assert.Equal(fakeResult, viewModel);
        }

        [Fact]
        public async Task NewParlayDetail_WhenCall_RetrunCorrectViewModelAsync()
        {
            //Arrange
            var metalModel = _fixture.Create<NewParlayDetailMetaModel>();
            metalModel.Lang = "en-US";
            var fakeResult = new NewParlayDetailViewModel();
            IActionResult result = null;
            _betListApiClient.Setup(x => x.PostAsync<NewParlayDetailViewModel>(
                "/MixParlayBetList/NewParlayDetail", It.IsAny<NewParlayDetailMetaModel>(),
                It.IsAny<string>()))
                .Returns(Task.FromResult(fakeResult));

            //Action
            using (var controller = new MixParlayBetListController(_betListApiClient.Object))
            {
                controller.ControllerContext = _controllerContext.Object;
                result = await controller.NewParlayDetail(metalModel).ConfigureAwait(false);
            }

            //Assert
            var viewResult = Assert.IsType<JsonResult>(result);
            var viewModel = Assert.IsAssignableFrom<NewParlayDetailViewModel>(viewResult.Value);

            Assert.Equal(fakeResult, viewModel);
            Assert.Equal(viewModel.Texts, fakeResult.Texts);
            Assert.Equal(viewModel.Data, fakeResult.Data);
        }
    }
}