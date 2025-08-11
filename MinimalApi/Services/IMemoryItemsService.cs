namespace MinimalApi.Services;

public interface IMemoryItemsService
{
    void AddItem(string key);
    string? GetNextItem();
}