using EventBus.RabbitMQ;
using Jobs.Service.Common.Repository;
using JobService.Models;
using JobService.RabbitMQEvents.Events;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace JobService.RabbitMQEvents.EventHandlers
{
    public class UserNameUpdatedEventHandler : IRabbitMQEventHandler<UserNameUpdatedEvent>
    {
        private readonly IEntityRepository<Job> _pepository;
        private readonly ILogger<UserNameUpdatedEventHandler> _logger;

        public UserNameUpdatedEventHandler(IEntityRepository<Job> pepository,
            ILogger<UserNameUpdatedEventHandler> logger)
        {
            _pepository = pepository;
            _logger = logger;
        }

        public async Task Handle(UserNameUpdatedEvent @event)
        {
            _logger.LogInformation("Received {Event} event at {AppName}", @event.GetType().Name, Program.AppName);

            var existingJobs = _pepository.GetQueryableEntities().Where(j => j.CreatedByUserId == @event.UserId);
            if (!existingJobs.Any()) return;

            foreach (var job in existingJobs.ToList())
            {
                job.CreatedByUserName = @event.NewName;
                await _pepository.UpdateEntity(job, autoSave: false);
            }
            await _pepository.Save();
        }
    }
}