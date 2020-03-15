using System;

using Fanex.Athena.ExtService.BetList.Models.DataDomain;
using Fanex.Athena.ExtService.BetList.Models.MetaModel;
using Microsoft.AspNetCore.Mvc;

namespace Fanex.Athena.ExtService.BetList.Controllers
{
    public class BetListByGroupMetaModel : StatementByGroupMetaModel, IBaseBetListMetaModel
    {
        public new bool IsValid
        {
            get
            {
                var pagingValidation = PageIndex > 0 && PageSize > 0;
                var mainValidation = CustId > 0 && WinlostDate > DateTime.MinValue;
                var productAndModeValidation = !string.IsNullOrEmpty(ProductCode) && (TransType == 0 || TransDescValue.MaptoModeView.Values.Contains(TransType));
                return pagingValidation && mainValidation && productAndModeValidation;
            }
        }

        [FromQuery(Name = "pc")]
        public string ProductCode { get; set; }

        [FromQuery(Name = "s")]
        public string Sort { get; set; } = "Transid ASC";

        [FromQuery(Name = "spi")]
        public int StatementPageIndex { get; set; }

        [FromQuery(Name = "sps")]
        public int StatementPageSize { get; set; }

        public int TransType { get { return ModeView; } }

        public bool HasPlayerCom { get; set; }

        public string AgentListXml { get; set; }
    }
}