namespace Fanex.Athena.ExtService.BetList.Controllers
{
    using System.Collections.Generic;
    using Fanex.Athena.ExtService.BetList.Models.MetaModel;
    using Fanex.Athena.ExtService.Common.Models.DataDomain;
    using Fanex.Athena.ExtService.Common.Models.ViewModels;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;

    public class RunningBetListMetaModel : IBaseBetListMetaModel, ICustomerMetaModel
    {
        public string Lang { get; set; } = SystemConstants.DefaultLangCode;

        public bool IsValid
        {
            get
            {
                return CustId > 0 && PageIndex > 0 && PageSize > 0;
            }
        }

        [FromQuery(Name = "pi")]
        public int PageIndex { get; set; } = 1;

        [FromQuery(Name = "ps")]
        public int PageSize { get; set; } = SystemConstants.DefaultPageSize;

        [FromQuery(Name = "cid")]
        public int CustId { get; set; }

        public bool HasPlayerCom { get; set; }

        public string AgentListXml { get; set; }

        public string BaseCurrency { get; set; } = SystemConstants.DefaultBaseCurrency;
    }
}