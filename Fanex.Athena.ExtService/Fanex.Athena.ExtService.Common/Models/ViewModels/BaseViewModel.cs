using Fanex.Athena.ExtService.Common.Models.DTO;

namespace Fanex.Athena.ExtService.Common.Models.ViewModels
{
    public class BaseViewModel
    {
        public LicenseeInfo Info { get; set; }
        public string AccessToken { get; set; }
        public string Lang { get; set; }
        public string BaseCurrency { get; set; }
    }
}
