namespace RedisLockServer;
public interface ILockService{
    Task<bool> ExecuteWithLockAsync(string resource, TimeSpan expiryTime, TimeSpan wait, TimeSpan retry, Func<Task> action);
}