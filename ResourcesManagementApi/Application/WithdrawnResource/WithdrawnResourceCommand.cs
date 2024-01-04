using MediatR;

namespace ResourcesManagementApi.Application.WithdrawnResource
{
    public class WithdrawnResourceCommand : IRequest
    {
        public int ResourceId { get; set; }

        public WithdrawnResourceCommand(int resourceId)
        {
            ResourceId = resourceId;
        }
    }
}
