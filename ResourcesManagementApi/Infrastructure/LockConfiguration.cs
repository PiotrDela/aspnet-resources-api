using ResourcesManagementApi.Application.LockResource;

namespace ResourcesManagementApi.Infrastructure
{
    public class LockConfiguration : ILockConfiguration
    {
        private readonly IConfiguration configuration;

        public LockConfiguration(IConfiguration configuration)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public TimeSpan TemporaryLockDuration
        {
            get
            {
                var temporaryLockPeriodInHours = configuration.GetValue<int>("TemporaryLockPeriodInHours");
                return TimeSpan.FromHours(temporaryLockPeriodInHours);
            }
        }
    }
}
