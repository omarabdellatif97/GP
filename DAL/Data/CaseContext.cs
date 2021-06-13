using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class CaseContext:DbContext
    {
        public DbSet<Case> Cases { get; set; }
        public DbSet<Step> Steps { get; set; }
        public DbSet<CaseFile> CaseFiles { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public CaseContext(DbContextOptions<CaseContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Case>().OwnsMany(
            //p => p.Tags, a =>
            //{
            //    a.WithOwner().HasForeignKey("CaseId");
            //    a.Property<int>("Id");
            //    a.HasKey("Id");
            //});
            base.OnModelCreating(modelBuilder);
        }
    }
}
