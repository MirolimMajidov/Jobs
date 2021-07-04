using IdentityService.Models;
using Jobs.Service.Common;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.DataProvider
{
    public static class DataSeeder
    {
        public static async Task SeedAsync(this JobsContext context, ILogger logger)
        {
            try
            {
                if (!context.Users.Any())
                {
                    await Task.Run(async () =>
                    {
                        var users = new List<User>()
                        {
                            new User { Name = "SuperAdmin", Login = "superadmin@jobs.com", HashPassword = Encryptor.SH1Hash("superadmin123"), Role = UserRole.SuperAdmin },
                            new User { Name = "Admin", Login = "admin@jobs.com", HashPassword = Encryptor.SH1Hash("admin123"), Role = UserRole.Admin },
                            new User { Name = "Editor", Login = "Editor@jobs.com", HashPassword = Encryptor.SH1Hash("Editor123"), Role = UserRole.Editor },
                            new User { Name = "User", Login = "user@jobs.com", HashPassword = Encryptor.SH1Hash("user123"), Role = UserRole.Admin },
                        };
                        await context.AddRangeAsync(users);

                        await context.SaveChangesAsync();
                    });
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Exception error while entering demo data: {Message}", ex.Message);
            }
        }
    }
}
