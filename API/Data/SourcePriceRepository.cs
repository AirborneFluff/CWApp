namespace API.Data
{
    public class SourcePriceRepository : ISourcePriceRepository
    {
        private readonly DataContext _context;
        public SourcePriceRepository(DataContext context)
        {
            this._context = context;
        }

        public async Task<SourcePrice> GetSourcePriceById(int id)
        {
            return await _context.SourcePrices.FindAsync(id);
        }

        public void RemoveSourcePrice(SourcePrice price)
        {
            _context.SourcePrices.Remove(price);
        }
    }
}