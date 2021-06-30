using EventBus.RabbitMQ;
using Jobs.Service.Common.Repository;
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
        private readonly IEntityRepository<Payment> _repository;
        private readonly ILogger<UserNameUpdatedEventHandler> _logger;

        public UserNameUpdatedEventHandler(JobsContext context, ILogger<UserNameUpdatedEventHandler> logger)
        {
            _repository = context.PaymentRepository;
            _logger = logger;
        }

        public async Task Handle(UserNameUpdatedEvent @event)
        {
            _logger.LogInformation("Received {Event} event at {AppName}", @event.GetType().Name, Program.AppName);

            var allPayments = await _repository.GetEntities();
            var existingPayments = allPayments.Where(j => j.UserId == @event.UserId);
            if (!existingPayments.Any()) return;

            foreach (var payment in existingPayments.ToList())
            {
                payment.UserName = @event.NewName;
                await _repository.UpdateEntity(payment);
            }
        }
    }
}