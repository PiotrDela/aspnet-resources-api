using MediatR;
using ResourcesManagementApi.Application.Commands;
using ResourcesManagementApi.Domain.Repositories;

namespace ResourcesManagementApi.Application.CommandHandlers
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

            return await resourceRepository.AddAsync(resource);
        }
    }
}
