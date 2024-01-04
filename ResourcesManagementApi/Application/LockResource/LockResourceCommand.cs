using MediatR;

namespace ResourcesManagementApi.Application.LockResource
{
    public class LockResourceCommand : IRequest
    {
        public int ResourceId { get; set; }
        public int RequestingUserId { get; set; }

        public bool UsePermanentLock { get; set; }

        public LockResourceCommand(int resourceId, int requestingUserId, bool usePermanentLock = false)
        {
            ResourceId = resourceId;
            RequestingUserId = requestingUserId;
            UsePermanentLock = usePermanentLock;
        }
    }
}
