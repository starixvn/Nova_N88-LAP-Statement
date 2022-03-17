using System;
using Fanex.Athena.ExtService.Common.Models.ViewModels;

namespace Fanex.Athena.ExtService.BetList.Models.MetaModel
{
    public class ResultPopupMetalModel : ICustomerMetaModel
    {
        public int MatchId
        {
            get;
            set;
        }

        public string Lang { get; set; }

        public int Bettype { get; set; }

        public int? RaceNo { get; set; }

        public int SportType { get; set; }

        public int? IsOutRight { get; set; }

        public long? RefNo { get; set; }

        public DateTime? WinLossDate { get; set; }

        public long? BetId { get; set; }

        public int CustId { get; set; }

        public int BetCheck { get; set; }

        public int Handicap1 { get; set; }

        public int Handicap2 { get; set; }

        public string TransDesc { get; set; }
    }
}