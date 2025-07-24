using SimpleIAM_API.DTO;
using SimpleIAM_API.Entity;

namespace SimpleIAM_API.Repository
{
    public interface IUserRepository
    {
        Task<User> GetByEmailAsync(string email);
        Task<List<User>> GetAllAsync();
        Task<List<Group>> GetAllGroupsAsync();
        Task AddAsync(User user);
        Task SaveChangesAsync();
    }
}
