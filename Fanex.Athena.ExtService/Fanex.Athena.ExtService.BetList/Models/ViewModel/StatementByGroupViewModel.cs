using System.Collections.Generic;

using Fanex.Athena.ExtService.BetList.Models.DTO;
using Fanex.Athena.ExtService.Common.Models.ViewModels;

namespace Fanex.Athena.ExtService.BetList.Models.ViewModel
{
    public class StatementByGroupViewModel : BaseViewModel
    {
        public IEnumerable<StatementByGroup> DataList { get; internal set; }

        public string TransDesc { get; set; }

        public int CustId { get; set; }

        public int ModeView { get; set; }
    }
}