namespace API.Data
{
    public class SupplySourceRepository : ISupplySourceRepository
    {
        private readonly DataContext _context;
        public SupplySourceRepository(DataContext context)
        {
            this._context = context;
        }

        public async Task<SupplySource> GetSupplySourceById(int id)
        {
            return await _context.SupplySources.FindAsync(id);
        }

        public void RemoveSupplySource(SupplySource source)
        {
            _context.SupplySources.Remove(source);
        }
    }
}