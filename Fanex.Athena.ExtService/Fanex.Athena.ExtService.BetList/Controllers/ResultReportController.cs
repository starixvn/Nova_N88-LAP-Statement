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

        [ResponseCache(Duration = SystemConstants.ThirtySecond, VaryByQueryKeys = new[] { "gameId", "platform", "lang" })]
        [HttpGet]
        public async Task<string> VirtualGamesResultPopup(string gameId, string platform, string lang)
        {
            var model = new
            {
                gamePlayId = gameId,
                languageCode = lang,
                platform,
            };
            return await _betListApiClient.PostAsyncString("/VirtualGamesResultPopup/GetDetailResult", model, lang).ConfigureAwait(false);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ShowRngCasinoSingleWalletResult([FromBody] dynamic data)
        {
            var result = await _betListApiClient.PostAsyncString(
                "/ResultReport/ShowRngCasinoSingleWalletResult",
                new { refNo = Convert.ToString(data.refNo) },
                Convert.ToString(data.lang)
                )
                .ConfigureAwait(false);

            return Content(result, "application/json");
        }

        [ResponseCache(Duration = SystemConstants.ThirtySecond, VaryByQueryKeys = new[] { "custId", "lang", "gameId", "gameRoundId" })]
        [HttpGet]
        public async Task<string> SBCResultPopup(string custId, string gameId, string gameRoundId, string siteId, string lang)
        {
            var model = new
            {
                custId,
                gameRoundId,
                gameId,
                siteId,
                languageCode = lang
            };
            return await _betListApiClient.PostAsyncString("/SabaClubResultPopup/Index", model, lang).ConfigureAwait(false);
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

        [ResponseCache(Duration = SystemConstants.ThirtySecond, VaryByQueryKeys = new[] { "transDesc", "lang", "matchCode", "betType" })]
        [HttpGet]
        public async Task<IActionResult> SexyGamingResult(string transDesc, string matchCode, string betType, string lang)
        {
            var model = new
            {
                transDesc,
                matchCode,
                betType,
                languageCode = lang
            };
            var data = await _betListApiClient.PostAsyncString("/SexyGamingResultPopup/Index", model, lang).ConfigureAwait(false);
            ViewBag.Data = data.Replace("{rootUrl}/", Url.Content("~/"));
            var viewModel = new BaseViewModel
            {
                AccessToken = HttpContext.Request.Query["access_token"],
                Lang = lang
            };

            ViewBag.Title = "Result";

            return View("~/Views/ResultReport/AllBetResult.cshtml", viewModel);
        }

        [ResponseCache(Duration = SystemConstants.ThirtySecond, VaryByQueryKeys = new[] { "custId", "lang", "gameId", "gameRoundId" })]
        [HttpGet]
        public async Task<string> VGamingResultPopup(string custId, string gameId, string gameRoundId, int sportType, string siteId, string currencyName, string lang)
        {
            var model = new
            {
                custId,
                gameRoundId,
                gameId,
                siteId,
                sportType,
                currencyName,
                languageCode = lang
            };
            return await _betListApiClient.PostAsyncString("/VGamingResultPopup/Index", model, lang).ConfigureAwait(false);
        }

        [ResponseCache(Duration = SystemConstants.ThirtySecond, VaryByQueryKeys = new[] { "custId", "transId3rd", "gameId3rd", "currencyId", "siteId", "betTypeId", "productPath", "lang" })]
        [HttpGet]
#pragma warning disable S107 // Methods should not have too many parameters
        public async Task<string> ThirdPartyProduct(
            string custId,
            string transId3rd,
            string gameId3rd,
            string currencyId,
            string siteId,
            string betTypeId,
            string productPath,
            string lang)
#pragma warning restore S107 // Methods should not have too many parameters
        {
            var model = new
            {
                custId,
                transId3rd,
                gameId3rd,
                currencyId,
                siteId,
                betTypeId,
                languageCode = lang
            };

            return await _betListApiClient.PostAsyncString($"/{productPath}ResultPopup/Index", model, lang).ConfigureAwait(false);
        }
    }
}