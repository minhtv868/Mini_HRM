using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;
using Web.Application.Interfaces.Caching;

namespace Web.Application.Common.Behaviours
{
    public class CachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
    {
        private readonly IDistributedCache _cache;
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;

        public CachingBehavior(IDistributedCache cache, ILogger<TResponse> logger, IConfiguration configuration)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = configuration;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (request is ICacheable cacheableQuery)
            {
                TResponse response;

                if (cacheableQuery.BypassCache) return await next();

                async Task<TResponse> GetResponseAndAddToCache()
                {
                    TimeSpan? slidingExpiration;

                    if (cacheableQuery.SlidingExpiration.HasValue)
                    {
                        slidingExpiration = cacheableQuery.SlidingExpiration.Value;
                    }
                    else
                    {
                        double slidingExpirationConfig = 0;
                        double.TryParse(_configuration["AppSettings:SlidingExpiration"], out slidingExpirationConfig);
                        slidingExpiration = TimeSpan.FromMinutes(slidingExpirationConfig);
                    }

                    response = await next();

                    var options = new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = slidingExpiration };
                    var serializedData = Encoding.Default.GetBytes(JsonConvert.SerializeObject(response));
                    await _cache.SetAsync(cacheableQuery.CacheKey, serializedData, options, cancellationToken);
                    return response;
                }

                var cachedResponse = await _cache.GetAsync(cacheableQuery.CacheKey, cancellationToken);

                if (cachedResponse != null)
                {
                    response = JsonConvert.DeserializeObject<TResponse>(Encoding.Default.GetString(cachedResponse));
                    _logger.LogInformation($"Fetched from Cache -> '{cacheableQuery.CacheKey}'.");
                }
                else
                {
                    response = await GetResponseAndAddToCache();
                    _logger.LogInformation($"Added to Cache -> '{cacheableQuery.CacheKey}'.");
                }

                return response;
            }
            else
            {
                return await next();
            }
        }
    }
}
