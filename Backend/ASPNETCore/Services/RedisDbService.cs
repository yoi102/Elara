using DomainCommons.ServiceInterface;
using StackExchange.Redis;
using System.Text.Json;

namespace ASPNETCore.Services;

internal class RedisDbService : IRedisDbService
{
    private readonly IConnectionMultiplexer redis;

    public RedisDbService(IConnectionMultiplexer redis)
    {
        this.redis = redis;
    }

    public async Task<TResult?> GetAsync<TResult>(string key)
    {
        var db = redis.GetDatabase();
        var redisValue = await db.StringGetAsync(key);
        if (!redisValue.HasValue)
        {
            return default;
        }

        try
        {
            return JsonSerializer.Deserialize<TResult>(redisValue.ToString());
        }
        catch (JsonException)
        {
            return default;
        }
    }

    public async Task<bool> SetAsync<TValue>(string key, TValue value, TimeSpan? expiry = null)
    {
        var db = redis.GetDatabase();
        try
        {
            string json = JsonSerializer.Serialize(value);
            return await db.StringSetAsync(key, json, expiry);
        }
        catch (JsonException)
        {
            return false;
        }
    }

    public async Task<bool> RemoveAsync(string key)
    {
        var db = redis.GetDatabase();
        return await db.KeyDeleteAsync(key);
    }
}
