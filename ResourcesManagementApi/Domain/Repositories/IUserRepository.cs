using ResourcesManagementApi.Domain.Entities;

namespace ResourcesManagementApi.Domain.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetAsync(int id);
    }
}
