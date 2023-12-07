using MediatR;
using ResourcesManagementApi.Domain.Repositories;

namespace ResourcesManagementApi.Application.Commands
{
    public class LockResourceCommandHandler : IRequestHandler<LockResourceCommand>
    {
        private readonly IResourceRepository resourceRepository;
        private readonly IUserRepository userRepository;

        public LockResourceCommandHandler(IResourceRepository resourceRepository, IUserRepository userRepository)
        {
            this.resourceRepository = resourceRepository ?? throw new ArgumentNullException(nameof(resourceRepository));
            this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task Handle(LockResourceCommand request, CancellationToken cancellationToken)
        {
            var getRequestingUserTask = this.userRepository.GetAsync(request.RequestingUserId);
            var getResourceTask = this.resourceRepository.GetAsync(request.ResourceId);

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

            resource.LockBy(getRequestingUserTask.Result, request.LockDuration);
            await this.resourceRepository.UpdateAsync(resource);
        }
    }
}
