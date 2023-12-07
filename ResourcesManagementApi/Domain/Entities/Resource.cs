namespace ResourcesManagementApi.Domain.Entities
{
    public class Resource: EntityBase
    {
        public ResourceAvaiabilityStatus Status { get; set; }
        public DateTime? LockExpirationTimeUtc { get; set; }
        public int? LockedById { get; set; }

        public void LockBy(User user, TimeSpan lockPeriod)
        {
            if (this.Status == ResourceAvaiabilityStatus.Withdrawn)
            {
                throw new Exceptions.BusinessRuleValidationException("Unlock not allowed. Resource has been withdrawn already");
            }

            var utcNow = DateTime.UtcNow;
            if (LockedById.HasValue && LockedById.Value != user.Id && utcNow < this.LockExpirationTimeUtc)
            {
                throw new Exceptions.BusinessRuleValidationException("Unlock not permitted");
            }

            this.LockedById = user.Id;
            this.LockExpirationTimeUtc = lockPeriod == TimeSpan.MaxValue ? DateTime.MaxValue : DateTime.UtcNow.Add(lockPeriod);
        }

        public void Withdrawn()
        {
            this.Status = ResourceAvaiabilityStatus.Withdrawn;
        }

        public void Unlock(User user)
        {
            if (LockedById.HasValue && LockedById.Value != user.Id)
            {
                throw new Exceptions.BusinessRuleValidationException("Unlock not permitted");
            }

            this.LockExpirationTimeUtc = null;
            this.LockedById = null;
        }
    }

    public enum ResourceAvaiabilityStatus
    {
        Available = 0,
        Withdrawn = 1,
    }
}
