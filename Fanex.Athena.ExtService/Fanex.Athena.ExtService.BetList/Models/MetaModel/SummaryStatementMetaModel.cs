using System;

using Fanex.Athena.ExtService.Common.Models.DataDomain;
using Fanex.Athena.ExtService.Common.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Fanex.Athena.ExtService.BetList.Models.MetaModel
{
    public class SummaryStatementMetaModel : IBaseMetaModel, ICustomerMetaModel
    {
        const int TotalDaysLimit = 31;

        [FromQuery(Name = "cid")]
        public int CustId { get; set; }

        [FromQuery(Name = "fd")]
        public DateTime FromDate { get; set; }

        [FromQuery(Name = "td")]
        public DateTime ToDate { get; set; }

        [FromQuery(Name = "ps")]
        public int PageSize { get; set; } = SystemConstants.DefaultPageSize;

        [FromQuery(Name = "pi")]
        public int PageIndex { get; set; } = 1;

        [FromQuery(Name = "bc")]
        public string BaseCurrency { get; set; } = SystemConstants.DefaultBaseCurrency;

        [FromQuery(Name = "sb")]
        public bool ShowBreadCrumb { get; set; }

        public string Lang { get; set; } = SystemConstants.DefaultLangCode;

        public bool IsValid
        {
            get
            {
                var pagingValidation = PageIndex > 0 && PageSize > 0;
                var mainValidation = CustId > 0 && FromDate <= ToDate && (ToDate - FromDate).TotalDays <= TotalDaysLimit;

                return pagingValidation && mainValidation;

            }
        }
    }
}