using MediatR;

namespace ResourcesManagementApi.Application.Commands
{
    public class UnlockResourceCommand: IRequest
    { 
        public int ResourceId { get; set; }
        public int RequestingUserId { get; set; }

        public UnlockResourceCommand(int resourceId, int requestingUserId)
        {
            ResourceId = resourceId;
            RequestingUserId = requestingUserId;
        }
    }
}
