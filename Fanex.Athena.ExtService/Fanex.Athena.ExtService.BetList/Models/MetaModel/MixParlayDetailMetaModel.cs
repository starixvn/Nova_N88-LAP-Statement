using System;
using System.Collections.Generic;
using System.Text;
using Fanex.Athena.ExtService.Common.Models.DataDomain;
using Fanex.Athena.ExtService.Common.Models.ViewModels;

namespace Fanex.Athena.ExtService.BetList.Models.MetaModel
{
    public class MixParlayDetailMetaModel : ICustomerMetaModel
    {
        public long TransId { get; set; }

        public int CustId { get; set; }
        public string Lang { get; set; } = SystemConstants.DefaultLangCode;
    }
}
