using Fanex.Athena.ExtService.Core.Models.DTO;
using Fanex.Athena.ExtService.Core.Services;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Fanex.Athena.ExtService.Core.UnitTest.Services
{
    public class UserManagementServiceTest
    {
        private readonly IUserManagementService _userManagementService;
        private readonly List<User> users = new List<User> { new User { UserName = "alex", Password = "1234" } };
        public UserManagementServiceTest()
        {
            var mock = new Mock<IOptions<List<User>>>();
            mock.Setup(x => x.Value).Returns(users);
            _userManagementService = new UserManagementService(mock.Object);
        }

        [Theory]
        [InlineData("alex", "123", false)]
        [InlineData(null, null, false)]
        [InlineData(null, "1234", false)]
        [InlineData("alex", "1234", true)]
        public void IsValidUser_WhenCalled_ReturnCorrectResult(string userName, string pass, bool expectedResult)
        {
            //Arrange

            //Action
            var result = _userManagementService.IsValidUser(userName, pass);

            //Assert

            Assert.Equal(expectedResult, result);
        }


    }
}
