using System;
using System.Collections.Generic;
using System.Text;
using Fanex.Athena.ExtService.Common.Models.DataDomain;
using Fanex.Athena.ExtService.Common.Models.ViewModels;
using Newtonsoft.Json;

namespace Fanex.Athena.ExtService.BetList.Models.MetaModel
{
    public class BetListDetailByProductMetaModel : IBaseBetListMetaModel, ICustomerMetaModel
    {
        public long TransId { get; set; }

        public long TransType { get; set; }

        public int CustId { get; set; }

        public string UspKey { get; set; }

        public bool HasPlayerCom { get; set; }
        public bool HasTotal { get; set; }
        public bool HasLicWl { get; set; }
        public long RefCode { get; set; }
        
        public string AgentListXml { get; set; }
        public string BaseCurrency { get; set; } = SystemConstants.DefaultBaseCurrency;
        public string Lang { get; set; } = SystemConstants.DefaultLangCode;
    }
}
