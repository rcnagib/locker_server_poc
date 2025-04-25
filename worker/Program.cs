using System.Net;
using RedisLockServer;
using RedLockNet;
using RedLockNet.SERedis;
using RedLockNet.SERedis.Configuration;
using worker;

var builder = Host.CreateApplicationBuilder(args);

// Configure RedLock with Redis endpoints
builder.Services.AddSingleton<IDistributedLockFactory>(_ =>
{
    var redisEndpoints = new List<RedLockEndPoint>
    {
        new RedLockEndPoint { EndPoint = new DnsEndPoint("localhost", 6379) }
        // Add more endpoints for distributed Redis
    };

    return RedLockFactory.Create(redisEndpoints);
});

// Register your lock service
builder.Services.AddSingleton<ILockService, RedisLockService>();

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
