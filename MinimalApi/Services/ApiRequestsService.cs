using MinimalApi.Models;
namespace MinimalApi.Services;

public class ApiRequestsService : IApiRequestsService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    public ApiRequestsService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }
    public async Task<bool> CallDefaultApi(PaymentProcessed request)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync(_configuration["DEFAULT_BASE_URL"], request, AppJsonSerializerContext.Default.PaymentProcessed);
            return response.IsSuccessStatusCode;
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Error calling default API: {ex.Message}");
            return false;
        }

    }
    public async Task<bool> CallFallbackApi(PaymentProcessed request)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync(_configuration["FALLBACK_BASE_URL"], request, AppJsonSerializerContext.Default.PaymentProcessed);
            return response.IsSuccessStatusCode;
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Error calling fallback API: {ex.Message}");
            return false;
        }
    }
}