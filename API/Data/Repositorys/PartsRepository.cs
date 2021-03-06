using AutoMapper.QueryableExtensions;

namespace API.Data.Repositorys
{
    public class PartsRepository : IPartsRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public PartsRepository(DataContext context, IMapper mapper)
        {
            this._mapper = mapper;
            this._context = context;
        }

        public void AddPart(NewPartDto part)
        {
            _context.Parts.Add(_mapper.Map<Part>(part));
        }

        public async Task<bool> Exists(string partCode)
        {
            var part = await _context.Parts.FirstOrDefaultAsync(p => p.PartCode == partCode);
            if (part == null) return false;
            return true;
        }

        public async Task<Part> GetPartById(int id)
        {
            return await _context.Parts.FindAsync(id);
        }

        public async Task<Part> GetPartByPartCode(string partCode)
        {
            var part = _context.Parts
                .Include(p => p.SupplySources)
                .ThenInclude(s => s.Supplier)
                .Include(p => p.SupplySources)
                .ThenInclude(s => s.Prices)
                .Include(p => p.Requisitions)
                .FirstOrDefaultAsync(p => p.PartCode == partCode);

            part.Result.Requisitions = part.Result.Requisitions.OrderByDescending(r => r.Id).ToList();
            return await part;

        }

        public async Task<PagedList<PartDto>> GetParts(PaginationParams partParams, Func<PartDto, bool> predicate)
        {
            var query = _context.Parts.OrderBy(p => p.PartCode).AsQueryable();
            var _query = query.ProjectTo<PartDto>(_mapper.ConfigurationProvider).AsNoTracking();

            return await PagedList<PartDto>.CreateAsync(_query, predicate, partParams.PageNumber, partParams.PageSize);
        }

        public void RemovePart(Part part)
        {
            _context.Parts.Remove(part);
        }

        public async Task RemovePartByPartCode(string partCode)
        {
            var part = await GetPartByPartCode(partCode);
            if (part == null) return;
            RemovePart(part);
        }

        public Task UpdatePart(Part part)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Part>> GetAllPartsAsList()
        {
            return await _context.Parts.ToListAsync();
        }

        public async Task<List<string>> GetAllPartCodes()
        {
            return await _context.Parts.Select(p => p.PartCode).ToListAsync();
        }
    }
}