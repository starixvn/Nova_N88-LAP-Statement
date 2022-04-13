using Fanex.Athena.ExtService.Resources;
using System;
using System.Collections.Generic;

namespace Fanex.Athena.ExtService.BetList.Models.DTO
{
    public class CashoutHistoryDetail
    {
        public CashoutHistoryDetail(DateTime changeTime, decimal amount, string actionId)
        {
            ChangeTime = changeTime;
            CashoutAmount = amount;
            ActionId = actionId;
        }

        public DateTime ChangeTime { get; set; }

        public string ChangeTimeText => ChangeTime.ToString("M/d/yyyy hh:mm:ss tt");

        public decimal CashoutAmount { get; set; }

        public string CashoutAmountText => CashoutAmount.ToString("#,##0.00");

        public string ActionId { get; set; }

        public string ActionName

        {
            get
            {
                var actionList = new Dictionary<string, string>
                {
                    { "1", ViewResources.ProductCO },
                    { "6", ViewResources.CashOutCancelled },
                    { "8", ViewResources.CashOutRecovered }
                };

                return actionList.ContainsKey(ActionId) ? actionList[ActionId] : ActionId;
            }
        }
    }
}