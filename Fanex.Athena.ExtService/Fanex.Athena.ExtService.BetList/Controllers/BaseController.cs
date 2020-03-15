namespace Fanex.Athena.ExtService.BetList.Controllers
{
    using Fanex.Athena.ExtService.BetList.Models.MetaModel;
    using Fanex.Athena.ExtService.Common;
    using Fanex.Athena.ExtService.Common.Models.DTO;
    using Fanex.Athena.ExtService.Common.Models.ViewModels;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class BaseController : Controller
    {
        protected  void AssignCommonViewModelData(IBaseMetaModel metaModel, BaseViewModel viewModel)
        {
            viewModel.BaseCurrency = metaModel.BaseCurrency;
            viewModel.AccessToken = HttpContext.Request.Query["access_token"];
            viewModel.Lang = metaModel.Lang;

        }

        protected static void AssignCommonMetaModelValue(IBaseBetListMetaModel metaModel, LicenseeInfo licenseeInfo)
        {
            metaModel.HasPlayerCom = licenseeInfo.IsAllowedCustomerComm;
            metaModel.AgentListXml = SystemHelper.GenerateAgentListXml(licenseeInfo);
        }
    }
}
