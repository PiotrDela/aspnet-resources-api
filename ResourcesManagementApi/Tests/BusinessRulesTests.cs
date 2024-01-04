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

            Assert.Throws<BusinessRuleValidationException>(() => resource.Lock(new User(), TimeSpan.FromDays(1)));
        }

        [Fact]
        public void ShouldBePossibleToLockResourceIfExistingLockExpired()
        {
            var expiredlockOwner = new User() { Id = 1, Name = "John" };

            var resource = new Resource();
            resource.Lock(expiredlockOwner, TimeSpan.FromHours(-1));

            var newLockOwner = new User();
            resource.Lock(newLockOwner, TimeSpan.FromDays(2));

            Assert.True(resource.CurrentLock.OwnedBy(newLockOwner));
        }

        [Fact]
        public void ShouldNotAllowToLockResourceAlreadyLockedByDifferentUser()
        {
            var lockOwner = new User() { Id = 1, Name = "John" };

            var resource = new Resource();
            resource.Lock(lockOwner, TimeSpan.FromDays(1));

            Assert.Throws<BusinessRuleValidationException>(() => resource.Lock(new User(), TimeSpan.FromDays(1)));
        }

        [Fact]
        public void ShouldNotAllowToUnlockResourceAlreadyLockedByDifferentUser()
        {
            var lockOwner = new User() { Id = 1, Name = "John" };

            var resource = new Resource();
            resource.Lock(lockOwner, TimeSpan.FromDays(5));

            Assert.Throws<BusinessRuleValidationException>(() => resource.Unlock(new User()));
        }

        [Fact]
        public void UnlockShouldNullLockSpecificProperties()
        {
            var resource = new Resource();
            var user = new User();

            resource.Lock(user, TimeSpan.FromDays(1));

            resource.Unlock(user);

            Assert.Null(resource.CurrentLock);
        }
    }
}
