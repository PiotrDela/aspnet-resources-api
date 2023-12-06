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

        public async Task Handle(ModifyResourceCommand request, CancellationToken cancellationToken)
        {
            var resource = await this.resourceRepository.GetAsync(request.ResourceId);
            if (resource == null)
            {
                throw new Domain.Exceptions.EntityNotFoundException($"Could not find resource with id: {request.ResourceId}");
            }

            request.ModifyAction(resource);
            await this.resourceRepository.UpdateAsync(resource);
        }
    }
}
