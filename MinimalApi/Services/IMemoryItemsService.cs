namespace MinimalApi.Services;

public interface IMemoryItemsService
{
    void AddItem(Guid key);
    Guid? GetNextItem();
}