using ResourcesManagementApi.Domain.Entities;

namespace ResourcesManagementApi.Domain.Repositories
{
    public interface IResourceRepository
    {
        Task<int> AddAsync(Resource resource);
        Task<Resource> GetAsync(int id);

        Task UpdateAsync(Resource resource);
    }
}
