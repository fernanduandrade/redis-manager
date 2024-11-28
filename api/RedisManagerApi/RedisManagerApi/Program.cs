using RedisManagerApi.Configurations;
using RedisManagerApi.Handlers;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.InjectApiBehavior();
builder.Services.InjectInfrastructure();
builder.Services.InjectServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var redis = app.MapGroup("api/redis");
redis.MapPost("connections", CacheHandler.CreateConnection);
redis.MapGet("keyspaces", CacheHandler.GetConnectionKeySpaces);
redis.MapGet("keyspaces/keys", CacheHandler.GetKeys);
redis.MapGet("keyspaces/keys/{hash}", CacheHandler.GetCacheValue);

app.UseStatusCodePages(async statusCodeContext
    => await Results.Problem(statusCode: statusCodeContext.HttpContext.Response.StatusCode)
        .ExecuteAsync(statusCodeContext.HttpContext));

app.UseCors(cp => cp
    .AllowAnyOrigin()
    .SetPreflightMaxAge(TimeSpan.FromHours(24))
    .AllowAnyMethod()
    .AllowAnyHeader());

app.Run();