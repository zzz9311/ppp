using Core.DependencyInjectionExtensions;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace PetPPP;

[SelfRegistered(typeof(ICache), ServiceLifeTime.Singleton)]
public class RedisCacheDecorated : ICache
{
    private readonly IDistributedCache _decorated;
    
    public RedisCacheDecorated(IDistributedCache decorated)
    {
        _decorated = decorated;
    }

    private DistributedCacheEntryOptions GetCacheEntryOptions(GeneralCacheOptions generalCacheOptions)
    {
        return new()
        {
            AbsoluteExpiration =
                generalCacheOptions?.AbsoluteExpiration ?? DateTimeOffset.Now + TimeSpan.FromMinutes(5),
            SlidingExpiration = generalCacheOptions?.SlidingExpiration ?? TimeSpan.FromMinutes(5),
            AbsoluteExpirationRelativeToNow =
                generalCacheOptions?.AbsoluteExpirationRelativeToNow ?? TimeSpan.FromMinutes(5)
        };
    }

    public async Task<T> GetOrAddAsync<T>(string key, Func<Task<T>> getEntity,
        GeneralCacheOptions options, CancellationToken cancellationToken = default) where T : class
    {
        var cachedValue = await _decorated.GetStringAsync(key, cancellationToken);
        if (string.IsNullOrEmpty(cachedValue))
        {
            var entity = await getEntity().ConfigureAwait(false);
            
            if (entity == null)
            {
                return null;
            }
    
            var serializedObject = JsonConvert.SerializeObject(entity);
    
            var cacheOptions = GetCacheEntryOptions(options);
    
            await _decorated.SetStringAsync(key, serializedObject, cacheOptions, cancellationToken);
            return entity;
        }
    
        return JsonConvert.DeserializeObject<T>(cachedValue);
    }
    
    public async Task<T> GetOrAddAsync<T>(string key, Func<T> getEntity,
        GeneralCacheOptions options, CancellationToken cancellationToken = default) where T : class
    {
        var cachedValue = await _decorated.GetStringAsync(key, cancellationToken);
        if (string.IsNullOrEmpty(cachedValue))
        {
            var entity = getEntity();
            
            if (entity == null)
            {
                return null;
            }

            var serializedObject = JsonConvert.SerializeObject(entity);

            var cacheOptions = GetCacheEntryOptions(options);

            await _decorated.SetStringAsync(key, serializedObject, cacheOptions, cancellationToken);
            return entity;
        }

        return JsonConvert.DeserializeObject<T>(cachedValue);
    }

    public async Task ClearCache(string key, CancellationToken cancellationToken = default)
    {
        await _decorated.RemoveAsync(key, cancellationToken);
    }
}