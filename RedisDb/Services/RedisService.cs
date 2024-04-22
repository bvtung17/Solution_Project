
using ServiceStack.Redis;

namespace RedisDb.Services
{
    public class RedisService : IRedisService
    {
        private readonly IRedisClientsManagerAsync _redisPool;

        public RedisService(IRedisClientsManagerAsync redisPool)
        {
            _redisPool = redisPool;
        }

        public async Task<string> GetAllValue(string hash)
        {
            await using var redisClient = await _redisPool.GetClientAsync();

            var value = await redisClient.GetAllEntriesFromHashAsync(hash);
            if (value is null)
            {
                return string.Empty;
            }
            var listKeys = value.Keys.ToList();
            foreach (var item in listKeys)
            {
                await redisClient.RemoveEntryFromHashAsync(hash, item);
            }

            return string.Empty;
        }

        public async Task<string> GetValue(string key)
        {
            await using var redisClient = await _redisPool.GetClientAsync();

            var value = await redisClient.GetValueAsync(key);

            return value;
        }

        public async Task<string> SetValue(string key, string value)
        {
            await using var redisClient = await _redisPool.GetClientAsync();
            await redisClient.SetValueAsync(key, value);

            return string.Empty;
        }

        public async Task<string> SetValue(string hash, string key, string value)
        {
            await using var redisClient = await _redisPool.GetClientAsync();

            var test = await redisClient.SetEntryInHashAsync(hash, key, value);

            return string.Empty;
        }
    }
}
