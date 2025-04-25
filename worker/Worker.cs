using System.Diagnostics;
using RedisLockServer;

namespace worker;
public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;

    private readonly ILockService _lockService;

    public Worker(ILogger<Worker> logger, ILockService lockService)
    {
        _logger = logger;
        _lockService = lockService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        int task_id = 0;
        
        var param_job = Environment.GetEnvironmentVariable("JOB_TIME");        
        var param_expire = Environment.GetEnvironmentVariable("EXPIRY_TIME");
        var param_wait = Environment.GetEnvironmentVariable("WAIT_TIME");
        var param_retry = Environment.GetEnvironmentVariable("RETRY_TIME");
        
        int val_job = param_job != null?int.Parse(param_job):6000;
        int val_expiry = param_expire != null?int.Parse(param_expire):10;
        int val_wait = param_wait != null?int.Parse(param_wait):1;
        int val_retry = param_retry != null?int.Parse(param_retry):500;

        TimeSpan expiryTime = TimeSpan.FromSeconds(val_expiry);
        TimeSpan waitTime = TimeSpan.FromSeconds(val_wait);
        TimeSpan retryTime = TimeSpan.FromMicroseconds(val_retry);

        while (!stoppingToken.IsCancellationRequested)
        {            
            task_id++;
            if (_logger.IsEnabled(LogLevel.Information))
            {
                var success = await _lockService.ExecuteWithLockAsync("chave-nfe", expiryTime, waitTime, retryTime, async () =>                                {
                    await Task.Delay( val_job); // simulate work
                    _logger.LogDebug($"Task {task_id} done inside lock {val_job}");
                    });
                if (!success)
                {
                    _logger.LogWarning($"Could not acquire lock in wait time {val_wait} expiry in {val_expiry}. Try again later.");
                }                
            }
            //await Task.Delay(500, stoppingToken);
        }
    }

}
