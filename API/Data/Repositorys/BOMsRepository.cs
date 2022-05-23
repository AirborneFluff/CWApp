namespace API.Data.Repositorys
{
    public class BOMsRepository : IBOMsRepository
    {
        private readonly DataContext _context;
        public BOMsRepository(DataContext context)
        {
            this._context = context;
        }

        public void AddNewBOM(BOM newList)
        {
            _context.BOMs.Add(newList);
        }

        public Task<List<BOM>> GetAllBOMs()
        {
            return _context.BOMs.ToListAsync();
        }

        public Task<List<BOM>> GetBOMs(Product product)
        {
            return _context.BOMs.Where(b => b.ProductId == product.Id).ToListAsync();
        }

        public async Task<BOM> GetBOM(int bomId)
        {
            return await _context.BOMs
                .Include(b => b.Parts)
                .ThenInclude(p => p.Part)
                .FirstOrDefaultAsync(b => b.Id == bomId);
        }

        public async Task<BOM> GetBOMFromTitle(string title)
        {
            return await _context.BOMs
                .Include(l => l.Parts)
                .FirstOrDefaultAsync(x => x.Title.ToLower() == title.ToLower());
        }

        public void RemoveBOM(BOM BOM)
        {
            _context.BOMs.Remove(BOM);
        }
    }
}