using PaymentService.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentService.DataProvider
{
    public static class DataSeeder
    {
        public static async Task SeedAsync(this JobsContext context, ILogger logger)
        {
            try
            {
                var repository = context.PaymentHistoryRepository;
                if (!repository.GetEntities().Result.Any())
                {
                    await Task.Run(async () =>
                    {
                        var userId = Guid.NewGuid();
                        for (int i = 1; i < 11; i++)
                        {
                            var payment = new PaymentHistory
                            {
                                OrderId = i.ToString(),
                                UserId = userId,
                                Amount = 100 * i,
                                Sender = "Payoneer",
                                Date = DateTime.Now
                            };
                            await repository.InsertEntity(payment);
                        }
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
