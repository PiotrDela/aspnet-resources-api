using MediatR;
using ResourcesManagementApi.Domain.Repositories;

namespace ResourcesManagementApi.Application.Commands
{
    public class CreateResourceCommandHandler : IRequestHandler<CreateResourceCommand, int>
    {
        private readonly IResourceRepository resourceRepository;

        public CreateResourceCommandHandler(IResourceRepository resourceRepository)
        {
            this.resourceRepository = resourceRepository ?? throw new ArgumentNullException(nameof(resourceRepository));
        }

        public Task<int> Handle(CreateResourceCommand request, CancellationToken cancellationToken)
        {
            var resource = new Domain.Entities.Resource();

            this.resourceRepository.Add(resource);

            return Task.FromResult(resource.Id);
        }
    }
}
