namespace Fanex.Athena.ExtService.BetList.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Fanex.Athena.ExtService.BetList.Models.MetaModel;
    using Fanex.Athena.ExtService.Common.Models.DataDomain;
    using Fanex.Athena.ExtService.Common.Services;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class SingleMatchParlayBetListController : BaseController
    {
        private readonly IBetListApiClient _betListApiClient;

        public SingleMatchParlayBetListController(IBetListApiClient betListApiClient)
        {
            _betListApiClient = betListApiClient;
        }

        [ResponseCache(Duration = SystemConstants.ThirtySecond)]
        [HttpGet]
        public async Task<IActionResult> Detail(SingleMatchParlayDetailMetaModel metaModel)
        {
            var result = await _betListApiClient.PostAsync<IList<string>>(
                "/SingleMatchParlayBetList/Detail",
                metaModel,
                metaModel.Lang).ConfigureAwait(false);
            return Json(result);
        }
    }
}