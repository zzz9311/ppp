namespace PetPPP;

public interface ICache
{
    Task<T> GetOrAddAsync<T>(string key, Func<Task<T>> getEntity,
        GeneralCacheOptions options, CancellationToken cancellationToken = default) where T : class;

    Task<T> GetOrAddAsync<T>(string key, Func<T> getEntity,
        GeneralCacheOptions options, CancellationToken cancellationToken = default) where T : class;

    Task ClearCache(string key, CancellationToken cancellationToken = default);
}