using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Fanex.Athena.ExtService.Core.Models
{
    [JsonObject("token")]
    public class Token
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

    }
}
