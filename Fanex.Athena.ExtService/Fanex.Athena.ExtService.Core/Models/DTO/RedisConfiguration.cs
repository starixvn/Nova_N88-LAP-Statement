using System;
using System.Collections.Generic;
using System.Text;

namespace Fanex.Athena.ExtService.Core.Models.DTO
{
    public class RedisConfiguration
    {
        public string Server { get; set; }
        public int Port { get; set; }
        public string Pass { get; set; }
        public string AppName { get; set; }
    }
}
