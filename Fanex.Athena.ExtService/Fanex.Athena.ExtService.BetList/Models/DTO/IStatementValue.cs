namespace Fanex.Athena.ExtService.BetList.Models.DTO
{
    using System;

    public interface IStatementValue
    {
         string TransType { get; set; }
         string TransDesc { get; set; }
         Decimal TurnOver { get; set; }
         Decimal TotalAmt { get; set; }
         Decimal Commission { get; set; }
    }
}
