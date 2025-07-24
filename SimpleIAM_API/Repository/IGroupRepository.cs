using SimpleIAM_API.Entity;

namespace SimpleIAM_API.Repository
{
    public interface IGroupRepository
    {
        Task AddAsync(Group group);
    }
}
