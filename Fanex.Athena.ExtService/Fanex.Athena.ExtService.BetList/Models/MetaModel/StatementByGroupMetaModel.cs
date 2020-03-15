using System;
using Fanex.Athena.ExtService.BetList.Models.DataDomain;
using Microsoft.AspNetCore.Mvc;

namespace Fanex.Athena.ExtService.BetList.Models.MetaModel
{
    public class StatementByGroupMetaModel : SummaryStatementMetaModel
    {
        [FromQuery(Name = "wd")]
        public DateTime WinlostDate { get; set; }

        [FromQuery(Name = "mv")]
        public int ModeView { get; set; }

        public new bool IsValid => CustId > 0 && TransDescValue.MaptoModeView.Values.Contains(ModeView)
            && WinlostDate > DateTime.MinValue;
    }
}