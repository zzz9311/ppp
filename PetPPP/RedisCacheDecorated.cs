using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace PetPPP;

public class RedisCacheDecorated
{
    private readonly IDistributedCache _decorated;
    
    public RedisCacheDecorated(IDistributedCache decorated)
    {
        _decorated = decorated;
    }

    private DistributedCacheEntryOptions GetDefaultCacheEntryOptions()
    {
        return new()
        {
            AbsoluteExpiration = DateTimeOffset.Now + TimeSpan.FromMinutes(5) //TODO add this to config
        };
    }

    public async Task<TEntity> GetOrAddAsync<TEntity>(string key, Func<Task<TEntity>> getEntity,
        DistributedCacheEntryOptions options, CancellationToken cancellationToken = default) where TEntity : class
    {
        var cachedValue = await _decorated.GetStringAsync(key, cancellationToken);
        if (string.IsNullOrEmpty(cachedValue))
        {
            var entity = await getEntity();
            
            if (entity == null)
            {
                return null;
            }

            var serializedObject = JsonConvert.SerializeObject(entity);

            options ??= GetDefaultCacheEntryOptions();

            await _decorated.SetStringAsync(key, serializedObject, options, cancellationToken);
            return entity;
        }

        return JsonConvert.DeserializeObject<TEntity>(cachedValue);
    }
    
    public async Task<TEntity> GetOrAddAsync<TEntity>(string key, Func<TEntity> getEntity,
        DistributedCacheEntryOptions options, CancellationToken cancellationToken = default) where TEntity : class
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

            options ??= GetDefaultCacheEntryOptions();

            await _decorated.SetStringAsync(key, serializedObject, options, cancellationToken);
            return entity;
        }

        return JsonConvert.DeserializeObject<TEntity>(cachedValue);
    }
}