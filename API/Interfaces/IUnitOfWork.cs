namespace API.Interfaces
{
    public interface IUnitOfWork
    {
        IPartsRepository PartsRepository { get; }
        ISuppliersRepository SuppliersRepository { get; }
        ISupplySourceRepository SupplySourceRepository { get; }
        ISourcePriceRepository SourcePriceRepository { get; }
        IPartsListsRepository PartsListsRepository { get; }
        Task<bool> Complete();
        bool HasChanges();
    }
}