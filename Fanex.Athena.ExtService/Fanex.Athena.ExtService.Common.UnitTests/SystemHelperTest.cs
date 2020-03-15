using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Fanex.Athena.ExtService.Common.UnitTests
{
    public class SystemHelperTest
    {
        [Theory]
        [InlineData("SB", "Sportsbook")]
        [InlineData("XXX", "")]
        public void GetProductName_WhenCall_ReturnCorrectResult(string productCode, string expectedValue)
        {
            //Arrange

            //Action
            var result = SystemHelper.GetProductName(productCode);

            //Assert

            Assert.Equal(expectedValue, result);
        }
    }
}
