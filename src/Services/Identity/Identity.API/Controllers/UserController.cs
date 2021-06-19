using IdentityService.Models;
using Microsoft.AspNetCore.Mvc;
using Jobs.Service.Common.Controllers;
using Jobs.Service.Common.Helpers;
using Jobs.Service.Common.Repository;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using EventBus.RabbitMQ;
using IdentityService.RabbitMQEvents.Events;

namespace IdentityService.Controllers
{
    [Route("api/[controller]")]
    public class UserController : BaseController<User>
    {
        private readonly IEventBusRabbitMQ _eventBus;

        public UserController(IEntityRepository<User> pepository, IEventBusRabbitMQ eventBus) : base(pepository)
        {
            _eventBus = eventBus;
        }

        [SwaggerResponse(200, "Return OK if it's added successfully", typeof(User))]
        public override async Task<RequestModel> Post([FromBody] User entity)
        {
            entity.HashPassword = Encryptor.SH1Hash(entity.Password);

            return await base.Post(entity);
        }

        [SwaggerResponse(200, "Return OK if it's updated successfully", typeof(RequestModel))]
        public override async Task<RequestModel> Put([FromBody] User entity)
        {
            User oldUser = null;
            if (entity != null)
                oldUser = await _pepository.GetEntityByID(entity.Id);

            var result = await base.Put(entity);
            if (result.ErrorId == 0 && oldUser.Name != entity.Name)
            {
                var NnameUpdated = new UserNameUpdatedEvent() { OldName = oldUser.Name, NewName = entity.Name };
                _eventBus.Publish(NnameUpdated);
            }

            return result;
        }
    }
}

