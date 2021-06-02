using Jobs.SharedModel.Helpers;
using Jobs.SharedModel.Models;
using JobService.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace JobService.DataProvider
{
    public class JobsContext : DbContext
    {
        public JobsContext(DbContextOptions<JobsContext> options) : base(options) { }

        /// <summary>   
        /// This is for getting entities by type from data base.
        /// </summary>   
        public IQueryable<T> GetEntities<T>() where T : BaseEntity => Set<T>();

        public DbSet<Job> Jobs { get; set; }

        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                   .HasMany(e => e.Jobs)
                   .WithOne(e => e.Category)
                   .HasForeignKey(e => e.CategoryId)
                   .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Job>().Property(p => p.CategoryId).IsRequired();
        }
    }
}
