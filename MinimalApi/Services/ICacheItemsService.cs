using MinimalApi.Models;

namespace MinimalApi.Services;

public interface ICacheItemsService
{
    Task AddItemAsync(PaymentRequest request);
    Task<PaymentRequest> GetItemAsync(Guid key);
    Task AddUpdateProcessedItemAsync(PaymentProcessed processedItem, ProcessedByEnum processedBy);
    Task AddUpdateSummaryAsync(decimal totalAmount, ProcessedByEnum processedBy);
    Task<List<PaymentProcessed>> GetProcessedItemsAsync(ProcessedByEnum processedBy);
    Task<SummaryInfo> GetSummaryAsync(ProcessedByEnum processedBy);
}