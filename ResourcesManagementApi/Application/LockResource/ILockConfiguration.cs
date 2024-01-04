namespace ResourcesManagementApi.Application.LockResource
{
    public interface ILockConfiguration
    {
        TimeSpan TemporaryLockDuration { get; }
    }
}
