using System;
using System.Threading.Tasks;

using Fanex.Athena.ExtService.BetList.Models.MetaModel;
using Fanex.Athena.ExtService.Common.Models.DataDomain;
using Fanex.Athena.ExtService.Common.Models.ViewModels;
using Fanex.Athena.ExtService.Common.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fanex.Athena.ExtService.BetList.Controllers
{
    [Authorize]
    public class ResultReportController : BaseController
    {
        private readonly IBetListApiClient _betListApiClient;

        public ResultReportController(IBetListApiClient betListApiClient)
        {
            _betListApiClient = betListApiClient;
        }

        [HttpGet]
        [ResponseCache(Duration = SystemConstants.ThirtySecond)]
        public async Task<IActionResult> ResultPopup(ResultPopupMetalModel metaModel)
        {

            var result = await _betListApiClient.PostAsyncString(
                 "/ResultReport/ResultPopup",
                 metaModel,
                 metaModel.Lang
                 ).ConfigureAwait(false);

            ViewBag.Data = result;
            var viewModel = new BaseViewModel
            {
                AccessToken = HttpContext.Request.Query["access_token"],
                Lang = metaModel.Lang
            };

            ViewBag.Title = "Result";

            return View(viewModel);


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ShowRngCasinoSingleWalletResult([FromBody]dynamic data)
        {
            var result = await _betListApiClient.PostAsyncString(
                "/ResultReport/ShowRngCasinoSingleWalletResult",
                new { refNo = Convert.ToString(data.refNo) },
                Convert.ToString(data.lang)
                )
                .ConfigureAwait(false);

            return Content(result, "application/json");
        }

        [ResponseCache(Duration = SystemConstants.ThirtySecond, VaryByQueryKeys = new[] { "custId", "lang", "gamePlayId" })]
        [HttpGet]
        public async Task<string> VoidbridgeResult(string custId, string gamePlayId, string lang)
        {
            var model = new
            {
                custId,
                gamePlayId,
                languageCode = lang
            };
            return await _betListApiClient.PostAsyncString("/VoidbridgeResultPopup/Index",
                model, lang).ConfigureAwait(false);
        }

        [ResponseCache(Duration = SystemConstants.ThirtySecond, VaryByQueryKeys = new[] { "custId", "lang", "gamePlayId" })]
        [HttpGet]
        public async Task<string> PPResultPopup(string custId, string gamePlayId, string lang)
        {
            var model = new
            {
                custId,
                gamePlayId,
                languageCode = lang
            };
            return await _betListApiClient.PostAsyncString("/PPResultPopup/Index", model, lang).ConfigureAwait(false);
        }

        [ResponseCache(Duration = SystemConstants.ThirtySecond, VaryByQueryKeys = new[] { "custId", "lang", "gamePlayId" })]
        [HttpGet]
        public async Task<string> SGResultPopup(string custId, string gamePlayId, string lang)
        {
            var model = new
            {
                custId,
                gamePlayId,
                languageCode = lang
            };
            return await _betListApiClient.PostAsyncString("/SGResultPopup/Index", model, lang).ConfigureAwait(false);
        }

        [ResponseCache(Duration = SystemConstants.ThirtySecond, VaryByQueryKeys = new[] { "custId", "lang", "gameTypeId", "transId3rd" })]
        [HttpGet]
        public async Task<string> CCResultPopup(string custId, string gameTypeId, string transId3rd, string lang)
        {
            var model = new
            {
                custId,
                gameTypeId,
                transId3rd,
                languageCode = lang
            };
            return await _betListApiClient.PostAsyncString("/CardClubResultPopup/Index", model, lang).ConfigureAwait(false);
        }

        [ResponseCache(Duration = SystemConstants.ThirtySecond, VaryByQueryKeys = new[] { "transDesc", "lang", "betCheck", "betType" })]
        [HttpGet]
        public async Task<IActionResult> AllBetResult(string transDesc, string betCheck, string betType, string lang)
        {
            var model = new
            {
                transDesc,
                betCheck,
                betType,
                languageCode = lang
            };
            var data = await _betListApiClient.PostAsyncString("/AllBetResultPopup/Index", model, lang).ConfigureAwait(false);
            ViewBag.Data = data.Replace("{rootUrl}/", Url.Content("~/"));
            var viewModel = new BaseViewModel
            {
                AccessToken = HttpContext.Request.Query["access_token"],
                Lang = lang
            };

            ViewBag.Title = "Result";

            return View(viewModel);
        }
        [ResponseCache(Duration = SystemConstants.ThirtySecond, VaryByQueryKeys = new[] { "custId", "lang", "gameId" })]
        [HttpGet]
        public async Task<string> GlobalGamingResult(string custId, string gameId, string lang)
        {
            var model = new
            {
                custId,
                gameId
            };
            return await _betListApiClient.PostAsyncString("/GlobalGamingResultPopup/Index",
                model, lang).ConfigureAwait(false);
        }

    }
}
