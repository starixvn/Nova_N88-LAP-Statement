using System.Collections.Generic;

using Fanex.Athena.ExtService.BetList.Models.DTO;
using Fanex.Athena.ExtService.Common.Models.ViewModels;

namespace Fanex.Athena.ExtService.BetList.Models.ViewModel
{
    public class ColossusBetDetailViewModel : BaseViewModel
    {
        public IEnumerable<ColossusBetDetail> DataList { get; set; }

        public decimal TotalSoldPercent { get; set; }

        public decimal TotalWinLoss { get; set; }

        public decimal TotalComm { get; set; }

        public decimal TotalSoldWinLoss { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Sonar Code Smell", "S3996:UriPropertiesShouldNotBeStrings",
            Justification = "Reviewed. Suppression is OK here.")]
        public string IframeUrl { get; set; }
    }
}
