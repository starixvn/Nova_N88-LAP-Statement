using System;
using Fanex.Athena.ExtService.BetList.Models.DataDomain;

namespace Fanex.Athena.ExtService.BetList.Models.DTO
{
    public class StatementByGroup : IStatementValue
    {
        public DateTime WinLostDate { get; set; }
        public string ProductCode { get; set; }
        public string TransDesc { get; set; }

        public decimal TurnOver { get; set; }
        public decimal TotalAmt { get; set; }
        public decimal Commission { get; set; }

        public string TransType { get; set; } = TransTypeValue.BET;
    }
}