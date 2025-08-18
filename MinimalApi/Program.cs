using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using MinimalApi.Models;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.AddHttpClient("clientApi", client =>
{
    client.Timeout = TimeSpan.FromSeconds(10);
})
.ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler
{
    PooledConnectionLifetime = TimeSpan.FromMinutes(10),
});

builder.Services.AddHostedService<MinimalApi.Services.PaymentWorkerService>();
builder.Services.AddSingleton<MinimalApi.Services.IMemoryItemsService, MinimalApi.Services.MemoryItemsService>();
builder.Services.AddSingleton<MinimalApi.Services.IPaymentProcessService, MinimalApi.Services.PaymentProcessService>();
builder.Services.AddSingleton<MinimalApi.Services.ICacheItemsService, MinimalApi.Services.CacheItemsService>();
builder.Services.AddSingleton<MinimalApi.Services.IApiRequestsService, MinimalApi.Services.ApiRequestsService>();
builder.Services.AddEnyimMemcached(options =>
{
    var memCPort = int.Parse(Environment.GetEnvironmentVariable("MEMCACHED_PORT"));
    var memCHost = Environment.GetEnvironmentVariable("MEMCACHED_HOST");
    options.AddServer(memCHost, memCPort);
});
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});

builder.Services.AddHealthChecks();

var app = builder.Build();

app.MapHealthChecks("/health");
app.MapPost("/payments", async (HttpContext context, 
                                [FromBody] PaymentRequest paymentRequest, 
                                [FromServices] MinimalApi.Services.IMemoryItemsService  memoryItemsService,
                                [FromServices] MinimalApi.Services.ICacheItemsService cacheItemsService
                                ) =>
{
    await cacheItemsService.AddItemAsync(paymentRequest);
    memoryItemsService.AddItem(paymentRequest.correlationId);
    return Results.Ok();
});

app.MapGet("/payments-summary", async (
                                [FromQuery] DateTimeOffset from,
                                [FromQuery] DateTimeOffset to,
                                [FromServices] MinimalApi.Services.IPaymentProcessService paymentProcessService
                                ) =>
{
    var summary = await paymentProcessService.GetPaymentSummaryAsync(from, to);
    return Results.Ok(summary);
});
app.UseEnyimMemcached();
app.Run();

[JsonSerializable(typeof(PaymentProcessed))]
[JsonSerializable(typeof(PaymentSummary))]
[JsonSerializable(typeof(PaymentRequest))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{

}
