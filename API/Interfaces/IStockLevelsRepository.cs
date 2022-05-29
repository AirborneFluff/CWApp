namespace API.Interfaces
{
    public interface IStockLevelsRepository
    {
        void AddStockEntry(StockLevelEntry entry);
        void RemoveStockEntry(StockLevelEntry entry);
    }
}