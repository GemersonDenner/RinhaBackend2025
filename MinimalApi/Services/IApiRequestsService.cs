using MinimalApi.Models;
namespace MinimalApi.Services;

public interface IApiRequestsService
{
    Task<bool> CallDefaultApi(PaymentProcessed request);
    Task<bool> CallFallbackApi(PaymentProcessed request);
}