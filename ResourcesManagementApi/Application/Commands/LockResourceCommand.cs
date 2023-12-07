using MediatR;

namespace ResourcesManagementApi.Application.Commands
{
    public class LockResourceCommand : IRequest
    {
        public int ResourceId { get; set; }
        public int RequestingUserId { get; set; }

        public TimeSpan LockDuration { get; set; }

        public LockResourceCommand(int resourceId, int requestingUserId, TimeSpan lockDuration)
        {
            ResourceId = resourceId;
            RequestingUserId = requestingUserId;
            LockDuration = lockDuration;
        }
    }
}
