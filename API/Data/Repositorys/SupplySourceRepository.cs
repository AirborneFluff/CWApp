namespace API.Data.Repositorys
{
    public class SupplySourceRepository : ISupplySourceRepository
    {
        private readonly DataContext _context;
        public SupplySourceRepository(DataContext context)
        {
            this._context = context;
        }

        public void AddSupplySource(SupplySource source)
        {
            _context.SupplySources.Add(source);
        }

        public async Task<SupplySource> GetSupplySource(string partCode, string supplierName, string supplierSKU)
        {
            var part = await _context.Parts.FirstOrDefaultAsync(x => x.PartCode == partCode);
            var supplier = await _context.Suppliers.FirstOrDefaultAsync(x => x.NormalizedName == supplierName.ToUpper());
            return await _context.SupplySources
                .Where(x => x.PartId == part.Id)
                .Where(x => x.Supplier == supplier)
                .FirstOrDefaultAsync(x => x.SupplierSKU == supplierSKU);
        }

        public async Task<SupplySource> GetSupplySourceById(int id)
        {
            return await _context.SupplySources.FindAsync(id);
        }

        public void RemoveSupplySource(SupplySource source)
        {
            _context.SupplySources.Remove(source);
        }
        
        public async Task ReplaceSourcesSupplier(int oldSupplierId, int newSupplierId)
        {
            var sources = await _context.SupplySources.Where(s => s.SupplierId == oldSupplierId).ToListAsync();
            foreach (var source in sources) {
                source.SupplierId = newSupplierId;
            }
        }

        public void Update(SupplySource source)
        {
            _context.Entry(source).State = EntityState.Modified;
        }
    }
}