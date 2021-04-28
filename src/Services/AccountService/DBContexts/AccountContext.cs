using AccountService.Models;
using Jobs.SharedModel.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace AccountService.DBContexts
{
    public class AccountContext : DbContext
    {
        public AccountContext(DbContextOptions<AccountContext> options) : base(options) { }

        /// <summary>   
        /// This is for getting entities by type from data base.
        /// </summary>   
        public IQueryable<T> GetEntities<T>() where T : BaseEntity => Set<T>();

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(new List<User>() {
                new User
                {
                    Name = "User 1"
                },
                new User
                {
                    Name = "User 2"
                },
                new User
                {
                    Name = "User 3"
                }
            });
        }
    }
}
