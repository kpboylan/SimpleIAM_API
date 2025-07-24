using Microsoft.EntityFrameworkCore;
using SimpleIAM_API.DBPersistence;
using SimpleIAM_API.Repository;

namespace SimpleIAM_API.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ClinicalTrialDbContext _context;
        public UnitOfWork(ClinicalTrialDbContext context)
        {
            _context = context;
            Users = new UserRepository(_context);
            Groups = new GroupRepository(_context);
        }

        public IUserRepository Users { get; }
        public IGroupRepository Groups { get; }
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose() => _context.Dispose();
    }
}
