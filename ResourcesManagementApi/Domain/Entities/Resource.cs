namespace ResourcesManagementApi.Domain.Entities
{
    public class Resource: EntityBase
    {
        public ResourceAvaiabilityStatus Status { get; private set; }

        public ResourceLock CurrentLock { get; private set; }

        public void Lock(User user, TimeSpan lockPeriod)
        {
            this.EnsureAvailability();
            this.EnsureNotLockedByDifferentUser(user);

            this.CurrentLock = ResourceLock.CreateTemporaryLock(user, lockPeriod);
        }

        public void LockPermanently(User user)
        {
            this.EnsureAvailability();
            this.EnsureNotLockedByDifferentUser(user);

            this.CurrentLock = ResourceLock.CreatePermanentLock(user);
        }

        private void EnsureAvailability()
        {
            if (this.Status == ResourceAvaiabilityStatus.Withdrawn)
            {
                throw new Exceptions.BusinessRuleValidationException("Unlock not allowed. Resource has been withdrawn already");
            }
        }

        private void EnsureNotLockedByDifferentUser(User user)
        {
            if (CurrentLock != null && CurrentLock.IsActive && CurrentLock.OwnedBy(user) == false)
            {
                throw new Exceptions.BusinessRuleValidationException("Unlock not permitted");
            }
        }

        public void Withdrawn()
        {
            this.Status = ResourceAvaiabilityStatus.Withdrawn;
        }

        public void Unlock(User user)
        {
            this.EnsureNotLockedByDifferentUser(user);

            this.CurrentLock = null;
        }
    }
}
