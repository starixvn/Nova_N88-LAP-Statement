using System;
using System.Collections.Generic;
using System.Text;

namespace Fanex.Athena.ExtService.BetList.Models.DTO
{
    public class BetListTotalData
    {
        public decimal SumWinLoss { get; set; }

        public decimal SumLicenseeWl { get; set; }

        public decimal SumBetEffective { get; set; }

        public decimal SumStake { get; set; }
    }
}
