using System.Collections.Generic;

using Fanex.Athena.ExtService.BetList.Models.DTO;
using Fanex.Athena.ExtService.Common.Models.ViewModels;

namespace Fanex.Athena.ExtService.BetList.Models.ViewModel
{
    public class RefNoViewModel : BaseViewModel
    {
        public string UserName { get; set; }
        public IList<TicketText> Data { get; set; }
    }
}
