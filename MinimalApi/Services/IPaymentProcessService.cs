using MinimalApi.Models;
namespace MinimalApi.Services;

public interface IPaymentProcessService
{
    Task<(DateTime, ProcessedByEnum)> ProcessPaymentAsync(PaymentRequest paymentRequest);
    Task<PaymentSummary> GetPaymentSummaryAsync(DateTimeOffset from, DateTimeOffset to);
}