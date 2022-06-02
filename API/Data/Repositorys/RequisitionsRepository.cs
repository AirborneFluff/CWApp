using API.DTOs.RequisitionDTOs;
using AutoMapper.QueryableExtensions;

namespace API.Data.Repositorys
{
    public class RequisitionsRepository : IRequisitionsRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public RequisitionsRepository(DataContext context, IMapper mapper)
        {
            this._mapper = mapper;
            this._context = context;
        }

        public void AddNewRequisition(Requisition requisition)
        {
            _context.Requisitions.Add(requisition);
        }

        public async Task<Requisition> GetOpenRequisitionForPart(int partId)
        {
            return await _context.Requisitions
                .Include(r => r.Part)
                .Include(r => r.User)
                .Where(r => r.OutboundOrderId == null && r.PartId == partId)
                .FirstOrDefaultAsync();
        }

        public async Task<RequisitionDetailsDto> GetRequisitionById(int id)
        {
            return await _context.Requisitions
                .Include(r => r.Part)
                .Include(r => r.User)
                .Include(r => r.OutboundOrder)
                .ProjectTo<RequisitionDetailsDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<PagedList<RequisitionDetailsDto>> GetRequisitions(PaginationParams pageParams)
        {
            var query = _context.Requisitions
                .Include(r => r.Part)
                .Include(r => r.User)
                .Where(r => r.OutboundOrderId == null)
                .OrderByDescending(p => p.Date)
                .ProjectTo<RequisitionDetailsDto>(_mapper.ConfigurationProvider)
                .AsQueryable()
                .AsNoTracking();

            return await PagedList<RequisitionDetailsDto>.CreateAsync(query, x => true, pageParams.PageNumber, pageParams.PageSize);
        }

        public async Task<IEnumerable<RequisitionDetailsDto>> GetRequisitionsForPart(int partId)
        {
            return await _context.Requisitions
                .Where(r => r.PartId == partId)
                .ProjectTo<RequisitionDetailsDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
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