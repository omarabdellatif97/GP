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
    /// <summary>
    /// this service is called every duration of time to update the casefile 
    /// to new directories based on the it's case
    /// </summary>
    public class ScheduledCaseFileWorkerService : TimedHostedService
    {
        public ScheduledCaseFileWorkerService(IServiceProvider services) : base(services)
        {
        }

        protected override TimeSpan Interval => TimeSpan.FromMinutes(10);

        protected override TimeSpan FirstRunAfter => TimeSpan.FromMinutes(10);

        protected async override Task RunJobAsync(IServiceProvider serviceProvider, CancellationToken stoppingToken)
        {

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
            }


        }
    }
}
