using SimpleIAM_API.Repository;

namespace SimpleIAM_API.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        IGroupRepository Groups { get; }
        Task<int> SaveChangesAsync();
    }
}
