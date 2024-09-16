
using Core.Interfaces.licenses;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data.Licenses
{
    public class DailyLicenseCheckService : BackgroundService
    {

        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<DailyLicenseCheckService> _logger;

        public DailyLicenseCheckService(IServiceScopeFactory serviceScopeFactory, ILogger<DailyLicenseCheckService> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var nextRun = DateTime.Today.AddDays(1).AddHours(0);
                //var nextRun = DateTime.Now.AddMinutes(1); // Schedule to run every minute
                var delay = nextRun - DateTime.Now;

                if (delay.TotalMilliseconds > 0)
                {
                    _logger.LogInformation("Next run scheduled in {Delay} milliseconds", delay.TotalMilliseconds);
                    await Task.Delay(delay, stoppingToken);
                }

                _logger.LogInformation("Running license expiration check at {Time}.", DateTimeOffset.Now);

                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var licenseExpirationService = scope.ServiceProvider.GetRequiredService<ILicenseExpirationService>();
                    await licenseExpirationService.CheckLicenseExpirationAsync();
                }

                _logger.LogInformation("License expiration check completed at {Time}.", DateTimeOffset.Now);
            }
        }
    }
} 
