using Microsoft.EntityFrameworkCore;
using SimpleIAM_API.DBPersistence;
using SimpleIAM_API.Entity;

namespace SimpleIAM_API.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ClinicalTrialDbContext _context;

        public UserRepository(ClinicalTrialDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _context.Users
                .Include(u => u.Groups)
                    .ThenInclude(ug => ug.Group)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _context.Users
                .Include(u => u.Groups)
                    .ThenInclude(ug => ug.Group)
                .ToListAsync();
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
