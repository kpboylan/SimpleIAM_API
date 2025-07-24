using SimpleIAM_API.DTO;
using SimpleIAM_API.Entity;

namespace SimpleIAM_API.Service
{
    public interface IUserService
    {
        Task<UserDto> RegisterAsync(string email, string password);
        Task<UserDto> RegisterWithGroupAsync(string email, string password, string groupName);
        Task<User> AuthenticateAsync(string email, string password);
        Task AssignGroupAsync(string email, int groupdId);
        Task<UserDto?> GetByEmailAsync(string email);
        Task<List<UserDto>> GetAllAsync();
        Task<List<Group>> GetAllGroupsAsync();
    }
}
