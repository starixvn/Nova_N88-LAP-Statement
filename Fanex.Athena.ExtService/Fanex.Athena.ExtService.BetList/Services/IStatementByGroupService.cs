using System.Threading.Tasks;

using Fanex.Athena.ExtService.BetList.Models.MetaModel;
using Fanex.Athena.ExtService.BetList.Models.ViewModel;

namespace Fanex.Athena.ExtService.BetList.Services
{
    public interface IStatementByGroupService
    {
        Task<StatementByGroupViewModel> QueryAsync(StatementByGroupMetaModel metaModel);

    }
}
