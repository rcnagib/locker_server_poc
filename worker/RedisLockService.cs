using RedLockNet;
using RedLockNet.SERedis;
using RedLockNet.SERedis.Configuration;
using System.Net;

namespace RedisLockServer;
public class RedisLockService : ILockService
{
    private readonly IDistributedLockFactory _redlockFactory;

    public RedisLockService(IDistributedLockFactory redlockFactory)
    {
        _redlockFactory = redlockFactory;
    }

    public async Task<bool> ExecuteWithLockAsync(string resource, TimeSpan expiryTime, TimeSpan waitTime, TimeSpan retryTime, Func<Task> action)
    {
        using (var redLock = await _redlockFactory.CreateLockAsync(resource, expiryTime, waitTime, retryTime))
        {
            if (redLock.IsAcquired)
            {
                await action();
                redLock.Dispose();
                return true;
            }

            return false;
        }
    }
}
