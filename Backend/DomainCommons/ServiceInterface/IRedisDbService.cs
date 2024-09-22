namespace DomainCommons.ServiceInterface;

public interface IRedisDbService
{
    Task<TResult?> GetAsync<TResult>(string key);

    Task<bool> SetAsync<TValue>(string key, TValue value, TimeSpan? expiry = null);

    Task<bool> RemoveAsync(string key);
}
