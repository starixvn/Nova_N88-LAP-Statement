using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    public class CashOutBetListController : BaseController
    {
        private readonly ICommonService _commonService;
        private readonly IBetListApiClient _betListApiClient;
        public CashOutBetListController(ICommonService commonService, IBetListApiClient betListApiClient)
        {
            _commonService = commonService;
            _betListApiClient = betListApiClient;
        }

        [HttpGet]
        [ResponseCache(Duration = SystemConstants.ThirtySecond)]
        public async Task<IActionResult> Detail(CashOutBetListDetailMetaModel metaModel)
        {
            var licenseeInfo = await _commonService.GetLicenseeInfoAsync(HttpContext.User.Identity.Name);

            AssignCommonMetaModelValue(metaModel, licenseeInfo);

            var viewModel = await _betListApiClient.PostAsync<CashOutBetListDetailViewModel>("/CashOutBetList/Detail",
                metaModel, metaModel.Lang).ConfigureAwait(false);
            viewModel.Info = licenseeInfo;

            AssignCommonViewModelData(metaModel, viewModel);
            ViewBag.Title = "Cash Out BetList";
            return View(viewModel);
        }
    }
}
