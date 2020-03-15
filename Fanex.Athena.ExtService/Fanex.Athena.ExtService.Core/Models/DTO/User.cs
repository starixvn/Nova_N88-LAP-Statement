using System;

namespace Fanex.Athena.ExtService.Core.Models.DTO
{
    [Serializable]
    public class User
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
