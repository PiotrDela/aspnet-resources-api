
namespace ResourcesManagementApi.Domain.Entities
{
    public class ResourceLock
    {
        public User Owner { get; internal set; }
        public DateTime ExpirationTimeUtc { get; internal set; }

        public static ResourceLock CreatePermanentLock(User owner)
        {
            return new ResourceLock(owner, DateTime.MaxValue);
        }

        public static ResourceLock CreateTemporaryLock(User owner, TimeSpan lockDuration)
        {
            return new ResourceLock(owner, DateTime.UtcNow.Add(lockDuration));
        }

        private ResourceLock(User owner, DateTime expirationTimeUtc)
        {
            this.Owner = owner;
            this.ExpirationTimeUtc = expirationTimeUtc;
        }

        public bool IsActive { get { return DateTime.Now <= ExpirationTimeUtc; } }

        public bool OwnedBy(User user)
        {
            return Owner.Id == user.Id;
        }
    }
}