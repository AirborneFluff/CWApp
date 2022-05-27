using Microsoft.AspNetCore.Identity;

namespace API.Interfaces
{
    public interface IUsersRepository
    {
        void AddNewUser(User user);
        void RemoveUser(User user);
        Task<User> GetUserById(int id);
        Task<User> GetUserByName(string firstName, string lastName);
        Task<PagedList<User>> GetUsers(PaginationParams pageParams);
    }
}