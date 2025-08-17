using System.Collections.Concurrent;

namespace MinimalApi.Services;

public class MemoryItemsService : IMemoryItemsService
{
    private readonly ConcurrentQueue<Guid> _items = new();
    public void AddItem(Guid key)
    {
        _items.Enqueue(key);
    }

    public Guid? GetNextItem()
    {
        return _items.TryDequeue(out var item) ? item : null;
    }
}