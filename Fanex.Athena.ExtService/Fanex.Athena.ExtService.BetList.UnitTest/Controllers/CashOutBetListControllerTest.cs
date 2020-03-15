using System.Threading.Tasks;
using AutoFixture;
using Fanex.Athena.ExtService.BetList.Controllers;
using Fanex.Athena.ExtService.BetList.Models.MetaModel;
using Fanex.Athena.ExtService.BetList.Models.ViewModel;

using Microsoft.AspNetCore.Mvc;

using Moq;

using Xunit;

namespace Fanex.Athena.ExtService.BetList.UnitTests.Controllers
{
    public class CashOutBetListControllerTest : BaseControllerTest
    {
        private static readonly Fixture _fixture = new Fixture();

        [Fact]
        public async Task Detail_WhenCall_RetrunCorrectViewModelAsync()
        {
            //Arrange
            const string Title = "Cash Out BetList";
            var metalModel = _fixture.Create<CashOutBetListDetailMetaModel>();
            metalModel.Lang = "en-US";

            var fakeResult = new CashOutBetListDetailViewModel();
            IActionResult result = null;
            _betListApiClient.Setup(x => x.PostAsync<CashOutBetListDetailViewModel>(
                "/CashOutBetList/Detail", It.IsAny<CashOutBetListDetailMetaModel>(),
                It.IsAny<string>()))
                .Returns(Task.FromResult(fakeResult));

            var licenseeInfo = await _commonService.Object.GetLicenseeInfoAsync(SiteName, 0);

            //Action
            using (var controller = new CashOutBetListController(_commonService.Object
                , _betListApiClient.Object))
            {
                controller.ControllerContext = _controllerContext.Object;
                result = await controller.Detail(metalModel).ConfigureAwait(false);
            }

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var viewModel = Assert.IsType<CashOutBetListDetailViewModel>(viewResult.Model);
            var title = Assert.IsAssignableFrom<string>(viewResult.ViewData["Title"]);

            Assert.NotNull(viewModel);
            Assert.Equal(viewModel.OriginalTicketText, fakeResult.OriginalTicketText);
            Assert.Equal(viewModel.CashOutTicketList, fakeResult.CashOutTicketList);
            Assert.Equal(viewModel.CashOutTicketTextList, fakeResult.CashOutTicketTextList);
            Assert.Equal(viewModel.IsCashOut, fakeResult.IsCashOut);
            Assert.Equal(viewModel.IsHighlightOriginalTicket, fakeResult.IsHighlightOriginalTicket);
            Assert.Equal(viewModel.Info, licenseeInfo);
            Assert.Equal(viewModel.BaseCurrency, metalModel.BaseCurrency);
            Assert.Equal(viewModel.Lang, metalModel.Lang);
            Assert.Equal(viewModel.AccessToken, AccessToken);
            Assert.Equal(title, Title);
        }
    }
}