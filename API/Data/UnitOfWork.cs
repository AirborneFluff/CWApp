using API.Data.Migrations;
using API.Interfaces;

namespace API.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public UnitOfWork(DataContext context, IMapper mapper)
        {
            this._mapper = mapper;
            this._context = context;
        }

        public IPartsRepository PartsRepository => new PartsRepository(_context, _mapper);
        public ISuppliersRepository SuppliersRepository => new SuppliersRepository(_context);
        public ISupplySourceRepository SupplySourceRepository => new SupplySourceRepository(_context);
        public ISourcePriceRepository SourcePriceRepository => new SourcePriceRepository(_context);
        public IPartsListsRepository PartsListsRepository => new PartsListRepository(_context);

        public async Task<bool> Complete()
        {
            try { return await _context.SaveChangesAsync() > 0; }
            catch { return false; }
        }

        public bool HasChanges()
        {
            return _context.ChangeTracker.HasChanges();
        }
    }
}