using System.Collections.Concurrent;

namespace MinimalApi.Services;

public class MemoryItemsService : IMemoryItemsService
{
    private readonly ConcurrentQueue<string> _items = new();
    public void AddItem(string key)
    {
        _items.Enqueue(key);
    }

    public string? GetNextItem()
    {
        return _items.TryDequeue(out var item) ? item : null;
    }
}