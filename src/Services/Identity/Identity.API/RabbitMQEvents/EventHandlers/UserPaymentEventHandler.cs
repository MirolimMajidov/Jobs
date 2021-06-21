﻿using EventBus.RabbitMQ;
using IdentityService.Models;
using IdentityService.RabbitMQEvents.Events;
using Jobs.Service.Common.Repository;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace IdentityService.RabbitMQEvents.EventHandlers
{
    public class UserPaymentEventHandler : IRabbitMQEventHandler<UserPaymentEvent>
    {
        private readonly IEntityRepository<User> _pepository;
        private readonly ILogger<UserPaymentEventHandler> _logger;

        public UserPaymentEventHandler(IEntityRepository<User> repository, ILogger<UserPaymentEventHandler> logger)
        {
            _pepository = repository;
            _logger = logger;
        }

        public async Task Handle(UserPaymentEvent @event)
        {
            _logger.LogInformation("Received {Event} event at {AppName}", @event.GetType().Name, Program.AppName);

            var user = await _pepository.GetEntityByID(@event.UserId);
            if (user == null) return;

            user.Balance += @event.NewBalance;
            await _pepository.UpdateEntity(user);
        }
    }
}