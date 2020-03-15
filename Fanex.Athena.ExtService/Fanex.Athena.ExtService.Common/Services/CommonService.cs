using Fanex.Athena.ExtService.Common.Models.DTO;
using Fanex.Data;
using Microsoft.Extensions.Caching.Distributed;
using MsgPack.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanex.Athena.ExtService.Common.Services
{
    public class CommonService : ICommonService
    {
        private const int CacheTimeout120Seconds = 120;
        private const int CacheTimeout300Seconds = 300;
        private const int CacheTimeOut30Seconds = 30;
        private const string BalanceUpCacheKey = "EXT_LAP_CACHE_BALANCEUP";
        private const string LicenseeExtInfoCacheKey = "__LicenseeExtInfoCacheKey_";
        private readonly IDistributedCache _cache;

        private static readonly SerializationContext _msgPackSerializationContext = new SerializationContext
        {
            DefaultDateTimeConversionMethod = DateTimeConversionMethod.Native
        };

        public CommonService(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<LicenseeInfo> GetLicenseeInfoAsync(string siteName, int custId = 0)
        {
            LicenseeInfo result = null;
            var realCacheKey = $"{LicenseeExtInfoCacheKey}{siteName}_{custId}";
            var cachedValue = await _cache.GetAsync(realCacheKey);

            if (cachedValue != null)
            {
                result = await UnpackAsync<LicenseeInfo>(cachedValue);
            }

            if (result is null)
            {
                result = await GetLicenseeInfoFromDBAsync(siteName, custId);
                if (result.Currencies.Any())
                {
                    var options = custId > 0
                        ? WithSlidingExpiration(TimeSpan.FromSeconds(CacheTimeOut30Seconds))
                        : WithAbsoluteExpiration(DateTimeOffset.Now.AddSeconds(CacheTimeout300Seconds));
                    await _cache.SetAsync(realCacheKey, await PackAsync(result), options);
                }
            }

            return result;
        }

        private static async Task<LicenseeInfo> GetLicenseeInfoFromDBAsync(string siteName, int custId)
        {
            const string UspName = "Lrf_LicenseeSelExtInfo";

            var result = new LicenseeInfo
            {
                SiteName = siteName,
                Currencies = new CurrencyInfo[] { }
            };
            using (var db = ObjectDbFactory.CreateInstance(UspName))
            {
                db.SetParameterValue(nameof(siteName), siteName);
                db.SetParameterValue(nameof(custId), custId);
                using (var reader = await db.ExecuteReaderAsync())
                {
                    var currencies = new List<CurrencyInfo>();
                    while (reader.Read())
                    {
                        currencies.Add(new CurrencyInfo
                        {
                            CurrencyId = Convert.ToInt32(reader["CurrencyId"]),
                            AgentId = Convert.ToInt32(reader["AgentId"]),
                            CurrencyName = Convert.ToString(reader["CurrencyName"])

                        });
                    }
                    result.Currencies = currencies.ToArray();
                    if (reader.NextResult() && reader.Read())
                    {
                        result.IsAllowedCustomerComm = Convert.ToBoolean(reader["IsAllowedCustomerComm"]);
                    }

                }
            }

            return result;
        }

        public async Task<BalanceUp> GetBalanceUpAsync()
        {
            var cachedValue = await _cache.GetAsync(BalanceUpCacheKey);
            if (cachedValue != null)
            {
                return await UnpackAsync<BalanceUp>(cachedValue);
            }

            var balanceup = await GetBalanceUpDayFromDb();
            var options = WithAbsoluteExpiration(DateTimeOffset.Now.AddSeconds(CacheTimeout120Seconds));
            await _cache.SetAsync(BalanceUpCacheKey, await PackAsync(balanceup), options);

            return balanceup;
        }

        private static async Task<BalanceUp> GetBalanceUpDayFromDb()
        {
            const string UspName = "Lrf_SelBalanceUpDay";

            using (var db = ObjectDbFactory.CreateInstance(UspName))
            {
                var balanceUp = new BalanceUp();
                using (var reader = await db.ExecuteReaderAsync())
                {
                    if (reader.Read())
                    {
                        balanceUp.BalanceUpDay = Convert.ToDateTime(reader["balanceupday"]);
                        balanceUp.MoveBetsDay = Convert.ToDateTime(reader["movebetsday"]);
                    }

                    return balanceUp;
                }
            }
        }

        private static DistributedCacheEntryOptions WithAbsoluteExpiration(DateTimeOffset time)
           => new DistributedCacheEntryOptions { AbsoluteExpiration = time };

        private static DistributedCacheEntryOptions WithSlidingExpiration(TimeSpan time)
            => new DistributedCacheEntryOptions { SlidingExpiration = time };

        private static Task<T> UnpackAsync<T>(byte[] data)
        {
            var serializer = MessagePackSerializer.Get<T>(_msgPackSerializationContext);
            return serializer.UnpackSingleObjectAsync(data);
        }

        private static Task<byte[]> PackAsync<T>(T value)
        {
            var serializer = MessagePackSerializer.Get<T>(_msgPackSerializationContext);
            return serializer.PackSingleObjectAsync(value);
        }
    }
}
