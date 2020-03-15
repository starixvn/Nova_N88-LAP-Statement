using System;
using System.Threading.Tasks;

using Fanex.Athena.ExtService.BetList.Controllers;
using Fanex.Athena.ExtService.BetList.Models.DataDomain;
using Fanex.Athena.ExtService.BetList.Models.MetaModel;
using Fanex.Athena.ExtService.BetList.Models.ViewModel;
using Fanex.Athena.ExtService.BetList.Services;

using Microsoft.AspNetCore.Mvc;

using Moq;

using Xunit;

namespace Fanex.Athena.ExtService.BetList.UnitTests.Controllers
{
    public class StatementControllerTest : BaseControllerTest
    {
        [Fact]
        public async Task Summary_WhenExecute_AlwayAssignCommonViewModelValue()
        {
            //Arrange
            const int Custid = 100;
            var metalModel = new SummaryStatementMetaModel
            {
                CustId = Custid,
                Lang = "en-US",
                BaseCurrency = "USD"
            };
            var fakeData = new SummaryStatementViewModel();
            IActionResult expectedResult = null;
            var fakeService = new Mock<ISummaryStatementService>();
            fakeService.Setup(x => x.QueryAsync(It.IsAny<SummaryStatementMetaModel>()))
            .Returns(Task.FromResult(fakeData));
            //Action
            using (var controller = new StatementController())
            {
                controller.ControllerContext = _controllerContext.Object;
                expectedResult = await controller.Summary(fakeService.Object, metalModel).ConfigureAwait(false);
            }

            //Assert
            var viewResult = Assert.IsType<ViewResult>(expectedResult);
            var viewModel = Assert.IsType<SummaryStatementViewModel>(viewResult.Model);
            Assert.Equal(viewModel.BaseCurrency, metalModel.BaseCurrency);
            Assert.Equal(viewModel.Lang, metalModel.Lang);
            Assert.Equal(viewModel.AccessToken, AccessToken);
            Assert.Equal(viewModel.CustId, Custid);

        }

        [Fact]
        public async Task Summary_ValidModel_QueryDataFromService()
        {
            //Arrange
            var metalModel = new SummaryStatementMetaModel
            {
                CustId = 100,
                FromDate = DateTime.Now,
                ToDate = DateTime.Now,
                PageSize = 50,
                PageIndex = 100,
                Lang = "en-US",
                BaseCurrency = "USD"
            };
            var fakeData = new SummaryStatementViewModel();
            var fakeService = new Mock<ISummaryStatementService>();
            fakeService.Setup(x => x.QueryAsync(It.IsAny<SummaryStatementMetaModel>()))
            .Returns(Task.FromResult(fakeData));
            //Action
            using (var controller = new StatementController())
            {
                controller.ControllerContext = _controllerContext.Object;
                await controller.Summary(fakeService.Object, metalModel).ConfigureAwait(false);
            }

            //Assert
            fakeService.Verify(x => x.QueryAsync(metalModel), Times.Once());
        }


        [Theory]
        [InlineData(TransDescValue.ModeViewBetStatement, TransDescValue.ModeViewBetStatement, "Betting Statement")]
        [InlineData(TransDescValue.ModeViewCancell, TransDescValue.ModeViewCancell, "Void/Cancelled Bet")]
        [InlineData(TransDescValue.ModeViewOutstanding, TransDescValue.ModeViewOutstanding, "Outstanding Bet")]
        [InlineData(TransDescValue.ModeViewPromotion, 0, "Promotion")]
        [InlineData(0, 0, "Statement")]
        public async Task ByGroup_WhenExecute_AlwayAssignCommonViewModelValue(int modeView, int expectedModelView, string expectedTransDesc)
        {
            //Arrange
            const int Custid = 100;
            var metalModel = new StatementByGroupMetaModel
            {
                CustId = Custid,
                Lang = "en-US",
                BaseCurrency = "USD",
                ModeView = modeView
            };
            var fakeData = new StatementByGroupViewModel();
            IActionResult expectedResult = null;
            var fakeService = new Mock<IStatementByGroupService>();
            fakeService.Setup(x => x.QueryAsync(It.IsAny<StatementByGroupMetaModel>()))
            .Returns(Task.FromResult(fakeData));
            //Action
            using (var controller = new StatementController())
            {
                controller.ControllerContext = _controllerContext.Object;
                expectedResult = await controller.ByGroup(fakeService.Object, metalModel).ConfigureAwait(false);
            }

            //Assert
            var viewResult = Assert.IsType<ViewResult>(expectedResult);
            var viewModel = Assert.IsType<StatementByGroupViewModel>(viewResult.Model);
            Assert.Equal(viewModel.BaseCurrency, metalModel.BaseCurrency);
            Assert.Equal(viewModel.Lang, metalModel.Lang);
            Assert.Equal(viewModel.AccessToken, AccessToken);
            Assert.Equal(viewModel.CustId, Custid);
            Assert.Equal(viewModel.ModeView, expectedModelView);
            Assert.Equal(viewModel.TransDesc, expectedTransDesc);
        }

        [Fact]
        public async Task ByGroup_ValidModel_QueryDataFromService()
        {
            //Arrange
            var metalModel = new StatementByGroupMetaModel
            {
                CustId = 100,
                WinlostDate = DateTime.Now,
                ModeView = 1,
                Lang = "en-US",
                BaseCurrency = "USD"
            };
            var fakeData = new StatementByGroupViewModel();
            var fakeService = new Mock<IStatementByGroupService>();
            fakeService.Setup(x => x.QueryAsync(It.IsAny<StatementByGroupMetaModel>()))
            .Returns(Task.FromResult(fakeData));
            //Action
            using (var controller = new StatementController())
            {
                controller.ControllerContext = _controllerContext.Object;
                await controller.ByGroup(fakeService.Object, metalModel).ConfigureAwait(false);
            }

            //Assert
            fakeService.Verify(x => x.QueryAsync(metalModel), Times.Once());
        }
    }
}
