using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Fanex.Athena.ExtService.Common.Services
{
    public class BetListApiClient : IBetListApiClient
    {
        private const string LANGUAGEKEY = "Language";

        private readonly string _betListApiClientName;
        private readonly IHttpClientFactory _clientFactory;

        private static readonly JsonSerializerSettings _serializerSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            DateTimeZoneHandling = DateTimeZoneHandling.Unspecified
        };

        public BetListApiClient(string clientName, IHttpClientFactory clientFactory)
        {
            _betListApiClientName = clientName;
            _clientFactory = clientFactory;
        }

        private async Task<T> HandleAsync<T>(string path, object value,
           string language, Func<HttpResponseMessage, Task<T>> codeBlock)
        {
            if (codeBlock == null)
            {
                return await Task.FromResult(default(T));
            }
            var client = _clientFactory.CreateClient(_betListApiClientName);
            using (var cancelToken = new CancellationTokenSource())
            {
                using (var request = new HttpRequestMessage(HttpMethod.Post, new Uri(client.BaseAddress + path))
                {
                    Content = ToJsonContent(value)
                })
                {
                    request.Headers.Add(LANGUAGEKEY, language);
                    var result = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancelToken.Token);
                    result?.EnsureSuccessStatusCode();
                    return await codeBlock(result).ConfigureAwait(false);

                }
            }
        }

        public async Task<string> PostAsyncString(string path, object value, string language)
        {
            return await HandleAsync(path, value, language,
                async x =>
                {
                    using (x)
                    {
                        return await x.Content.ReadAsStringAsync();
                    }
                }
                ).ConfigureAwait(false);
        }

        public async Task<T> PostAsync<T>(string path, object value, string language)
        {
            return await HandleAsync(path, value, language, async x =>
            {
                using (x)
                {
                    using (var stream = await x.Content.ReadAsStreamAsync())
                    {
                        using (var streamReader = new StreamReader(stream))
                        using (var jsonTextReader = new JsonTextReader(streamReader))
                        {
                            var serializer = JsonSerializer.Create(_serializerSettings);
                            return serializer.Deserialize<T>(jsonTextReader);
                        }
                    }
                }
            }).ConfigureAwait(false);
        }

        private static HttpContent ToJsonContent(object obj)
        {
            using (var stringWriter = new StringWriter())
            {
                using (var jsonWriter = new JsonTextWriter(stringWriter))
                {
                    var serializer = JsonSerializer.Create(_serializerSettings);
                    serializer.Serialize(jsonWriter, obj);
                    return new StringContent(stringWriter.ToString(), Encoding.UTF8, "application/json");
                }
            }
        }
    }
}