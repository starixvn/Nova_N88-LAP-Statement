using System;
using System.Linq;
using System.Threading.Tasks;

using Fanex.Athena.ExtService.Common.Models.ViewModels;
using Fanex.Athena.ExtService.Common.Services;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Fanex.Athena.ExtService.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class CheckCustomerAttribute : ActionFilterAttribute
    {
        private readonly ICommonService _commonService;
        public CheckCustomerAttribute(ICommonService commonService)
        {
            _commonService = commonService;

        }
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var siteName = context.HttpContext.User.Identity.Name;
            var metaModel = context.ActionArguments.Where(x => x.Value is ICustomerMetaModel)
                      .Select(x => x.Value as ICustomerMetaModel).FirstOrDefault();

            var isValid = metaModel != null && metaModel.CustId > 0;

            if (isValid)
            {
                isValid = (await _commonService.GetLicenseeInfoAsync(siteName, metaModel.CustId)).Currencies.Any();
            }

            if (isValid)
            {
                await next();
            }
            else
            {
                var result = new ContentResult
                {
                    Content = "Not found any customers that match your query!"
                };
                context.Result = result;
            }

        }
    }
}
