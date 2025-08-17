using Enyim.Caching;
using MinimalApi.Models;

namespace MinimalApi.Services;

public class CacheItemsService : ICacheItemsService
{
    private const int cacheTimeSeconds = 3600;
    private const string processedItemsDefaultKey = "Default";
    private const string processedItemsFallbackKey = "Fallback";
    private const string summaryItemsDefaultKey = "SummaryDefault";
    private const string summaryItemsFallbackKey = "SummaryFallback";

    private readonly IMemcachedClient memcachedClient;

    public CacheItemsService(IMemcachedClient memcachedClient)
    {
        this.memcachedClient = memcachedClient;
    }
    public async Task AddItemAsync(PaymentRequest request)
    {
        await this.memcachedClient.SetAsync(request.correlationId.ToString(), request, cacheTimeSeconds);
    }

    public async Task<PaymentRequest> GetItemAsync(Guid key)
    {
        var value = await this.memcachedClient.GetValueAsync<PaymentRequest>(key.ToString());
        if(value != null)
        {
            this.memcachedClient.RemoveAsync(key.ToString());
        }
        return value;
    }
    
    public async Task AddUpdateProcessedItemAsync(PaymentProcessed processedItem, ProcessedByEnum processedBy)
    {
        var processedItemsKey = processedBy == ProcessedByEnum.Default ? processedItemsDefaultKey : processedItemsFallbackKey;
        var array = await this.memcachedClient.GetValueAsync<List<PaymentProcessed>>(processedItemsKey) ?? new List<PaymentProcessed>();
        await this.memcachedClient.ReplaceAsync(processedItemsKey, array.Append(processedItem), cacheTimeSeconds);
    }
    public async Task AddUpdateSummaryAsync(decimal totalAmount, ProcessedByEnum processedBy)
    {
        var summaryItemsKey = processedBy == ProcessedByEnum.Default ? summaryItemsDefaultKey : summaryItemsFallbackKey;
        var summary = await this.memcachedClient.GetValueAsync<SummaryInfo>(summaryItemsKey) ?? new SummaryInfo();
        summary.TotalAmount += totalAmount;
        summary.TotalRequests++;
        await this.memcachedClient.ReplaceAsync(summaryItemsKey, summary, cacheTimeSeconds);
    }
    
    public async Task<List<PaymentProcessed>> GetProcessedItemsAsync(ProcessedByEnum processedBy)
    {
        var processedItemsKey = processedBy == ProcessedByEnum.Default ? processedItemsDefaultKey : processedItemsFallbackKey;
        return await this.memcachedClient.GetValueAsync<List<PaymentProcessed>>(processedItemsKey) ?? new List<PaymentProcessed>();
    }
    
    public async Task<SummaryInfo> GetSummaryAsync(ProcessedByEnum processedBy)
    {
        var summaryItemsKey = processedBy == ProcessedByEnum.Default ? summaryItemsDefaultKey : summaryItemsFallbackKey;
        return await this.memcachedClient.GetValueAsync<SummaryInfo>(summaryItemsKey) ?? new SummaryInfo();
    }
}