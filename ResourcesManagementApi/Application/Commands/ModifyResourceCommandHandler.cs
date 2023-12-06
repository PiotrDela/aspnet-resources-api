using MediatR;
using ResourcesManagementApi.Domain.Repositories;

namespace ResourcesManagementApi.Application.Commands
{
    public class ModifyResourceCommandHandler : IRequestHandler<ModifyResourceCommand>
    {
        private readonly IResourceRepository resourceRepository;

        public ModifyResourceCommandHandler(IResourceRepository resourceRepository)
        {
            this.resourceRepository = resourceRepository ?? throw new ArgumentNullException(nameof(resourceRepository));
        }

        public Task Handle(ModifyResourceCommand request, CancellationToken cancellationToken)
        {
            var resource = this.resourceRepository.Get(request.ResourceId);
            if (resource == null)
            {
                throw new Domain.Exceptions.EntityNotFoundException($"Could not find resource with id: {request.ResourceId}");
            }

            request.ModifyAction(resource);
            this.resourceRepository.Update(resource);

            return Task.CompletedTask;
        }
    }
}
