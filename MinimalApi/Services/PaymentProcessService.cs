namespace MinimalApi.Services;

public class PaymentProcessService: BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            Console.WriteLine("Processing payments...");
            // Simulate some background processing
            await Task.Delay(1000, stoppingToken);
        }
    }
}