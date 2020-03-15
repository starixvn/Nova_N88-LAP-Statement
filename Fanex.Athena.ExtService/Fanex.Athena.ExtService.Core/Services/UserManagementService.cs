using Fanex.Athena.ExtService.Core.Models.DTO;

using Microsoft.Extensions.Options;

using System.Collections.Generic;
using System.Linq;

namespace Fanex.Athena.ExtService.Core.Services
{
    public class UserManagementService : IUserManagementService
    {
        private readonly List<User> _users;

        public UserManagementService(IOptions<List<User>> userListAccessor)
        {
            _users = userListAccessor.Value;
        }
        public bool IsValidUser(string username, string password)
        {
            return _users.Any(x => (
            x.UserName.Equals(username)
            )
            && x.Password.Equals(password));
        }
    }
}
