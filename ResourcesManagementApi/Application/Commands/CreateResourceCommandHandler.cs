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

        public async Task<int> Handle(CreateResourceCommand request, CancellationToken cancellationToken)
        {
            var resource = new Domain.Entities.Resource();

            return await this.resourceRepository.AddAsync(resource);
        }
    }
}
