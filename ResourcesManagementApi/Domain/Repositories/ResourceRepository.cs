using ResourcesManagementApi.Domain.Entities;

namespace ResourcesManagementApi.Domain.Repositories
{
    public interface IResourceRepository
    {
        void Add(Resource resource);
        Resource Get(int id);

        void Update(Resource resource);
    }

    public class ResourceRepository
    {
    }
}
