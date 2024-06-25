using EventBus.RabbitMQ;
using Jobs.Service.Common;
using Microsoft.Extensions.Logging;
using PaymentService.DataProvider;
using PaymentService.Models;
using PaymentService.RabbitMQEvents.Events;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentService.RabbitMQEvents.EventHandlers
{
    public class UserNameUpdatedEventHandler : IRabbitMQEventHandler<UserNameUpdatedEvent>
    {
        private readonly IEntityRepository<Payment> _paymentRepository;
        private readonly IEntityRepository<Transaction> _transactionRepository;
        private readonly ILogger<UserNameUpdatedEventHandler> _logger;

        public UserNameUpdatedEventHandler(IJobsMongoContext context, IEntityRepository<Transaction> transactionRepository, ILogger<UserNameUpdatedEventHandler> logger)
        {
            _paymentRepository = context.PaymentRepository;
            _transactionRepository = transactionRepository;
            _logger = logger;
        }

        public async Task Handle(UserNameUpdatedEvent @event)
        {
            _logger.LogInformation("Received {Event} event at {AppName}", @event.GetType().Name, Program.AppName);

            var allPayments = (await _paymentRepository.GetEntities()).Where(j => j.UserId == @event.UserId);
            if (!allPayments.Any()) return;

            foreach (var payment in allPayments.ToList())
            {
                payment.UserName = @event.NewName;
                await _paymentRepository.UpdateEntity(payment);
            }

            var allTransactions = (await _transactionRepository.GetEntities()).Where(j => j.UserId == @event.UserId);
            if (!allTransactions.Any()) return;

            foreach (var transaction in allTransactions.ToList())
            {
                transaction.UserName = @event.NewName;
                await _transactionRepository.UpdateEntity(transaction, autoSave: false);
            }

            await _transactionRepository.Save();
        }
    }
}