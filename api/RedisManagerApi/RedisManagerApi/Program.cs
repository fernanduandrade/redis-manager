using System.Text.Json;
using System.Text.Json.Serialization;
using RedisManagerApi.Configurations;
using RedisManagerApi.Handlers;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.InjectApiBehavior();
builder.Services.InjectInfrastructure();
builder.Services.InjectServices();
builder.Services.ConfigureHttpJsonOptions(x =>
{
    x.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.Configure<Microsoft.AspNetCore.Mvc.JsonOptions>(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var redis = app.MapGroup("api/redis");
redis.MapPost("connections", CacheHandler.CreateConnection);
redis.MapGet("keyspaces", CacheHandler.GetKeys);
redis.MapGet("keyspaces/{hash}", CacheHandler.GetCacheValue);
redis.MapPost("keyspaces/", CacheHandler.CreateKeyValue);
redis.MapPut("keyspaces/{hash}", CacheHandler.UpdateKeyValue);
redis.MapGet("connections/open", CacheHandler.OpenConnection);

app.UseStatusCodePages(async statusCodeContext
    => await Results.Problem(statusCode: statusCodeContext.HttpContext.Response.StatusCode)
        .ExecuteAsync(statusCodeContext.HttpContext));

app.UseCors(cp => cp
    .AllowAnyOrigin()
    .SetPreflightMaxAge(TimeSpan.FromHours(24))
    .AllowAnyMethod()
    .AllowAnyHeader());

app.Run();