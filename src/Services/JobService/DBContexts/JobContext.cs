using Microsoft.EntityFrameworkCore;
using JobService.Models;
using System.Collections.Generic;
using Jobs.SharedModel.Helpers;
using Jobs.SharedModel.Models;
using System.Linq;

namespace JobService.DBContexts
{
    public class JobContext : DbContext
    {
        public JobContext(DbContextOptions<JobContext> options) : base(options)
        {
        }

        /// <summary>   
        /// This is for getting entities by type from data base.
        /// </summary>    
        public virtual IQueryable<T> GetEntities<T>() where T : BaseEntity => Set<T>();

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

            var categories = new List<Category>()
            {
                new Category
                {
                    Name = "Web, Mobile & Software Dev",
                    Description = "Web Development, Mobile Development, Desktop Software Developmen, QA & Testing",
                },
                new Category
                {
                    Name = "Sales & Marketing",
                    Description = "Sales & Marketing Strategy",
                },
                new Category
                {
                    Name = "Design & Writing",
                    Description = "Design, Writing, Photography & Translator",
                },
                new Category
                {
                    Name = "Engineering & Architecture",
                    Description = "Engineering & Architecture",
                }
            };

            modelBuilder.Entity<Category>().HasData(categories);

            modelBuilder.Entity<Job>().HasData(
                new Job
                {
                    Name = "Back-end developer",
                    Description = "ASP.Net Core and Xamarin developer",
                    Cost = 25,
                    Type = JobType.Hourly,
                    Duration = JobDuration.FromOneToThreeMonths,
                    CategoryId = categories[0].Id
                },
                new Job
                {
                    Name = "Angular Developer Needed",
                    Description = "We need experienced Angular developer for short term project.",
                    Cost = 25,
                    Type = JobType.Hourly,
                    Duration = JobDuration.LessThanMonth,
                    CategoryId = categories[0].Id
                },
                new Job
                {
                    Name = "Salesperson",
                    Description = "Salesperson needed",
                    Cost = 2000,
                    Type = JobType.FixedPrice,
                    Duration = JobDuration.FromOneToThreeMonths,
                    CategoryId = categories[1].Id
                },
                new Job
                {
                    Name = "Design & Photography",
                    Description = "Design & Photography needed to build mockup of mobile app",
                    Cost = 30,
                    Type = JobType.Hourly,
                    Duration = JobDuration.MoreThanSixMonths,
                    CategoryId = categories[2].Id
                }
            );
        }
    }
}
