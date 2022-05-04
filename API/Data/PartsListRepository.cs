namespace API.Data
{
    public class PartsListRepository : IPartsListsRepository
    {
        private readonly DataContext _context;
        public PartsListRepository(DataContext context)
        {
            this._context = context;
        }

        public void AddNewPartsList(PartsList newList)
        {
            _context.PartsLists.Add(newList);
        }

        public Task<List<PartsList>> GetAllPartsLists()
        {
            return _context.PartsLists.ToListAsync();
        }

        public async Task<PartsList> GetPartsListFromId(int partsListId)
        {
            return await _context.PartsLists.Include(l => l.Parts).FirstOrDefaultAsync(l => l.Id == partsListId);
        }

        public async Task<PartsList> GetPartsListFromTitle(string title)
        {
            return await _context.PartsLists.Include(l => l.Parts).FirstOrDefaultAsync(x => x.Title.ToLower() == title.ToLower());
        }

        public void RemovePartsList(PartsList partsList)
        {
            _context.PartsLists.Remove(partsList);
        }
    }
}