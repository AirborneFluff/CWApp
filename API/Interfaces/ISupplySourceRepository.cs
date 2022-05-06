namespace API.Interfaces
{
    public interface ISupplySourceRepository
    {
        void RemoveSupplySource(SupplySource source);
        Task<SupplySource> GetSupplySourceById(int id);
        Task<SupplySource> GetSupplySource(string partCode, string supplierName, string supplierSKU);
        Task ReplaceSourcesSupplier(int oldSupplierId, int newSupplierId);
    }
}