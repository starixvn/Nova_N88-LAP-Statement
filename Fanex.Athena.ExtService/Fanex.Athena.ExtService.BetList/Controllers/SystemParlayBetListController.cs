using System.Threading.Tasks;
using Fanex.Athena.ExtService.BetList.Models.MetaModel;
using Fanex.Athena.ExtService.BetList.Models.ViewModel;
using Fanex.Athena.ExtService.Common;
using Fanex.Athena.ExtService.Common.Models.DataDomain;
using Fanex.Athena.ExtService.Common.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fanex.Athena.ExtService.BetList.Controllers
{
    [Authorize]
    public class SystemParlayBetListController : BaseController
    {
        private readonly ICommonService _commonService;
        private readonly IBetListApiClient _betListApiClient;

        public SystemParlayBetListController(ICommonService commonService, IBetListApiClient betListApiClient)
        {
            _commonService = commonService;
            _betListApiClient = betListApiClient;
        }

        [HttpGet]
        [ResponseCache(Duration = SystemConstants.ThirtySecond)]
        public async Task<IActionResult> Detail(SystemParlayDetailMetaModel metaModel)
        {
            var licenseeInfo = await _commonService.GetLicenseeInfoAsync(HttpContext.User.Identity.Name);
            metaModel.AgentListXml = SystemHelper.GenerateAgentListXml(licenseeInfo);

            var data = await _betListApiClient.PostAsyncString("/SystemParlayBetList/Detail", metaModel,
                metaModel.Lang).ConfigureAwait(false);

            var viewModel = new SystemParlayDetailViewModel
            {
                DetailText = data
            };

            return View(viewModel);
        }
    }
}