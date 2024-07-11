using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ReminderHostedService : IHostedService, IDisposable
    {
        private Timer _timer;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<ReminderHostedService> _logger;

        public ReminderHostedService(IServiceScopeFactory scopeFactory, ILogger<ReminderHostedService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(SendReminders, null, TimeSpan.Zero, TimeSpan.FromMinutes(5));
            return Task.CompletedTask;
        }

        private async void SendReminders(object state)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var scheduleService = scope.ServiceProvider.GetRequiredService<IScheduleService>();

                try
                {
                    await scheduleService.SendClassRemindersAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while sending reminders.");
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }   
}
