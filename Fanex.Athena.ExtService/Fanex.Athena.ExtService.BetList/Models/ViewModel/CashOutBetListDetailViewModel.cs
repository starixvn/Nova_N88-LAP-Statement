using System;
using System.Collections.Generic;
using System.Text;
using Fanex.Athena.ExtService.BetList.Models.DTO;
using Fanex.Athena.ExtService.Common.Models.ViewModels;
namespace Fanex.Athena.ExtService.BetList.Models.ViewModel
{
    public class CashOutBetListDetailViewModel : BaseViewModel
    {
        public TicketText OriginalTicketText { get; set; }

        public IEnumerable<CashOutTicket> CashOutTicketList { get; set; }

        public IEnumerable<CashOutTicketText> CashOutTicketTextList { get; set; }

        public bool IsCashOut { get; set; }

        public bool IsHighlightOriginalTicket { get; set; }
    }
}
