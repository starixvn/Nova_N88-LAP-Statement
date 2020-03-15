namespace Fanex.Athena.ExtService.BetList.Models.MetaModel
{
    using Fanex.Athena.ExtService.Common.Models.DataDomain;
    using Fanex.Athena.ExtService.Common.Models.ViewModels;

    public class SingleMatchParlayDetailMetaModel : ICustomerMetaModel
    {
        public long TransId { get; set; }

        public int CustId { get; set; }

        public string Status { get; set; }

        public string Lang { get; set; } = SystemConstants.DefaultLangCode;
    }
}
