using Microsoft.EntityFrameworkCore;
using SimpleIAM_API.DBPersistence;
using SimpleIAM_API.Entity;

namespace SimpleIAM_API.Repository
{
    public class GroupRepository : IGroupRepository
    {
        private readonly ClinicalTrialDbContext _context;

        public GroupRepository(ClinicalTrialDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Group group)
        {
            await _context.Groups.AddAsync(group);
        }
    }
}
