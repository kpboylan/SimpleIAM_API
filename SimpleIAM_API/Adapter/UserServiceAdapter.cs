using Microsoft.EntityFrameworkCore;
using SimpleIAM_API.DBPersistence;
using SimpleIAM_API.DTO;
using SimpleIAM_API.Entity;
using SimpleIAM_API.Factory;
using SimpleIAM_API.Helper;
using SimpleIAM_API.Repository;
using SimpleIAM_API.Service;
using SimpleIAM_API.UnitOfWork;
using static SimpleIAM_API.Factory.Enum.FactoryEnum;

namespace SimpleIAM_API.Adapter
{
    public class UserServiceAdapter : IUserService
    {
        private readonly LegacyUserService _legacyUserService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _repo;
        private readonly ClinicalTrialDbContext _context;

        public UserServiceAdapter(IUnitOfWork unitOfWork, IUserRepository repo, ClinicalTrialDbContext context, LegacyUserService legacyUserService)
        {
            _unitOfWork = unitOfWork;
            _repo = repo;
            _context = context;
            _legacyUserService = legacyUserService;
        }

        public async Task<UserDto> RegisterAsync(string email, string password)
        {
            _legacyUserService.RegisterUser(email);

            var user = new User { Email = email };

            SendNotification(email);

            return new UserDto
            {
                Email = user.Email
            };
        }

        private void SendNotification(string email)
        {
            var emailNotification = NotificationFactory.CreateNotification(NotificationType.Email);
            emailNotification.Send(email, "This is an email message.");
        }

        public async Task<UserDto> RegisterWithGroupAsync(string email, string password, string groupName)
        {
            var (isValid, errors) = UserRegistration.ValidateCredentials(email, password);

            if (!isValid)
            {
                throw new ArgumentException(string.Join(" ", errors));
            }

            var existing = await _repo.GetByEmailAsync(email);
            if (existing != null)
                throw new InvalidOperationException("User already exists");

            var group = new Group { Name = groupName };
            var user = new User { Email = email, Password = password };

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.Groups.AddAsync(group);
            await _unitOfWork.SaveChangesAsync();

            return new UserDto
            {
                Id = user.Id,
                Email = user.Email, // or user.Username depending on your model
                GroupName = groupName // default empty list for new users
            };
        }

        public async Task<User> AuthenticateAsync(string email, string password)
        {
            var user = await _repo.GetByEmailAsync(email);
            if (user == null || user.Password != password)
                throw new InvalidOperationException("Invalid credentials");

            return user;
        }

        public async Task AssignGroupAsync(string email, int groupId)
        {
            var user = await _repo.GetByEmailAsync(email);
            if (user == null)
                throw new KeyNotFoundException("User not found");

            var group = await _context.Groups.FirstOrDefaultAsync(g => g.Id == groupId);
            if (group == null)
                throw new KeyNotFoundException("Group not found");

            bool alreadyInGroup = user.Groups.Any(g => g.GroupId == group.Id);
            if (!alreadyInGroup)
            {
                user.Groups.Add(new UserGroup
                {
                    GroupId = group.Id,
                    UserId = user.Id
                });

                await _repo.SaveChangesAsync();
            }
        }

        public async Task<UserDto?> GetByEmailAsync(string email)
        {
            var user = await _repo.GetByEmailAsync(email);
            if (user == null)
                return null;

            // Something like AutoMapper is an option here but that would be overkill in this simple project
            return new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                Groups = user.Groups.Select(g => g.Group.Name).ToList()
            };
        }

        public async Task<List<UserDto>> GetAllAsync()
        {
            var users = await _repo.GetAllAsync(); // raw User entities

            return users.Select(u => new UserDto
            {
                Id = u.Id,
                Email = u.Email,
                Groups = u.Groups.Select(g => g.Group.Name).ToList()
            }).ToList();
        }

        public async Task<List<Group>> GetAllGroupsAsync()
        {
            return await _repo.GetAllGroupsAsync();
        }
    }
}
