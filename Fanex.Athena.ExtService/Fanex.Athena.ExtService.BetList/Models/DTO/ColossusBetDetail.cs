using System;
using System.Collections.Generic;
using System.Text;

namespace Fanex.Athena.ExtService.BetList.Models.DTO
{
   public class ColossusBetDetail
    {
        public DateTime TransDate { get; set; }

        public string TransactionType { get; set; }

        public string WinLoss { get; set; }

        public decimal Commission { get; set; }
    }
}
