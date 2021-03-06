using API.Helpers;

namespace API.Interfaces
{
    public interface ISuppliersRepository
    {
        void AddSupplier(NewSupplierDto supplier);
        void RemoveSupplier(Supplier supplier);
        Task RemoveSupplierByName(string supplierName);
        Task RemoveSupplierById(int id);
        Task<bool> Exists(string supplierName);
        Task<Supplier> GetSupplierByName(string name);
        Task<List<Supplier>> GetSuppliersByName(string[] supplierNames);
        Task<Supplier> GetSupplierById(int id);
        Task<PagedList<Supplier>> GetSuppliers(PaginationParams partParams, Func<Supplier, bool> predicate);
        Task<List<Supplier>> GetAllSuppliers();
        Task<List<string>> GetAllSupplierNames();
    }
}