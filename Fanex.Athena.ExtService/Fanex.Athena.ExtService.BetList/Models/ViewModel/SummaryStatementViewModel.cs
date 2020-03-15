namespace Fanex.Athena.ExtService.BetList.Models.ViewModel
{
    using System;
    using System.Collections.Generic;
    using Fanex.Athena.ExtService.BetList.Models.DTO;
    using Fanex.Athena.ExtService.Common.Models.ViewModels;

    public class SummaryStatementViewModel : BaseViewModel
    {
        public IEnumerable<SummaryStatement> DataList { get; set; }

        public int TotalRows { get; set; }

        public int CustId { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }
    }
}
