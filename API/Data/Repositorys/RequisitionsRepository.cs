namespace API.Data.Repositorys
{
    public class RequisitionsRepository : IRequisitionsRepository
    {
        private readonly DataContext _context;
        public RequisitionsRepository(DataContext context)
        {
            this._context = context;
        }

        public void AddNewRequisition(Requisition requisition)
        {
            _context.Requisitions.Add(requisition);
        }

        public async Task<Requisition> GetNotOrderedRequisitionForPart(int partId)
        {
            return await _context.Requisitions
                .Include(r => r.Part)
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.OutboundOrderId == null);
        }

        public async Task<Requisition> GetRequisitionById(int id)
        {
            return await _context.Requisitions
                .Include(r => r.Part)
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<PagedList<Requisition>> GetRequisitions(PaginationParams pageParams)
        {
            var query = _context.Requisitions
                .Include(r => r.Part)
                .Include(r => r.User)
                .OrderBy(p => p.Date)
                .AsQueryable()
                .AsNoTracking();

            return await PagedList<Requisition>.CreateAsync(query, x => true, pageParams.PageNumber, pageParams.PageSize);
        }

        public async Task<IEnumerable<Requisition>> GetRequisitionsForPart(int partId)
        {
            return await _context.Requisitions.Where(r => r.PartId == partId).ToListAsync();
        }

        public async Task<IEnumerable<Requisition>> GetRequisitionsFromDate(DateTime fromDate)
        {
            return await _context.Requisitions.Where(r => r.Date >= fromDate).ToListAsync();
        }

        public void RemoveRequisition(Requisition requisition)
        {
            _context.Requisitions.Remove(requisition);
        }
    }
}