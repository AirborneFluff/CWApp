using API.Helpers;

namespace API.Data.Migrations
{
    public class SuppliersRepository : ISuppliersRepository
    {
        private readonly DataContext _context;
        public SuppliersRepository(DataContext context)
        {
            this._context = context;
        }

        public void AddSupplier(Supplier supplier)
        {
            _context.Add(supplier);
        }

        public async Task<bool> Exists(string supplierName)
        {
            var supplier = await GetSupplierByName(supplierName);
            if (supplier == null) return false;
            return true;
        }
        public async Task<Supplier> GetSupplierById(int id)
        {
            return await _context.Suppliers.FindAsync(id);
        }

        public async Task<Supplier> GetSupplierByName(string name)
        {
            return await _context.Suppliers.FirstOrDefaultAsync(s => s.NormalizedName == name.ToUpper());
        }

        public async Task<PagedList<Supplier>> GetSuppliers(PaginationParams partParams)
        {
            var query = _context.Suppliers.OrderBy(s => s.Name);
            return await PagedList<Supplier>.CreateAsync(query, partParams.PageNumber, partParams.PageSize);
        }

        public void RemoveSupplier(Supplier supplier)
        {
            _context.Suppliers.Remove(supplier);
        }

        public async Task RemoveSupplierByName(string supplierName)
        {
            var supplier = await GetSupplierByName(supplierName);
            if (supplier == null) return;
            RemoveSupplier(supplier);
        }

        public async Task RemoveSupplierById(string supplierName)
        {
            var supplier = await GetSupplierByName(supplierName);
            if (supplier == null) return;
            RemoveSupplier(supplier);
        }

        public async Task RemoveSupplierById(int id)
        {
            var supplier = await GetSupplierById(id);
            if (supplier == null) return;
            RemoveSupplier(supplier);
        }

    }
}