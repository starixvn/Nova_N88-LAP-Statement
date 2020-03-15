using System.Threading.Tasks;

namespace Fanex.Athena.ExtService.Common.Services
{
    public interface IBetListApiClient
    {
        Task<T> PostAsync<T>(string path, object value, string language);

        Task<string> PostAsyncString(string path, object value, string language);
    }
}