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
    ///<inheritdoc/>
    public class ScheduledCaseFileWorkerService : TimedHostedService
    {
        protected TimeSpan interval;
        protected CleanCaseFilesSettings settings;
        public ScheduledCaseFileWorkerService(IServiceProvider services) : base(services)
        {


            settings = services.GetRequiredService<CleanCaseFilesSettings>();

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
            if (!settings.EnableService)
            {
                this.Dispose();
                return;
            }

            var db = serviceProvider.GetRequiredService<CaseContext>();
            var fileService = serviceProvider.GetRequiredService<IFileService>();

            try
            {
                // get all case files that exists in the db, without caseId, for a 
                // time more that specified in the settings (MaxCaseFileHours)
                var date = DateTime.Now.AddHours(settings.MaxCaseFilesHours * -1);
                var todeleteCaseFiles = await db.CaseFiles.Where(file => file.PublishDate < date && file.CaseId == null).ToListAsync();

                foreach (var item in todeleteCaseFiles)
                {
                    if (await fileService.FileExistsAsync(item.FileURL))
                        await fileService.DeleteFileAsync(item.FileURL);
                }

                db.CaseFiles.RemoveRange(todeleteCaseFiles);
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
