namespace API.Data.Repositorys
{
    public class UsersRepository : IUsersRepository
    {
        private readonly DataContext _context;
        public UsersRepository(DataContext context)
        {
            this._context = context;
        }

        public void AddNewUser(User user)
        {
            _context.Users.Add(user);
        }

        public async Task<User> GetUserById(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User> GetUserByName(string firstName, string lastName)
        {
            return await _context.Users.FirstOrDefaultAsync(u =>
                u.FirstName.ToUpper() == firstName.ToUpper() &&
                u.LastName.ToUpper() == lastName.ToUpper());
        }

        public void RemoveUser(User user)
        {
            _context.Users.Remove(user);
        }
    }
}