using AccountService.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AccountService.DataProvider
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
                        for (int i = 1; i < 11; i++)
                            await context.AddAsync(new User { Name = $"User {i}" });

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
