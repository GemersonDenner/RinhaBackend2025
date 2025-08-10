using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using MinimalApi.Models;

var builder = WebApplication.CreateSlimBuilder(args);

var app = builder.Build();

app.MapPost("/payments", async (HttpContext context, [FromBody] PaymentRequest paymentRequest) =>
{
    await Task.Delay(10);
    return Results.Ok();
});

app.MapGet("/payments-summary", async ([FromQuery] DateTime from, [FromQuery] DateTime to) =>
{
    await Task.Delay(10);
    return Results.Ok();
});

app.Run();
