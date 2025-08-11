using MinimalApi.Models;
namespace MinimalApi.Services;

public class ApiRequestsService : IApiRequestsService
{
    private readonly HttpClient _httpClient;
    public ApiRequestsService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    public async Task<bool> CallDefaultApi(PaymentProcessed request)
    {
        var response = await _httpClient.PostAsJsonAsync("https://default-api.example.com/process", request, AppJsonSerializerContext.Default.PaymentProcessed);
        return response.IsSuccessStatusCode;
    }
    public async Task<bool> CallFallbackApi(PaymentProcessed request)
    {
        var response = await _httpClient.PostAsJsonAsync("https://fallback-api.example.com/process", request, AppJsonSerializerContext.Default.PaymentProcessed);
        return response.IsSuccessStatusCode;
    }
}