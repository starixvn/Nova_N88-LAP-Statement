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
    public class SummaryStatementServiceTest
    {
        public SummaryStatementServiceTest()
        {
            DbSettingProviderManager
             .StartTestingSession()
             .Run();
        }

        [Fact]
        public async Task QueryAsync_WhenCall_AssignCorrectParrameterAndReturnCorrectResultAsync()
        {
            //Arrange
            const int TotalRow = 100;
            var balanceUpInfo = new BalanceUp { BalanceUpDay = DateTime.Now, MoveBetsDay = DateTime.Now };
            var metaModel = new SummaryStatementMetaModel();

            var mockCommonService = new Mock<ICommonService>();
            mockCommonService.Setup(x => x.GetBalanceUpAsync()).Returns(Task.FromResult(
               balanceUpInfo
                ));

            IEnumerable<SummaryStatement> fakeData = new List<SummaryStatement>();
            var mockObjectDB = new Mock<IObjectDb>();

            mockObjectDB.Setup(x => x.QueryAsync<SummaryStatement>(It.IsAny<object>()
                , It.IsAny<DbTransaction>(),
                It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(fakeData));

            mockObjectDB.Setup(x => x.GetOutputParams()).Returns(new Hashtable
            {
                { "TotalRows",TotalRow }
            });

            ObjectDbFactory.SwitchToTestingMode();
            ObjectDbFactory.Mock((x, y) => mockObjectDB.Object);

            var service = new SummaryStatementService(mockCommonService.Object);


            //Action
            var result = await service.QueryAsync(metaModel).ConfigureAwait(false);

            //Assert
            Assert.Equal(result.DataList, fakeData);
            Assert.Equal(result.TotalRows, TotalRow);
            mockObjectDB.Verify(x => x.SetParameterValue("custId", metaModel.CustId), Times.Once);
            mockObjectDB.Verify(x => x.SetParameterValue("FromDate", metaModel.FromDate), Times.Once);
            mockObjectDB.Verify(x => x.SetParameterValue("@ToDate", metaModel.ToDate), Times.Once);
            mockObjectDB.Verify(x => x.SetParameterValue("@BalanceUpDay", balanceUpInfo.BalanceUpDay), Times.Once);
            mockObjectDB.Verify(x => x.SetParameterValue("@MoveBetsDay", balanceUpInfo.MoveBetsDay), Times.Once);
            mockObjectDB.Verify(x => x.SetParameterValue("@PageSize", metaModel.PageSize), Times.Once);
            mockObjectDB.Verify(x => x.SetParameterValue("@PageIndex", metaModel.PageIndex), Times.Once);
            mockObjectDB.Verify(x => x.SetParameterValue("@baseCurrency", metaModel.BaseCurrency), Times.Once);
            mockObjectDB.Verify(x => x.SetParameterValue("@TotalRows", 0), Times.Once);
        }
    }
}
