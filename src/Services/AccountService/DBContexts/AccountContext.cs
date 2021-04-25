using AccountService.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace AccountService.DBContexts
{
    public class AccountContext : DbContext
    {
        public AccountContext(DbContextOptions<AccountContext> options) : base(options) { }

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
