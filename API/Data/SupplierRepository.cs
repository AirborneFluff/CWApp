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

        public void AddSupplier(NewSupplierDto supplier)
        {
            var newSupplier = new Supplier(supplier.Name)
            {
                Website = supplier.Website
            };
            _context.Suppliers.Add(newSupplier);
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

        public async Task<PagedList<Supplier>> GetSuppliers(PaginationParams partParams, Func<Supplier, bool> predicate)
        {
            var query = _context.Suppliers.OrderBy(s => s.Name).AsQueryable();;
            return await PagedList<Supplier>.CreateAsync(query, predicate, partParams.PageNumber, partParams.PageSize);
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

        public async Task<List<string>> GetAllSupplierNames()
        {
            return await _context.Suppliers.Select(s => s.Name).ToListAsync();
        }

        public async Task<List<Supplier>> GetAllSuppliers()
        {
            return await _context.Suppliers.ToListAsync();
        }
    }
}