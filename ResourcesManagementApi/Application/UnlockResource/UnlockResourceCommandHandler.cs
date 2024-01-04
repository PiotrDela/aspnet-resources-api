using MediatR;
using ResourcesManagementApi.Domain.Repositories;

namespace ResourcesManagementApi.Application.UnlockResource
{
    public class UnlockResourceCommandHandler : IRequestHandler<UnlockResourceCommand>
    {
        private readonly IResourceRepository resourceRepository;
        private readonly IUserRepository userRepository;

        public UnlockResourceCommandHandler(IResourceRepository resourceRepository, IUserRepository userRepository)
        {
            this.resourceRepository = resourceRepository ?? throw new ArgumentNullException(nameof(resourceRepository));
            this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task Handle(UnlockResourceCommand request, CancellationToken cancellationToken)
        {
            var getRequestingUserTask = userRepository.GetAsync(request.RequestingUserId);
            var getResourceTask = resourceRepository.GetAsync(request.ResourceId);

            await Task.WhenAll(getRequestingUserTask, getResourceTask);

            if (getRequestingUserTask.Result == null)
            {
                throw new Domain.Exceptions.EntityNotFoundException($"Could not find User with id: {request.RequestingUserId}");
            }

            var resource = getResourceTask.Result;
            if (resource == null)
            {
                throw new Domain.Exceptions.EntityNotFoundException($"Could not find resource with id: {request.ResourceId}");
            }

            resource.Unlock(getRequestingUserTask.Result);
            await resourceRepository.UpdateAsync(resource);
        }
    }
}
