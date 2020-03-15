using System;
using System.Collections.Generic;
using Fanex.Athena.ExtService.BetList.Models.DTO;
using Fanex.Athena.ExtService.Common.Models.ViewModels;

namespace Fanex.Athena.ExtService.BetList.Models.ViewModel
{
    public class BetListByProductViewModel : BaseViewModel
    {
        public string ProductCode { get; set; }

        public string TransDesc { get; set; }

        public IList<TicketText> Data { get; set; }

        public int CustId { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int TransType { get; set; }

        public string Sort { get; set; }

        public DateTime WinlostDate { get; set; }

        public int TotalRows { get; set; }
    }
}
