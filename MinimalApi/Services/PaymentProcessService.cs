using System.Linq;
using MinimalApi.Models;

namespace MinimalApi.Services;

public class PaymentProcessService : IPaymentProcessService
{
    private readonly ICacheItemsService cacheItemsService;
    public PaymentProcessService(ICacheItemsService cacheItemsService)
    {
        this.cacheItemsService = cacheItemsService;
    }
    public async Task<DateTime> ProcessPaymentAsync(PaymentRequest paymentRequest)
    {
        var processDate = DateTime.UtcNow;
        // Simulate payment processing logic}
        Console.WriteLine($"Processing payment for correlationId: {paymentRequest.correlationId}, amount: {paymentRequest.amount}");
        //await Task.Delay(500); // Simulate async work
        return processDate;
    }


    public async Task<PaymentSummary> GetPaymentSummaryAsync(DateTime from, DateTime to)
    {
        var defaultSummary = await cacheItemsService.GetProcessedItemsAsync(ProcessedByEnum.Default);
        var fallbackSummary = await cacheItemsService.GetProcessedItemsAsync(ProcessedByEnum.Fallback);

        var defaultSummaryFiltered = defaultSummary.Where(x => x.processedAt >= from && x.processedAt <= to);
        var fallbackSummaryFiltered = fallbackSummary.Where(x => x.processedAt >= from && x.processedAt <= to);

        // Simulate fetching payment summary logic
        Console.WriteLine($"Fetching payment summary from {from} to {to}");
        return new PaymentSummary
        {
            Default = new SummaryInfo(defaultSummaryFiltered.Count(), defaultSummaryFiltered.Sum(s=> s.amount)),
            Fallback = new SummaryInfo(fallbackSummaryFiltered.Count(), fallbackSummaryFiltered.Sum(s => s.amount))
        };
    }
}