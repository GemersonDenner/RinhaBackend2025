using MinimalApi.Models;
namespace MinimalApi.Services;

public class PaymentWorkerService: BackgroundService
{
    private readonly IMemoryItemsService memoryItemsService;
    private readonly ICacheItemsService cacheItemsService;
    private readonly IPaymentProcessService paymentProcessService;
    public PaymentWorkerService(
        IMemoryItemsService memoryItemsService,
        ICacheItemsService cacheItemsService,
        IPaymentProcessService paymentProcessService
        )
    {
        this.memoryItemsService = memoryItemsService;
        this.cacheItemsService = cacheItemsService;
        this.paymentProcessService = paymentProcessService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        
        Console.WriteLine("Processing payments...");
        while (!stoppingToken.IsCancellationRequested)
        {
            
            // Get the next item from memory items service
            var nextItem = memoryItemsService.GetNextItem();
            while (nextItem != null)
            {
                var parallelOptions = new ParallelOptions
                {
                    CancellationToken = stoppingToken,
                    MaxDegreeOfParallelism = 10,
                };
                await Parallel.ForEachAsync(new[] { nextItem }, parallelOptions, async (item, token) =>
                {
                    var cachedItem = await cacheItemsService.GetItemAsync(nextItem.Value);
                    if (cachedItem != null)
                    {
                        Console.WriteLine($"Processing item: {nextItem}");
                        // Here you would call the payment processing service
                        var dateProcess = await paymentProcessService.ProcessPaymentAsync(new PaymentRequest(nextItem.Value, 100.00m)); // Example amount
                        var processedItem = new PaymentProcessed() { amount = cachedItem.amount, correlationId = cachedItem.correlationId, processedAt = dateProcess }; // Example amount
                        await cacheItemsService.AddUpdateProcessedItemAsync(processedItem, ProcessedByEnum.Default);
                    }
                    nextItem = memoryItemsService.GetNextItem();
                });

                
            }
            await Task.Delay(2, stoppingToken);
        }
    }
}