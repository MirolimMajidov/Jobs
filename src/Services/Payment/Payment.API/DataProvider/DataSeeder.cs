using PaymentService.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using Jobs.Service.Common;

namespace PaymentService.DataProvider
{
    public static class DataSeeder
    {
        public static async Task SeedAsync(this JobsMongoContext context, ILogger logger, IEntityRepository<Transaction> transactionRepository)
        {
            try
            {
                var repository = context.PaymentRepository;
                if (!repository.GetEntities().Result.Any())
                {
                    var userName = "Admin";
                    await Task.Run(async () =>
                    {
                        var userId = Guid.NewGuid();
                        for (int i = 1; i < 11; i++)
                        {
                            var payment = new Payment
                            {
                                OrderId = i.ToString(),
                                UserId = userId,
                                UserName = userName,
                                Amount = 100 * i,
                                Sender = "MasterCard",
                                Date = DateTime.Now
                            };
                            await repository.InsertEntity(payment);
                        }
                    });
                }

                if (!transactionRepository.GetEntities().Result.Any())
                {
                    var userName = "Admin";
                    await Task.Run(async () =>
                    {
                        var userId = Guid.NewGuid();
                        for (int i = 1; i < 5; i++)
                        {
                            var payment = new Transaction
                            {
                                UserId = userId,
                                UserName = userName,
                                Amount = 5 * i,
                                Receiver = "PayPal",
                                Date = DateTime.Now
                            };
                            await transactionRepository.InsertEntity(payment, autoSave: false);
                        }
                        await transactionRepository.Save();
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
