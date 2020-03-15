using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Fanex.Athena.ExtService.Common.Attributes;
using Fanex.Athena.ExtService.Common.Models.DTO;
using Fanex.Athena.ExtService.Common.Models.ViewModels;
using Fanex.Athena.ExtService.Common.Services;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;

using Moq;

using Xunit;

namespace Fanex.Athena.ExtService.Core.UnitTests.Attributes
{
    public class CheckCustomerAttributeTest
    {
        private readonly Mock<ICommonService> _mockCommonService;
        private readonly Mock<HttpContext> _mockHttpContext;
        private readonly Mock<ICustomerMetaModel> _mockCustomerMetaModel;
        private const string ResultMsg = "Not found any customers that match your query!";
        private readonly ActionContext _actionContext;
        public CheckCustomerAttributeTest()
        {
            var modelState = new ModelStateDictionary();
            _mockHttpContext = new Mock<HttpContext>();
            _mockHttpContext.Setup(x => x.User.Identity.Name).Returns("12BET");
            _actionContext = new ActionContext(
                  _mockHttpContext.Object,
                  Mock.Of<RouteData>(),
                  Mock.Of<ActionDescriptor>(),
                  modelState
              );

            _mockCustomerMetaModel = new Mock<ICustomerMetaModel>();
            _mockCommonService = new Mock<ICommonService>();
        }

        [Fact]
        public async Task OnActionExecution_MetaModelIsNull_ReturnExpectedMsg()
        {
            //Arrange
            //Action
            var filter = new CheckCustomerAttribute(_mockCommonService.Object);
            var actionExcutingContext = new ActionExecutingContext(
            _actionContext,
            new List<IFilterMetadata>(),
            new Dictionary<string, object>(),
            Mock.Of<Controller>());
            await filter.OnActionExecutionAsync(actionExcutingContext, next: () =>
           Task.FromResult(new ActionExecutedContext(
               actionExcutingContext,
               actionExcutingContext.Filters,
               Mock.Of<Controller>()))); ;

            //Assert
            var actualResult = Assert.IsAssignableFrom<ContentResult>(actionExcutingContext.Result);

            Assert.Equal(ResultMsg, actualResult.Content);
        }


        [Theory]
        [InlineData(0)]
        [InlineData(-10000)]
        public async Task OnActionExecution_InValidCustID_ReturnExpectedMsg(int custID)
        {
            //Arrange
            _mockCustomerMetaModel.Setup(x => x.CustId).Returns(custID);
            var actionExcutingContext = new ActionExecutingContext(
           _actionContext,
           new List<IFilterMetadata>(),
           new Dictionary<string, object> { { "metaModel", _mockCustomerMetaModel.Object } },
           Mock.Of<Controller>());

            //Action
            var filter = new CheckCustomerAttribute(_mockCommonService.Object);
            await filter.OnActionExecutionAsync(actionExcutingContext, next: () =>
           Task.FromResult(new ActionExecutedContext(
               actionExcutingContext,
               actionExcutingContext.Filters,
               Mock.Of<Controller>()))); ;

            //Assert
            var actualResult = Assert.IsAssignableFrom<ContentResult>(actionExcutingContext.Result);
            _mockCommonService.Verify(x => x.GetLicenseeInfoAsync(It.IsAny<string>(), It.IsAny<int>()), Times.Never);
            Assert.Equal(ResultMsg, actualResult.Content);
        }
        [Fact]
        public async Task OnActionExecution_ValidCustomer_ContineToExecuteAction()
        {
            //Arrange
            _mockCommonService.Setup(x => x.GetLicenseeInfoAsync(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(Task.FromResult(new LicenseeInfo
                {
                    Currencies = new CurrencyInfo[] { new CurrencyInfo() }

                }));
            _mockCustomerMetaModel.Setup(x => x.CustId).Returns(100);
            var actionExcutingContext = new ActionExecutingContext(
           _actionContext,
           new List<IFilterMetadata>(),
           new Dictionary<string, object> { { "metaModel", _mockCustomerMetaModel.Object } },
           Mock.Of<Controller>());

            //Action
            var filter = new CheckCustomerAttribute(_mockCommonService.Object);
            await filter.OnActionExecutionAsync(actionExcutingContext, next: () =>
           Task.FromResult(new ActionExecutedContext(
               actionExcutingContext,
               actionExcutingContext.Filters,
               Mock.Of<Controller>()))); ;

            //Assert
            _mockCommonService.Verify(x => x.GetLicenseeInfoAsync(It.IsAny<string>(), It.IsAny<int>()), Times.Once);
            Assert.Null(actionExcutingContext.Result);
        }

        [Fact]
        public async Task OnActionExecution_InValidCustomer_ContineToExecuteAction()
        {
            //Arrange
            _mockCommonService.Setup(x => x.GetLicenseeInfoAsync(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(Task.FromResult(new LicenseeInfo
                {
                    Currencies = new CurrencyInfo[] { }
                }));
            _mockCustomerMetaModel.Setup(x => x.CustId).Returns(100);
            var actionExcutingContext = new ActionExecutingContext(
           _actionContext,
           new List<IFilterMetadata>(),
           new Dictionary<string, object> { { "metaModel", _mockCustomerMetaModel.Object } },
           Mock.Of<Controller>());

            //Action
            var filter = new CheckCustomerAttribute(_mockCommonService.Object);
            await filter.OnActionExecutionAsync(actionExcutingContext, next: () =>
           Task.FromResult(new ActionExecutedContext(
               actionExcutingContext,
               actionExcutingContext.Filters,
               Mock.Of<Controller>()))); ;

            //Assert
            var actualResult = Assert.IsAssignableFrom<ContentResult>(actionExcutingContext.Result);
            _mockCommonService.Verify(x => x.GetLicenseeInfoAsync(It.IsAny<string>(), It.IsAny<int>()), Times.Once);
            Assert.Equal(ResultMsg, actualResult.Content);
        }
    }
}
