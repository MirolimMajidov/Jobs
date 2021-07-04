using EventBus.RabbitMQ;
using Jobs.Service.Common;
using JobService.Models;
using JobService.RabbitMQEvents.Events;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace JobService.RabbitMQEvents.EventHandlers
{
    public class UserNameUpdatedEventHandler : IRabbitMQEventHandler<UserNameUpdatedEvent>
    {
        private readonly IEntityRepository<Job> _repository;
        private readonly ILogger<UserNameUpdatedEventHandler> _logger;

        public UserNameUpdatedEventHandler(IEntityRepository<Job> repository,
            ILogger<UserNameUpdatedEventHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task Handle(UserNameUpdatedEvent @event)
        {
            _logger.LogInformation("Received {Event} event at {AppName}", @event.GetType().Name, Program.AppName);

            var existingJobs = _repository.GetQueryableEntities().Where(j => j.CreatedByUserId == @event.UserId);
            if (!existingJobs.Any()) return;

            foreach (var job in existingJobs.ToList())
            {
                job.CreatedByUserName = @event.NewName;
                await _repository.UpdateEntity(job, autoSave: false);
            }
            await _repository.Save();
        }
    }
}