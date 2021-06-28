using DAL.Models;
using GP_API.Services;
using GP_API.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GP_API
{
    /// <summary>
    /// this service is called every duration of time to update the casefile 
    /// to new directories based on the it's case, NOTE: that the DirectoryPerCase if it has no valid 
    /// value, the interval of service will be 30 minutes
    /// </summary>
    public class ScheduledCaseFileWorkerService : TimedHostedService
    {
        protected TimeSpan interval;
        protected DirectoryPerCaseServiceSettings settings;
        public ScheduledCaseFileWorkerService(IServiceProvider services) : base(services)
        {


            settings = services.GetRequiredService<DirectoryPerCaseServiceSettings>();

            if (!settings.EnableService)
            {
                this.Dispose();
                return;
            }

            interval = new TimeSpan(settings.Hours, settings.Minutes, settings.Seconds);

            // if one of them is true the duration will be 30 minutes
            var falseConditions = new List<Predicate<TimeSpan>>()
            {
                (span)=> span == TimeSpan.MaxValue,
                (span)=> span == TimeSpan.MinValue,
                (span)=> span == TimeSpan.Zero,
                (span)=> span < TimeSpan.FromSeconds(30)
            };

            if (falseConditions.Any(f => f.Invoke(interval)))
                interval = new TimeSpan(0, 30, 0);
        }

        protected override TimeSpan Interval => interval;

        protected override TimeSpan FirstRunAfter => interval;

        protected async override Task RunJobAsync(IServiceProvider serviceProvider, CancellationToken stoppingToken)
        {
            if(!settings.EnableService)
            {
                this.Dispose();
                return;
            }

            var db = serviceProvider.GetRequiredService<CaseContext>();
            var fileService = serviceProvider.GetRequiredService<IFileService>();

            try
            {
                var newCaseFiles = await db.ScheduledCaseFiles.Include(s => s.CaseFile)
                .ThenInclude(s => s.Case).Where(c => c.CaseFile.Case != null).ToListAsync();
                var cases = newCaseFiles.Select(s => s.CaseFile.Case).Distinct();
                foreach (var c in cases)
                {
                    if(c.CaseUrl == null)
                        c.CaseUrl = $@"Cases/Case-{Guid.NewGuid()}";
                    await fileService.CreateDirectoryAsync(c.CaseUrl);
                }
                foreach (var item in newCaseFiles)
                {
                    if (item.CaseFile.FileURL == null)
                        item.CaseFile.FileURL = $@"{Guid.NewGuid()}";
                    if (await fileService.FileExistsAsync(item.CaseFile.FileURL))
                    {
                        var newPath = $@"{item.CaseFile.Case.CaseUrl}/{item.CaseFile.FileURL}";
                        await fileService.MoveFileAsync(item.CaseFile.FileURL, newPath);
                        item.CaseFile.FileURL = newPath;
                    }

                }
                db.ScheduledCaseFiles.RemoveRange(db.ScheduledCaseFiles);
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // logging here
                logger.LogError(ex.Message, ex.StackTrace);
            }


        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return base.StopAsync(cancellationToken);
        }
    }
}
