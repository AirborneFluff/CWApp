namespace API.Interfaces
{
    public interface ISupplySourceRepository
    {
        void RemoveSupplySource(SupplySource source);
        Task<SupplySource> GetSupplySourceById(int id);

    }
}