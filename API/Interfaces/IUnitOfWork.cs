namespace API.Interfaces
{
    public interface IUnitOfWork
    {
        IPartsRepository PartsRepository { get; }
        ISuppliersRepository SuppliersRepository { get; }
        ISupplySourceRepository SupplySourceRepository { get; }
        ISourcePriceRepository SourcePriceRepository { get; }
        Task<bool> Complete();
        bool HasChanges();
    }
}