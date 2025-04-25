
using System.Net;
using lockserver;
using RedLockNet;
using RedLockNet.SERedis;
using RedLockNet.SERedis.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

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

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

/*builder.Services.AddHttpsRedirection(options =>
{
    options.HttpsPort = 5001; // <- must match the one in launchSettings.json
});*/

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapGet("/", () => "Hello with Redis Lock!");
app.Run();
