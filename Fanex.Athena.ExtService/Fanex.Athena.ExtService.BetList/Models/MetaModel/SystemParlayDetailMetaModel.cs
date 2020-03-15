using System;
using Fanex.Athena.ExtService.Common.Models.DataDomain;

namespace Fanex.Athena.ExtService.BetList.Models.MetaModel
{
    public class SystemParlayDetailMetaModel : IBaseMetaModel
    {
        public long RefNo { get; set; }

        public DateTime MoveBetsDay { get; set; }

        public DateTime BalanceUpDay { get; set; }
        public string BaseCurrency { get; set; } = SystemConstants.DefaultBaseCurrency;
        public string Lang { get; set; } = SystemConstants.DefaultLangCode;

        public string AgentListXml { get; set; }
    }
}