using System;
using System.Threading.Tasks;
using Fanex.Athena.ExtService.BetList.Models.MetaModel;
using Fanex.Athena.ExtService.BetList.Models.ViewModel;
using Fanex.Athena.ExtService.Common.Services;
using Fanex.Athena.ExtService.Core.Constant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fanex.Athena.ExtService.BetList.Controllers
{
    [Authorize]
    public class BetListDetailByProductController : BaseController
    {
        private readonly ICommonService _commonService;
        private readonly IBetListApiClient _betListApiClient;

        public BetListDetailByProductController(ICommonService commonService, IBetListApiClient betListApiClient)
        {
            _commonService = commonService;
            _betListApiClient = betListApiClient;
        }

        [HttpGet]
        public async Task<IActionResult> Detail(BetListDetailByProductMetaModel metaModel)
        {
            var licenseeInfo = await _commonService.GetLicenseeInfoAsync(HttpContext.User.Identity.Name, metaModel.CustId);
            metaModel.IsShowIpvn = IsShowIpvn();
            AssignCommonMetaModelValue(metaModel, licenseeInfo);
            var result =
                 await _betListApiClient.PostAsync<BetListResultViewModel>("/BetList/GetBetList", metaModel, metaModel.Lang).ConfigureAwait(false);
            result.HasTotalRow = metaModel.HasTotal;
            result.HasLicWl = metaModel.HasLicWl;
            AssignCommonViewModelData(metaModel, result);

            ViewBag.Title = "Bet List - Product";
            result.Info = licenseeInfo;
            return View(result);
        }

        private bool IsShowIpvn()
        {
            if (string.IsNullOrEmpty(HttpContext.User.FindFirst(CustomClaimType.IsShowIPVN)?.Value))
            {
                return true;
            }
            return Convert.ToBoolean(HttpContext.User.FindFirst(CustomClaimType.IsShowIPVN).Value);
        }
    }
}