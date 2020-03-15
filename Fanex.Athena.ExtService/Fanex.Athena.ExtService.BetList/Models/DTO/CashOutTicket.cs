using System;
using System.Collections.Generic;
using System.Text;

namespace Fanex.Athena.ExtService.BetList.Models.DTO
{
    public class CashOutTicket
    {
        public int BetType { get; set; }

        public long TransId { get; set; }

        public string RefNo { get; set; }

        public string Status { get; set; }

        public DateTime TransDate { get; set; }

        public DateTime WinLostDate { get; set; }

        public int Currency { get; set; }

        public int CustId { get; set; }

        public decimal Stake { get; set; }

        public decimal Price { get; set; }

        public decimal WinLoss { get; set; }
    }
}
