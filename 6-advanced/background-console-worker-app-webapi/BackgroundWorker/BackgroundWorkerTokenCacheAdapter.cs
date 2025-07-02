using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Web.TokenCacheProviders;
using Microsoft.Identity.Web.TokenCacheProviders.Distributed;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BackgroundWorker
{
    /// <summary>
    /// Custom token cache adapter where you can explicitly tell what is the cache key to be used
    /// </summary>
    public class BackgroundWorkerTokenCacheAdapter : MsalDistributedTokenCacheAdapter
    {
        /// <summary>
        /// .NET Core Memory cache.
        /// </summary>
        private readonly IDistributedCache _distributedCache;
        private readonly ILogger<MsalDistributedTokenCacheAdapter> _logger;
        /// <summary>
        /// MSAL memory token cache options.
        /// </summary>
        private readonly MsalDistributedTokenCacheAdapterOptions _cacheOptions;
        private readonly string _cacheKey;
        public BackgroundWorkerTokenCacheAdapter(string cacheKey, 
            IDistributedCache distributedCache, 
            IOptions<MsalDistributedTokenCacheAdapterOptions> cacheOptions,
            ILogger<MsalDistributedTokenCacheAdapter> logger) 
            : base(distributedCache, cacheOptions, logger)
        {
            _cacheKey = cacheKey;
            _distributedCache = distributedCache;
            _cacheOptions = cacheOptions?.Value;
            _logger = logger;
        }
        protected override async Task RemoveKeyAsync(string cacheKey)
        {
            _logger.LogInformation($"RemoveKeyAsync::cacheKey-'{cacheKey}'");
            await _distributedCache.RemoveAsync(_cacheKey).ConfigureAwait(false);
        }
        protected override async Task RemoveKeyAsync(string cacheKey, CacheSerializerHints cacheSerializerHints)
        {
            _logger.LogInformation($"RemoveKeyAsync::cacheKey-'{cacheKey}'");
            await _distributedCache.RemoveAsync(_cacheKey).ConfigureAwait(false);
        }
        protected override async Task<byte[]> ReadCacheBytesAsync(string cacheKey)
        {
            _logger.LogInformation($"ReadCacheBytesAsync::cacheKey-'{cacheKey}'");
            return await _distributedCache.GetAsync(_cacheKey).ConfigureAwait(false);
        }
        protected override async Task<byte[]> ReadCacheBytesAsync(string cacheKey, CacheSerializerHints cacheSerializerHints)
        {
            _logger.LogDebug($"ReadCacheBytesAsync::cacheKey-'{cacheKey}'");
            return await _distributedCache.GetAsync(_cacheKey).ConfigureAwait(false);
        }
        protected override async Task WriteCacheBytesAsync(string cacheKey, byte[] bytes)
        {
            _logger.LogInformation($"WriteCacheBytesAsync::cacheKey-'{cacheKey}'");
            await _distributedCache.SetAsync(_cacheKey, bytes, _cacheOptions).ConfigureAwait(false);
        }
        protected override async Task WriteCacheBytesAsync(string cacheKey, byte[] bytes, CacheSerializerHints cacheSerializerHints)
        {
            _logger.LogInformation($"WriteCacheBytesAsync::cacheKey-'{cacheKey}'");
            await _distributedCache.SetAsync(_cacheKey, bytes, _cacheOptions).ConfigureAwait(false);
        }
    }
}
