namespace API.Data.Repositorys
{
    public class StockRepository : IStockLevelsRepository
    {
        private readonly DataContext _context;
        public StockRepository(DataContext context)
        {
            this._context = context;
        }

        public void AddStockEntry(StockLevelEntry entry)
        {
            _context.StockLevelEntries.Add(entry);
        }

        public void RemoveStockEntry(StockLevelEntry entry)
        {
            _context.StockLevelEntries.Remove(entry);
        }
    }
}