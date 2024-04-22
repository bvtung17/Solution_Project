namespace RedisDb.Services
{
    public interface IRedisService
    {
        Task<string> GetValue(string key);
        Task<string> GetAllValue(string hash);
        Task<string> SetValue(string key, string value);
        Task<string> SetValue(string hash, string key, string value);
    }
}
