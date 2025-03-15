namespace PizzaOderingAppAPI.Services;

public interface ICacheService
{
    Task<T?> GetAsync<T>(string key);
    Task SetAsync<T>(string key, T value, TimeSpan? expiryTime = null);
    Task RemoveAsync(string key);
}
