using EventBus.RabbitMQ;
using IdentityService.Models;
using IdentityService.RabbitMQEvents.Events;
using Jobs.Service.Common;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace IdentityService.RabbitMQEvents.EventHandlers
{
    public class UserTransactionEventHandler : IRabbitMQEventHandler<UserTransactionEvent>
    {
        private readonly IEntityQueryableRepository<User> _repository;
        private readonly ILogger<UserTransactionEventHandler> _logger;

        public UserTransactionEventHandler(IEntityQueryableRepository<User> repository, ILogger<UserTransactionEventHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task Handle(UserTransactionEvent @event)
        {
            _logger.LogInformation("Received {Event} event at {AppName}", @event.GetType().Name, Program.AppName);

            var user = await _repository.GetEntityByID(@event.UserId);
            if (user == null) return;

            user.Balance -= @event.Amount;
            await _repository.UpdateEntity(user);
        }
    }
}