using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Fanex.Athena.ExtService.BetList.Models.DTO;
using Fanex.Athena.ExtService.Common.Models.ViewModels;

namespace Fanex.Athena.ExtService.BetList.Models.ViewModel
{
    public class NewCashOutBetListDetailViewModel : BaseViewModel
    {
        public long TransId { get; set; }

        public string CashoutExtraInfo { get; set; }

        public string Currency { get; set; }

        public decimal Rate { get; set; }

        public List<CashoutHistoryDetail> DetailHistory
        {
            get
            {
                var detailHistory = new List<CashoutHistoryDetail>();
                if (!string.IsNullOrEmpty(CashoutExtraInfo))
                {
                    var root = XElement.Parse(CashoutExtraInfo);
                    foreach (XElement item in root.Elements())
                    {
                        var time = item.Attribute("ct").Value;
                        var amount = item.Attribute("w").Value;
                        var action = item.Attribute("s").Value;

                        detailHistory.Add(new CashoutHistoryDetail(DateTime.Parse(time), decimal.Parse(amount) * Rate, action));
                    }
                }

                return detailHistory;
            }
        }
    }
}