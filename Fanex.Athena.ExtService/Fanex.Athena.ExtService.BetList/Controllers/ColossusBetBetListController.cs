using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Fanex.Athena.ExtService.BetList.Models.MetaModel;
using Fanex.Athena.ExtService.BetList.Models.ViewModel;
using Fanex.Athena.ExtService.Common;
using Fanex.Athena.ExtService.Common.Attributes;
using Fanex.Athena.ExtService.Common.Models.ViewModels;
using Fanex.Athena.ExtService.Common.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fanex.Athena.ExtService.BetList.Controllers
{
    [Authorize]
    public class ColossusBetBetListController : BaseController
    {
        private const int CacheIntTenSecond = 10;
        private readonly ICommonService _commonService;
        private readonly IBetListApiClient _betListApiClient;
        public ColossusBetBetListController(ICommonService commonService, IBetListApiClient betListApiClient)
        {
            _commonService = commonService;
            _betListApiClient = betListApiClient;
        }

        [HttpGet]
        [ResponseCache(Duration = CacheIntTenSecond)]
        [ServiceFilter(typeof(CheckCustomerAttribute))]
        public async Task<IActionResult> Detail(ColossusBetDetailMetaModel metaModel)
        {
            var licenseeInfo = await _commonService.GetLicenseeInfoAsync(HttpContext.User.Identity.Name, metaModel.CustId);
            metaModel.AgentListXml = SystemHelper.GenerateAgentListXml(licenseeInfo);

            var viewModel = await _betListApiClient.PostAsync<ColossusBetDetailViewModel>("/ColossusBetBetList/Detail",
                metaModel,
                metaModel.Lang
                ).ConfigureAwait(false);

            AssignCommonViewModelData(metaModel, viewModel);

            ViewBag.Title = "Colossus Bet";

            return View(viewModel);
        }


    }
}
