using Newtonsoft.Json;

namespace Fanex.Athena.ExtService.Common.Models.DTO
{ 
    public class CurrencyInfo
    {
        [JsonProperty("Id")]
        public int CurrencyId { get; set; }
        [JsonProperty("Name")]
        public string CurrencyName { get; set; }
        [JsonProperty("aId")]
        public int AgentId { get; set; }
    }
}
