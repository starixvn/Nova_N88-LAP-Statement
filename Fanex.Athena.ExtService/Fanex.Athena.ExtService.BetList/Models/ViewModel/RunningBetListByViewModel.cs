using System.Collections.Generic;
using Fanex.Athena.ExtService.BetList.Models.DTO;
using Fanex.Athena.ExtService.Common.Models.ViewModels;

namespace Fanex.Athena.ExtService.BetList.Models.ViewModel
{
    public class RunningBetListByViewModel : BaseViewModel
    {
        public IList<TicketText> Data { get; set; }
    }
}
