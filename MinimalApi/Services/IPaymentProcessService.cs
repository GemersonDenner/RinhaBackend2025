using MinimalApi.Models;
namespace MinimalApi.Services;

public interface IPaymentProcessService
{
    Task<DateTime> ProcessPaymentAsync(PaymentRequest paymentRequest);
    Task<PaymentSummary> GetPaymentSummaryAsync(DateTime from, DateTime to);
}