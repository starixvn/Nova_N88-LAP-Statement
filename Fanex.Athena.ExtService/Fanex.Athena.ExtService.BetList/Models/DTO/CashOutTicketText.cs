using System;
using System.Collections.Generic;
using System.Text;

namespace Fanex.Athena.ExtService.BetList.Models.DTO
{
    public class CashOutTicketText
    {
        public string TransTime { get; set; }

        public string Stake { get; set; }

        public string Price { get; set; }

        public string WinLoss { get; set; }

        public string Status { get; set; }

        public bool IsHighlight { get; set; }
    }
}
