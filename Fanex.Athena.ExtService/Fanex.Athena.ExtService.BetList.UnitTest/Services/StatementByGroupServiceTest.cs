using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Fanex.Athena.ExtService.BetList.Models.DTO;
using Fanex.Athena.ExtService.BetList.Models.MetaModel;
using Fanex.Athena.ExtService.BetList.Services;
using Fanex.Athena.ExtService.Common.Models.DTO;
using Fanex.Athena.ExtService.Common.Services;
using Fanex.Data;
using Moq;
using Xunit;

namespace Fanex.Athena.ExtService.BetList.UnitTests.Services
{
    public class StatementByGroupServiceTest
    {
        public StatementByGroupServiceTest()
        {
            DbSettingProviderManager
             .StartTestingSession()
             .Run();
        }

        [Fact]
        public async Task QueryAsync_WhenCall_AssignCorrectParrameterAndReturnCorrectResultAsync()
        {
            //Arrange
            var balanceUpInfo = new BalanceUp { BalanceUpDay = DateTime.Now, MoveBetsDay = DateTime.Now };
            var metaModel = new StatementByGroupMetaModel { WinlostDate = DateTime.Now };

            var mockCommonService = new Mock<ICommonService>();
            mockCommonService.Setup(x => x.GetBalanceUpAsync()).Returns(Task.FromResult(
                balanceUpInfo
                ));

            IEnumerable<StatementByGroup> fakeData = new List<StatementByGroup>();
            var mockObjectDB = new Mock<IObjectDb>();

            mockObjectDB.Setup(x => x.QueryAsync<StatementByGroup>(It.IsAny<object>()
                , It.IsAny<DbTransaction>(),
                It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(fakeData));

            ObjectDbFactory.SwitchToTestingMode();
            ObjectDbFactory.Mock((x, y) => mockObjectDB.Object);

            var service = new StatementByGroupService(mockCommonService.Object);


            //Action
            var result = await service.QueryAsync(metaModel).ConfigureAwait(false);

            //Assert
            Assert.Equal(result.DataList, fakeData);
            mockObjectDB.Verify(x => x.SetParameterValue("@CustID", metaModel.CustId), Times.Once);
            mockObjectDB.Verify(x => x.SetParameterValue("@Winlostdate", metaModel.WinlostDate), Times.Once);
            mockObjectDB.Verify(x => x.SetParameterValue("@BalanceUpDay", balanceUpInfo.BalanceUpDay), Times.Once);
            mockObjectDB.Verify(x => x.SetParameterValue("@MoveBetsDay", balanceUpInfo.MoveBetsDay), Times.Once);
            mockObjectDB.Verify(x => x.SetParameterValue("@baseCurrency", metaModel.BaseCurrency), Times.Once);
            mockObjectDB.Verify(x => x.SetParameterValue("@ModeView", metaModel.ModeView), Times.Once);

        }
    }
}
