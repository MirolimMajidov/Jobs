using AccountService.Models;
using Jobs.SharedModel.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace AccountService.DataProvider
{
    public class JobsContext : DbContext
    {
        public JobsContext(DbContextOptions<JobsContext> options) : base(options) { }

        /// <summary>   
        /// This is for getting entities by type from data base.
        /// </summary>   
        public IQueryable<T> GetEntities<T>() where T : BaseEntity => Set<T>();

        public DbSet<User> Users { get; set; }
    }
}
