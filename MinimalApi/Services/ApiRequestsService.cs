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
            var response = await _httpClient.PostAsJsonAsync(_configuration["DEFAULT_BASE_URL"]+"/payments/", request, AppJsonSerializerContext.Default.PaymentProcessed);
            //Console.WriteLine("Response from default API: " + response);

            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error calling default API: {ex.Message}");
            return false;
        }

    }
    public async Task<bool> CallFallbackApi(PaymentProcessed request)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync(_configuration["FALLBACK_BASE_URL"]+"/payments/", request, AppJsonSerializerContext.Default.PaymentProcessed);
            //Console.WriteLine("Response from default API: " + response);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error calling fallback API: {ex.Message}");
            return false;
        }
    }
}