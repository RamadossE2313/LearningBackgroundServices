
using Microsoft.Extensions.Caching.Memory;

namespace LearningBackgroundServices.UserProfiles.Services
{
    public class TimedBackgroundService : BackgroundService
    {
        private readonly IUserProfileService _userProfileService;
        private readonly IMemoryCache _memoryCache;
        private readonly TimeSpan _scheduledTime = new TimeSpan(4, 15, 0);

        public TimedBackgroundService(IUserProfileService userProfileService, IMemoryCache memoryCache)
        {
            _userProfileService = userProfileService ?? throw new ArgumentNullException(nameof(userProfileService));
            _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var profiles = _memoryCache.Get<List<UserProfile>>("profiles");
            if (profiles == null)
            {
                ScheduleTask();
            }
        }

        private void ScheduleTask()
        {
            var cetTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
            var currentTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, cetTimeZone);
            var scheduledTimeToRunTask = currentTime.Date + _scheduledTime;

            if (currentTime > scheduledTimeToRunTask)
            {
                scheduledTimeToRunTask = scheduledTimeToRunTask.AddDays(1);
            }

            var initialDelay = scheduledTimeToRunTask - currentTime;
            new Timer(LoadData, null, initialDelay, TimeSpan.FromHours(24));
        }

        private async void LoadData(object? state)
        {
            var profileResponse = await _userProfileService.GetUserProfile(1);
            _memoryCache.Set("profiles", profileResponse.Data);
            ScheduleTask();
        }
    }
}
