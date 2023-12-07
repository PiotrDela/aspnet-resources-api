using MediatR;

namespace ResourcesManagementApi.Application.Commands
{
    public class WithdrawnResourceCommand: IRequest
    {
        public int ResourceId { get; set; }

        public WithdrawnResourceCommand(int resourceId)
        {
            this.ResourceId = resourceId;
        }
    }
}
