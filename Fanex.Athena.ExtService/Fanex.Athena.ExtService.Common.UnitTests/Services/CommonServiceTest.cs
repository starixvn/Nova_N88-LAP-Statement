using Fanex.Athena.ExtService.Common.Models.DTO;
using Fanex.Athena.ExtService.Common.Services;
using Fanex.Data;
using Microsoft.Extensions.Caching.Distributed;
using Moq;
using MsgPack.Serialization;
using System;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Fanex.Athena.ExtService.Common.UnitTests.Services
{
    public class CommonServiceTest
    {
        private const string BalanceUpCacheKey = "EXT_LAP_CACHE_BALANCEUP";
        private const string LicenseeExtInfoCacheKey = "__LicenseeExtInfoCacheKey_";

        private readonly Mock<IDistributedCache> _mockCache;
        private readonly SerializationContext _serializationContext = new SerializationContext
        {
            DefaultDateTimeConversionMethod = DateTimeConversionMethod.Native
        };

        public CommonServiceTest()
        {
            _mockCache = new Mock<IDistributedCache>();
            DbSettingProviderManager.StartTestingSession().Run();
        }

        [Fact]
        public async Task GetBalanceUpAsync_WhenCallWithNoDataInCache_GetResulFromDB()
        {
            //Arrange
            var balanceupInfo = new BalanceUp
            {
                BalanceUpDay = DateTime.Now.AddHours(3),
                MoveBetsDay = DateTime.Now.AddDays(-2)
            };
            SetupBalanceUpDatabaseData(balanceupInfo);
            _mockCache.SetReturnsDefault(Task.FromResult((byte[])null));

            var service = new CommonService(_mockCache.Object);

            //Action
            var result = await service.GetBalanceUpAsync().ConfigureAwait(false);

            //Assert
            Assert.Equal(result.BalanceUpDay, balanceupInfo.BalanceUpDay);
            Assert.Equal(result.MoveBetsDay, balanceupInfo.MoveBetsDay);
        }

        [Fact]
        public async Task GetBalanceUpAsync_ResulFromDB_SetCache()
        {
            //Arrange
            SetupBalanceUpDatabaseData(new BalanceUp());
            _mockCache.SetReturnsDefault(Task.FromResult((byte[])null));

            var service = new CommonService(_mockCache.Object);

            //Action
            var result = await service.GetBalanceUpAsync().ConfigureAwait(false);

            //Assert
            _mockCache.Verify(c => c.SetAsync(BalanceUpCacheKey,
                It.IsAny<byte[]>(),
                It.Is<DistributedCacheEntryOptions>(o => o.AbsoluteExpiration != null),
                It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task GetBalanceUpAsync_WhenCall_ReturnDataFromCache()
        {
            //Arrange
            var balanceupInfo = new BalanceUp
            {
                BalanceUpDay = DateTime.Now,
                MoveBetsDay = DateTime.Now.AddMonths(-1)
            };

            var cachedValue = MessagePackSerializer.Get<BalanceUp>(_serializationContext)
                .PackSingleObject(balanceupInfo);
            _mockCache.Setup(c => c.GetAsync(BalanceUpCacheKey, It.IsAny<CancellationToken>())).ReturnsAsync(cachedValue);

            var service = new CommonService(_mockCache.Object);

            //Action
            var result = await service.GetBalanceUpAsync().ConfigureAwait(false);

            //Assert
            Assert.Equal(balanceupInfo.MoveBetsDay, result.MoveBetsDay);
            Assert.Equal(balanceupInfo.BalanceUpDay, result.BalanceUpDay);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public async Task GetLicenseeInfoAsync_WhenCallWithoutdataInCache_ReturnDataFromDB(int custId)
        {
            //Arrange
            const string SiteName = "12Bet";
            const int CurrencyId = 1;
            const int AgentId = 2;
            const string CurrencyName = "USD";
            const bool HasComm = true;

            SetupLicenseeInfoDatabaseData(CurrencyId, CurrencyName, AgentId, HasComm);
            _mockCache.SetReturnsDefault(Task.FromResult((byte[])null));

            var service = new CommonService(_mockCache.Object);

            ///Action

            var result = await service.GetLicenseeInfoAsync(SiteName, custId).ConfigureAwait(false);

            ///Assert
            Assert.Equal(result.SiteName, SiteName);
            Assert.Equal(result.IsAllowedCustomerComm, HasComm);
            Assert.Equal(result.Currencies[0].CurrencyId, CurrencyId);
            Assert.Equal(result.Currencies[0].CurrencyName, CurrencyName);
            Assert.Equal(result.Currencies[0].AgentId, AgentId);

            var objectDb = ObjectDbFactory.CreateInstance("mock");
            var mockObjectDB = Mock.Get(objectDb);
            mockObjectDB.Verify(x => x.SetParameterValue("siteName", SiteName), Times.Once);
            mockObjectDB.Verify(x => x.SetParameterValue("custId", custId), Times.Once);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public async Task GetLicenseeInfoAsync_DataFromDB_SetCache(int custId)
        {
            //Arrange
            const string SiteName = "12Bet-test";
            const int CurrencyId = 10;
            const int AgentId = 20;
            const string CurrencyName = "EUR";
            const bool HasComm = false;

            SetupLicenseeInfoDatabaseData(CurrencyId, CurrencyName, AgentId, HasComm);

            var realCacheKey = $"{LicenseeExtInfoCacheKey}{SiteName}_{custId}";
            _mockCache.SetReturnsDefault(Task.FromResult((byte[])null));

            var service = new CommonService(_mockCache.Object);
            ///Action

            var result = await service.GetLicenseeInfoAsync(SiteName, custId).ConfigureAwait(false);

            ///Assert
            _mockCache.Verify(c => c.SetAsync(realCacheKey, It.IsAny<byte[]>(), It.Is<DistributedCacheEntryOptions>(o =>
                (custId > 0 && o.SlidingExpiration != null) ||
                (custId == 0 && o.AbsoluteExpiration != null)), It.IsAny<CancellationToken>()));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public async Task GetLicenseeInfoAsync_WhenCall_ReturnDataFromCache(int custId)
        {
            //Arrange
            const string SiteName = "12Bet";
            const int CurrencyId = 1;
            const int AgentId = 2;
            const string CurrencyName = "USD";
            const bool HasComm = true;
            var realCacheKey = $"{LicenseeExtInfoCacheKey}{SiteName}_{custId}";
            var licenseeInfo = new LicenseeInfo
            {
                Currencies = new[] { new CurrencyInfo { CurrencyId = CurrencyId, CurrencyName = CurrencyName, AgentId = AgentId } },
                SiteName = SiteName,
                IsAllowedCustomerComm = HasComm
            };

            var cachedValue = MessagePackSerializer.Get<LicenseeInfo>(_serializationContext)
                .PackSingleObject(licenseeInfo);
            _mockCache.Setup(c => c.GetAsync(realCacheKey, It.IsAny<CancellationToken>()))
                .ReturnsAsync(cachedValue);

            var service = new CommonService(_mockCache.Object);

            ///Action

            var result = await service.GetLicenseeInfoAsync(SiteName, custId).ConfigureAwait(false);

            ///Assert
            Assert.Equal(result.SiteName, SiteName);
            Assert.Equal(result.IsAllowedCustomerComm, HasComm);
            Assert.Equal(result.Currencies[0].CurrencyId, CurrencyId);
            Assert.Equal(result.Currencies[0].CurrencyName, CurrencyName);
            Assert.Equal(result.Currencies[0].AgentId, AgentId);
        }

        private void SetupBalanceUpDatabaseData(BalanceUp balanceup)
        {
            var mockIDataReader = new Mock<IDataReader>();
            mockIDataReader.Setup(m => m["balanceupday"]).Returns(balanceup.BalanceUpDay);
            mockIDataReader.Setup(m => m["movebetsday"]).Returns(balanceup.MoveBetsDay);
            mockIDataReader.SetupSequence(m => m.Read()).Returns(true).Returns(false);
            MockObjectDbFactory(mockIDataReader.Object);
        }

        private void SetupLicenseeInfoDatabaseData(int currencyId, string currencyName, int agentId, bool hasComm)
        {
            var mockIDataReader = new Mock<IDataReader>();
            mockIDataReader.Setup(m => m["CurrencyId"]).Returns(currencyId);
            mockIDataReader.Setup(m => m["AgentId"]).Returns(agentId);
            mockIDataReader.Setup(m => m["CurrencyName"]).Returns(currencyName);
            mockIDataReader.Setup(m => m["IsAllowedCustomerComm"]).Returns(hasComm);
            mockIDataReader.Setup(x => x.NextResult()).Returns(true);
            mockIDataReader.SetupSequence(m => m.Read())
              .Returns(true)
              .Returns(false)
              .Returns(true)
              .Returns(false);

            MockObjectDbFactory(mockIDataReader.Object);
        }

        private static void MockObjectDbFactory(IDataReader reader)
        {
            var mockObjectDB = new Mock<IObjectDb>();
            mockObjectDB.Setup(x =>
            x.ExecuteReaderAsync(It.IsAny<object>(),
            It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>())).ReturnsAsync(reader);

            ObjectDbFactory.SwitchToTestingMode();
            ObjectDbFactory.Mock((x, y) => mockObjectDB.Object);
        }
    }
}
