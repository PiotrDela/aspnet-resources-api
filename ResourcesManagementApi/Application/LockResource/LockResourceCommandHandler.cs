using MediatR;
using ResourcesManagementApi.Domain.Repositories;

namespace ResourcesManagementApi.Application.LockResource
{
    public class LockResourceCommandHandler : IRequestHandler<LockResourceCommand>
    {
        private readonly IResourceRepository resourceRepository;
        private readonly IUserRepository userRepository;
        private readonly ILockConfiguration configuration;

        public LockResourceCommandHandler(IResourceRepository resourceRepository, IUserRepository userRepository, ILockConfiguration configuration)
        {
            this.resourceRepository = resourceRepository ?? throw new ArgumentNullException(nameof(resourceRepository));
            this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task Handle(LockResourceCommand request, CancellationToken cancellationToken)
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

            if (request.UsePermanentLock)
            {
                resource.LockPermanently(getRequestingUserTask.Result);
            }
            else
            {
                resource.Lock(getRequestingUserTask.Result, this.configuration.TemporaryLockDuration);
            }

            await resourceRepository.UpdateAsync(resource);
        }
    }
}
