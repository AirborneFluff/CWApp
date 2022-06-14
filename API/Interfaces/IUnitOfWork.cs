namespace API.Interfaces
{
    public interface IUnitOfWork
    {
        IPartsRepository PartsRepository { get; }
        ISuppliersRepository SuppliersRepository { get; }
        ISupplySourceRepository SupplySourceRepository { get; }
        ISourcePriceRepository SourcePriceRepository { get; }
        IBOMsRepository BOMsRepository { get; }
        IProductsRepository ProductsRepository { get; }
        IRequisitionsRepository RequisitionsRepository { get; }
        IUsersRepository UsersRepository { get; }
        IStockLevelsRepository StockRepository { get; }
        IOutboundOrdersRepository OutboundOrdersRepository { get; }
        Task<bool> Complete();
        bool HasChanges();
    }
}