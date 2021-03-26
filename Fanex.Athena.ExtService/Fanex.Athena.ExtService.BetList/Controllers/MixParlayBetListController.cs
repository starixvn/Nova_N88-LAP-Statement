namespace Fanex.Athena.ExtService.BetList.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Fanex.Athena.ExtService.BetList.Models.DTO;
    using Fanex.Athena.ExtService.BetList.Models.MetaModel;
    using Fanex.Athena.ExtService.BetList.Models.ViewModel;
    using Fanex.Athena.ExtService.Common.Attributes;
    using Fanex.Athena.ExtService.Common.Models.DataDomain;
    using Fanex.Athena.ExtService.Common.Services;
    using Fanex.Athena.ExtService.Core.Constant;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class MixParlayBetListController : BaseController
    {
        private readonly IBetListApiClient _betListApiClient;

        public MixParlayBetListController(IBetListApiClient betListApiClient)
        {
            _betListApiClient = betListApiClient;
        }

        [ResponseCache(Duration = SystemConstants.ThirtySecond)]
        [HttpGet]
        public async Task<IActionResult> Detail(MixParlayDetailMetaModel metaModel)
        {
            metaModel.IsShowIpvn = IsShowIpvn();
            var result = await _betListApiClient.PostAsync<IList<TicketText>>("/MixParlayBetList/Detail",
                metaModel,
                metaModel.Lang).ConfigureAwait(false);
            return Json(result);
        }

        [ResponseCache(Duration = SystemConstants.ThirtySecond)]
        [ServiceFilter(typeof(CheckCustomerAttribute))]
        [HttpGet]
        public async Task<IActionResult> NewParlayDetail(
            NewParlayDetailMetaModel metaModel)
        {
            metaModel.IsShowIpvn = IsShowIpvn();
            var result = await _betListApiClient.PostAsync<NewParlayDetailViewModel>("/MixParlayBetList/NewParlayDetail",
                metaModel, metaModel.Lang).ConfigureAwait(false);
            return Json(result);
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
