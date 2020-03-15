using System;
using Fanex.Athena.ExtService.BetList.Models.DataDomain;
using Fanex.Athena.ExtService.BetList.Models.DTO;
using Fanex.Athena.ExtService.Common;

namespace Fanex.Athena.ExtService.BetList.Helper
{
    public static class StatementHelper
    {
        public static decimal GetStatementBalance(Decimal previousBalance, IStatementValue item)
        {
            switch (item.TransType?.ToUpperInvariant())
            {
                case TransTypeValue.BAL:
                    return item.TotalAmt;
                case TransTypeValue.GP:
                    return previousBalance + item.TotalAmt;
                case TransTypeValue.BET:
                    return GetBalanceForBETTranstype(previousBalance, item);
                default: return previousBalance + item.TotalAmt;
            }
        }

        public static decimal GetBettingBalance(Decimal previousBalance, IStatementValue item)
        {
            return item.TransDesc == TransDescValue.PromotionPrize
                ? previousBalance + item.TotalAmt
                : GetBalanceForBETTranstype(previousBalance, item);
        }

        private static decimal GetBalanceForBETTranstype(decimal previousBalance, IStatementValue item)
        {
            switch (item.TransDesc)
            {
                case TransDescValue.BetStatement:
                    return previousBalance + item.TotalAmt + item.Commission;
                case TransDescValue.Outstanding:
                    return previousBalance - item.TurnOver;
                case TransDescValue.VoidCancell:
                    return previousBalance;
                default:
                    return 0;
            }
        }

        public static decimal GetCreditDebit(IStatementValue item)
        {
            return item.TransDesc.Equals(TransDescValue.Outstanding, StringComparison.OrdinalIgnoreCase)
                || item.TransType.Equals(TransTypeValue.BAL) ? 0 : item.TotalAmt;
        }

        public static bool HideBetListLink(string productCode)
        {
            const string AGCasino = "AGC";
            const string GoldDeluxe = "GD";
            const string Exchange = "BX";

            return productCode == AGCasino || productCode == GoldDeluxe || productCode == Exchange;
        }

        public static string GetStatementRemark(string transDesc, string transType)
        => TransDescValue.RemarkValues.ContainsKey(transDesc)
                ? $"- {TransDescValue.RemarkValues[transDesc].Invoke()}"
                : transType.ToUpperInvariant();

        public static string GetTransDescText(string transDesc)
        => TransDescValue.RemarkValues.ContainsKey(transDesc)
                ? TransDescValue.RemarkValues[transDesc].Invoke()
                : string.Empty;

        public static string GetBetingStatementRemark(string transDescText, string productCode)
        {
            const string SabaPromotion = "SPR";

            return productCode == SabaPromotion
                ? SystemHelper.GetProductName(productCode)
                : $"{SystemHelper.GetProductName(productCode)} {transDescText}";
        }

        public static bool ShowBetingStatementLink(string transDesc, string transType)
        => (transType == TransTypeValue.BET || transType == TransTypeValue.PROMO) && TransDescValue.RemarkValues.ContainsKey(transDesc);
    }
}
