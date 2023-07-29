using Core.DependencyInjectionExtensions;
using Core.Helpers;
using Microsoft.Extensions.Caching.Memory;

namespace PetPPP;

[SelfRegistered(typeof(ICache), ServiceLifeTime.Singleton)]
public class MemoryCache : ICache
{
    private readonly IMemoryCache _cache;

    public MemoryCache(IMemoryCache cache)
    {
        _cache = cache;
    }

    private MemoryCacheEntryOptions GetCacheEntryOptions(GeneralCacheOptions generalCacheOptions)
    {
        return new MemoryCacheEntryOptions
        {
            AbsoluteExpiration =
                DateTimeOffset.UtcNow + TimeSpanHelper.FromMinutesNotNull(generalCacheOptions?.AbsoluteExpiration, 5),

            SlidingExpiration = TimeSpanHelper.FromMinutesNotNull(generalCacheOptions?.SlidingExpiration, 5),

            AbsoluteExpirationRelativeToNow =
                TimeSpanHelper.FromMinutesNotNull(generalCacheOptions?.AbsoluteExpirationRelativeToNow, 5)
        };
    }

    public async Task<T> GetOrAddAsync<T>(string key, Func<Task<T>> getEntity, GeneralCacheOptions options = null,
        CancellationToken cancellationToken = default) where T : class
    {
        if (_cache.TryGetValue(key, out T valueFromCache))
        {
            return valueFromCache;
        }

        var cachingObject = await getEntity().ConfigureAwait(false);
        if (cachingObject == null)
        {
            return null;
        }
        
        _cache.Set(key, cachingObject, GetCacheEntryOptions(options));
        return cachingObject;
    }

    public async Task<T> GetOrAddAsync<T>(string key, Func<T> getEntity, GeneralCacheOptions options,
        CancellationToken cancellationToken = default) where T : class
    {
        if (_cache.TryGetValue(key, out T valueFromCache))
        {
            return valueFromCache;
        }

        var cachingObject = getEntity();
        if (cachingObject == null)
        {
            return null;
        }
        
        _cache.Set(key, cachingObject, GetCacheEntryOptions(options));
        return cachingObject;
    }

    public Task ClearCache(string key, CancellationToken cancellationToken = default)
    {
        _cache.Remove(key);
        return Task.CompletedTask;
    }
}