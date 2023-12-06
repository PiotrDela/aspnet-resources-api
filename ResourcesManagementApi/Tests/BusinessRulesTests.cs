using ResourcesManagementApi.Domain.Entities;
using ResourcesManagementApi.Domain.Exceptions;
using Xunit;

namespace ResourcesManagementApi.Tests
{
    public class BusinessRulesTests
    {
        [Fact]
        public void ShouldNotAllowToLockAlreadyWithdrawnResource()
        {
            var resource = new Resource();
            resource.Withdrawn();

            Assert.Throws<BusinessRuleValidationException>(() => resource.LockBy(new User(), TimeSpan.FromDays(1)));
        }

        [Fact]
        public void ShouldNotAllowToLockResourceAlreadyLockedByDifferentUser()
        {
            var lockOwner = new User() { Id = 1, Name = "John" };

            var resource = new Resource() { Status = ResourceAvaiabilityStatus.Available, LockedById = lockOwner.Id, LockExpirationTimeUtc = DateTime.UtcNow.AddDays(1)  };

            Assert.Throws<BusinessRuleValidationException>(() => resource.LockBy(new User(), TimeSpan.FromDays(1)));
        }

        [Fact]
        public void ShouldNotAllowToUnlockResourceAlreadyLockedByDifferentUser()
        {
            var lockOwner = new User() { Id = 1, Name = "John" };

            var resource = new Resource() { Status = ResourceAvaiabilityStatus.Available, LockedById = lockOwner.Id, LockExpirationTimeUtc = DateTime.UtcNow.AddDays(1) };

            Assert.Throws<BusinessRuleValidationException>(() => resource.LockBy(new User(), TimeSpan.FromDays(1)));
        }

        [Fact]
        public void LockShouldAssignLockedById()
        {
            var resource = new Resource();
            var user = new User();

            resource.LockBy(user, TimeSpan.FromDays(1));

            Assert.Equal(resource.LockedById, user.Id);
            // TODO: check for lock expiration time, need to dealt with DateTime.UtcNow() by abtracting a datetime provider
        }

        [Fact]
        public void UnlockShouldNullLockSpecificProperties()
        {
            var resource = new Resource();
            var user = new User();

            resource.LockBy(user, TimeSpan.FromDays(1));

            resource.Unlock(user);

            Assert.Null(resource.LockedById);
            Assert.Null(resource.LockExpirationTimeUtc);
        }
    }
}
