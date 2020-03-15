using System;
using System.Collections.Generic;
using System.Text;
using Fanex.Athena.ExtService.Common.Models.DataDomain;
using Newtonsoft.Json;

namespace Fanex.Athena.ExtService.BetList.Models.MetaModel
{
    public class CashOutBetListDetailMetaModel : IBaseBetListMetaModel
    {
        public long TransId { get; set; }

        public long RefNo { get; set; }

        public int BetType { get; set; }

        public long BetdaqId { get; set; }

        public bool IsCashOut { get; set; }

        public string AgentListXml { get; set; }
        public string BaseCurrency { get; set; }
        public string Lang { get; set; } = SystemConstants.DefaultLangCode;
        [JsonProperty("HasPlayerComm")]
        public bool HasPlayerCom { get; set; }
    }
}
