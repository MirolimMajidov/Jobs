using IdentityService.Models;
using Microsoft.AspNetCore.Mvc;
using Jobs.Service.Common.Controllers;
using Jobs.Service.Common.Helpers;
using Jobs.Service.Common.Repository;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using EventBus.RabbitMQ;
using IdentityService.RabbitMQEvents.Events;
using AutoMapper;

namespace IdentityService.Controllers
{
    [Route("api/[controller]")]
    public class UserController : BaseController<User, UserDTO>
    {
        private readonly IEventBusRabbitMQ _eventBus;

        public UserController(IEntityRepository<User> repository, IMapper mapper, IEventBusRabbitMQ eventBus) : base(repository, mapper)
        {
            _eventBus = eventBus;
        }

        [SwaggerResponse(200, "Return OK if it's added successfully", typeof(UserDTO))]
        [SwaggerResponse(400, "The Password field is required.", typeof(RequestModel))]
        public override async Task<RequestModel> Create([FromBody] UserDTO entity)
        {
            if (entity == null)
                return await base.Create(entity);

            if (string.IsNullOrEmpty(entity.Password))
                return await RequestModel.ErrorRequestAsync("The Password field is required.", 400);

            entity.Password = Encryptor.SH1Hash(entity.Password);

            return await base.Create(entity);
        }

        [SwaggerOperation(Summary = "To update exists user info. For this you must be authorized")]
        [SwaggerResponse(200, "Return OK if it's updated successfully", typeof(RequestModel))]
        [SwaggerResponse(400, "Entity can't be null", typeof(RequestModel))]
        [SwaggerResponse(404, "User with the specified ID was not found", typeof(RequestModel))]
        public override async Task<RequestModel> Update([FromBody] UserDTO entity)
        {
            if (entity == null)
                return await RequestModel.ErrorRequestAsync("User can not be null");

            User oldUser =  await _repository.GetEntityByID(entity.Id);

            if (oldUser == null)
                return await RequestModel.NotFoundAsync();

            var oldPassword = oldUser.HashPassword;
            var oldName = oldUser.Name;
            _mapper.Map(entity, oldUser);

            if (string.IsNullOrEmpty(entity.Password) || Encryptor.SH1Hash(entity.Password) == oldPassword)
                oldUser.HashPassword = oldPassword;
            else
                oldUser.HashPassword = Encryptor.SH1Hash(entity.Password);

            await _repository.UpdateEntity(oldUser);

            if (oldName != entity.Name)
            {
                var nameUpdated = new UserNameUpdatedEvent() { UserId = entity.Id, OldName = oldName, NewName = entity.Name };
                _eventBus.Publish(nameUpdated);
            }

            return await RequestModel.SuccessAsync();
        }
    }
}

