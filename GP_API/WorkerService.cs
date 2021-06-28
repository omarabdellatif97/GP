using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GP_API
{
    public abstract class TimedHostedService : IHostedService, IDisposable
    {
        private readonly ILogger _logger;
        private Timer _timer;
        private Task _executingTask;
        private readonly CancellationTokenSource _stoppingCts = new CancellationTokenSource();

        IServiceProvider _services;
        public TimedHostedService(IServiceProvider services)
        {
            _services = services;
            _logger = _services.GetRequiredService<ILogger<TimedHostedService>>();

        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(ExecuteTask, null, FirstRunAfter, TimeSpan.FromMilliseconds(-1));

            return Task.CompletedTask;
        }

        private void ExecuteTask(object state)
        {
            _timer?.Change(Timeout.Infinite, 0);
            _executingTask = ExecuteTaskAsync(_stoppingCts.Token);
        }

        private async Task ExecuteTaskAsync(CancellationToken stoppingToken)
        {
            try
            {
                using (var scope = _services.CreateScope())
                {
                    await RunJobAsync(scope.ServiceProvider, stoppingToken);
                }
            }
            catch (Exception exception)
            {
                _logger.LogError("BackgroundTask Failed", exception);
            }
            _timer.Change(Interval, TimeSpan.FromMilliseconds(-1));
        }

        /// <summary>
        /// This method is called when the <see cref="IHostedService"/> starts. The implementation should return a task 
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="stoppingToken">Triggered when <see cref="IHostedService.StopAsync(CancellationToken)"/> is called.</param>
        /// <returns>A <see cref="Task"/> that represents the long running operations.</returns>
        protected abstract Task RunJobAsync(IServiceProvider serviceProvider, CancellationToken stoppingToken);
        protected abstract TimeSpan Interval { get; }

        protected abstract TimeSpan FirstRunAfter { get; }

        public virtual async Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);

            // Stop called without start
            if (_executingTask == null)
            {
                return;
            }

            try
            {
                // Signal cancellation to the executing method
                _stoppingCts.Cancel();
            }
            finally
            {
                // Wait until the task completes or the stop token triggers
                await Task.WhenAny(_executingTask, Task.Delay(Timeout.Infinite, cancellationToken));
            }

        }

        public void Dispose()
        {
            _stoppingCts.Cancel();
            _timer?.Dispose();
        }
    }
}
