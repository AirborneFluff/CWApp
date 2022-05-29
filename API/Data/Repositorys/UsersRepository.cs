namespace API.Data.Repositorys
{
    public class UsersRepository : IUsersRepository
    {
        private readonly DataContext _context;
        public UsersRepository(DataContext context)
        {
            this._context = context;
        }

        public void AddNewUser(AppUser user)
        {
            _context.Users.Add(user);
        }

        public async Task<AppUser> GetUserById(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<AppUser> GetUserByName(string firstName, string lastName)
        {
            return await _context.Users.FirstOrDefaultAsync(u =>
                u.FirstName.ToUpper() == firstName.ToUpper() &&
                u.LastName.ToUpper() == lastName.ToUpper());
        }

        public async Task<PagedList<AppUser>> GetUsers(PaginationParams pageParams)
        {
            var query = _context.Users.OrderBy(u => u.LastName).AsQueryable();

            return await PagedList<AppUser>.CreateAsync(query, x => true, pageParams.PageNumber, pageParams.PageSize);
        }

        public void RemoveUser(AppUser user)
        {
            _context.Users.Remove(user);
        }
    }
}