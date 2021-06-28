using DAL.Models;
using GP_API.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GP_API
{
    public class ScheduledCaseFileWorkerService : TimedHostedService
    {
        public ScheduledCaseFileWorkerService(IServiceProvider services) : base(services)
        {
        }

        protected override TimeSpan Interval => TimeSpan.FromHours(4);

        protected override TimeSpan FirstRunAfter => Interval;

        protected async override Task RunJobAsync(IServiceProvider serviceProvider, CancellationToken stoppingToken)
        {

            var db = serviceProvider.GetRequiredService<CaseContext>();
            var fileService = serviceProvider.GetRequiredService<IFileService>();



            var newCaseFiles = db.ScheduledCaseFiles.Include(s => s.CaseFile)
                .ThenInclude(s=> s.Case).AsAsyncEnumerable();
            await foreach (var item in newCaseFiles)
            {
                if (stoppingToken.IsCancellationRequested)
                    break;
                if (await fileService.FileExistsAsync(item.CaseFile.FileURL))
                {
                    var newPath = $@"{item.CaseFile.Case.CaseUrl}/{item.CaseFile.FileURL}";
                    await fileService.MoveFileAsync(item.CaseFile.FileURL, newPath);
                    item.CaseFile.FileURL = newPath;
                    await db.SaveChangesAsync();
                }

            }


        }
    }
}
