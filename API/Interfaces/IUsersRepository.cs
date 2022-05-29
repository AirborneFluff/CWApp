using Microsoft.AspNetCore.Identity;

namespace API.Interfaces
{
    public interface IUsersRepository
    {
        void AddNewUser(AppUser user);
        void RemoveUser(AppUser user);
        Task<AppUser> GetUserById(int id);
        Task<AppUser> GetUserByName(string firstName, string lastName);
        Task<PagedList<AppUser>> GetUsers(PaginationParams pageParams);
    }
}