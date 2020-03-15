using System.Threading.Tasks;

using Fanex.Athena.ExtService.Common.Models.DTO;

namespace Fanex.Athena.ExtService.Common.Services
{
    public interface ICommonService
    {
        Task<LicenseeInfo> GetLicenseeInfoAsync(string siteName, int custId = 0);
        Task<BalanceUp> GetBalanceUpAsync();

    }
}
