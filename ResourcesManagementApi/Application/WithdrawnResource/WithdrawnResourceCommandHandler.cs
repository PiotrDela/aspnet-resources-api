﻿using MediatR;
using ResourcesManagementApi.Domain.Repositories;

namespace ResourcesManagementApi.Application.WithdrawnResource
{
    public class WithdrawnResourceCommandHandler : IRequestHandler<WithdrawnResourceCommand>
    {
        private readonly IResourceRepository resourceRepository;

        public WithdrawnResourceCommandHandler(IResourceRepository resourceRepository)
        {
            this.resourceRepository = resourceRepository ?? throw new ArgumentNullException(nameof(resourceRepository));
        }

        public async Task Handle(WithdrawnResourceCommand request, CancellationToken cancellationToken)
        {
            var resource = await resourceRepository.GetAsync(request.ResourceId);
            if (resource == null)
            {
                throw new Domain.Exceptions.EntityNotFoundException($"Could not find resource with id: {request.ResourceId}");
            }

            resource.Withdrawn();
            await resourceRepository.UpdateAsync(resource); // TODO: passing cancellation token would nice as well
        }
    }
}
