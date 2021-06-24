using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class CaseContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Case> Cases { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<Step> Steps { get; set; }
        public DbSet<CaseFile> CaseFiles { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public CaseContext(DbContextOptions<CaseContext> options) : base(options) { }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Case>().Property<DateTime>("PublishDate").HasDefaultValueSql("getdate()");
            builder.Entity<CaseFile>().Property<DateTime>("PublishDate").HasDefaultValueSql("getdate()");
            base.OnModelCreating(builder);
        }

    }
}
