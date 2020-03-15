using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

using Fanex.Athena.ExtService.Common.Models.DTO;
using Fanex.Athena.ExtService.Common.Services;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Moq;

namespace Fanex.Athena.ExtService.BetList.UnitTests.Controllers
{
    public class BaseControllerTest
    {
        protected readonly Mock<HttpContext> _httpContext;
        protected readonly Mock<IIdentity> _identity;
        protected readonly Mock<ControllerContext> _controllerContext;
        protected const string SiteName = "12BET";
        protected string AccessToken = "ANy";
        protected readonly Mock<ICommonService> _commonService;
        protected readonly Mock<IBetListApiClient> _betListApiClient;

        public BaseControllerTest()
        {
            _commonService = new Mock<ICommonService>();
            _commonService.Setup(x => x.GetLicenseeInfoAsync(SiteName, It.IsAny<int>()))
                .Returns(Task.FromResult(new LicenseeInfo
                {
                    SiteName = SiteName,
                    Currencies = new CurrencyInfo[] { new CurrencyInfo {
                        CurrencyId =1,
                        CurrencyName="UUS",
                        AgentId=100
                    } },
                    IsAllowedCustomerComm = true

                }));

            _betListApiClient = new Mock<IBetListApiClient>();

            _identity = new Mock<IIdentity>();
            _identity.Setup(x => x.IsAuthenticated).Returns(true);
            _identity.Setup(x => x.Name).Returns(SiteName);

            _httpContext = new Mock<HttpContext>();
            _httpContext.Setup(x => x.Request.Query["access_token"]).Returns(AccessToken);
            var claimsPrinciple = new ClaimsPrincipal(_identity.Object);
            _httpContext.Setup(x => x.User).Returns(claimsPrinciple);

            _controllerContext = new Mock<ControllerContext>();
            _controllerContext.Object.HttpContext = _httpContext.Object;
        }
    }
}
