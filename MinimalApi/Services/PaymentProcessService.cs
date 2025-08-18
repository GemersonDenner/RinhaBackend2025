using System.Linq;
using MinimalApi.Models;

namespace MinimalApi.Services;

public class PaymentProcessService : IPaymentProcessService
{
    private readonly ICacheItemsService cacheItemsService;
    private readonly IApiRequestsService apiRequestsService;
    public PaymentProcessService(ICacheItemsService cacheItemsService, IApiRequestsService apiRequestsService)
    {
        this.cacheItemsService = cacheItemsService;
        this.apiRequestsService = apiRequestsService;
    }
    public async Task<DateTime> ProcessPaymentAsync(PaymentRequest paymentRequest)
    {
        var processDate = DateTime.UtcNow;
        var successCallApi = await apiRequestsService.CallDefaultApi(new PaymentProcessed
        {
            amount = paymentRequest.amount,
            correlationId = paymentRequest.correlationId,
            processedAt = processDate
        });
        Console.WriteLine($"Default API call success: {successCallApi}");

        // If the default API call fails, call the fallback API
        if (!successCallApi)
        {
            var successCallApiFallback = await apiRequestsService.CallFallbackApi(new PaymentProcessed
            {
                amount = paymentRequest.amount,
                correlationId = paymentRequest.correlationId,
                processedAt = processDate
            });

            Console.WriteLine($"Fallback API call success: {successCallApiFallback}");
        }

        Console.WriteLine($"Processing payment for correlationId: {paymentRequest.correlationId}, amount: {paymentRequest.amount}");
        //await Task.Delay(500); // Simulate async work
        return processDate;
        
    }


    public async Task<PaymentSummary> GetPaymentSummaryAsync(DateTimeOffset from, DateTimeOffset to)
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