using System.Threading.Tasks;

using Fanex.Athena.ExtService.BetList.Models.DTO;
using Fanex.Athena.ExtService.BetList.Models.MetaModel;
using Fanex.Athena.ExtService.BetList.Models.ViewModel;
using Fanex.Athena.ExtService.Common.Services;
using Fanex.Data;

namespace Fanex.Athena.ExtService.BetList.Services
{
    public class StatementByGroupService : IStatementByGroupService
    {
        private readonly ICommonService _commonService;
        public StatementByGroupService(ICommonService commonService)
        {
            _commonService = commonService;
        }
        public async Task<StatementByGroupViewModel> QueryAsync(StatementByGroupMetaModel metaModel)
        {
            const string UspName = "LAP_Statement_Mem_Product";
            var balanceUpInfo = await _commonService.GetBalanceUpAsync().ConfigureAwait(false);
            using (var db = ObjectDbFactory.CreateInstance(UspName))
            {
                db.SetParameterValue("@CustID", metaModel.CustId);
                db.SetParameterValue("@Winlostdate", metaModel.WinlostDate);
                db.SetParameterValue("@BalanceUpDay", balanceUpInfo.BalanceUpDay);
                db.SetParameterValue("@MoveBetsDay", balanceUpInfo.MoveBetsDay);
                db.SetParameterValue("@baseCurrency", metaModel.BaseCurrency);
                db.SetParameterValue("@ModeView", metaModel.ModeView);
                var statementResult = await db.QueryAsync<StatementByGroup>();

                return new StatementByGroupViewModel
                {
                    DataList = statementResult,
                };
            }
        }
    }
}
