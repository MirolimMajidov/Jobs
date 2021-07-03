using AutoMapper;
using Jobs.Common.Models;
using Jobs.Service.Common.Helpers;
using Jobs.Service.Common.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Jobs.Service.Common.Controllers
{
    [Produces("application/json")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public abstract class BaseController<TEntity, TEntityDTO> : ControllerBase
        where TEntity : IEntity
        where TEntityDTO : BaseEntityDTO
    {
        protected readonly IEntityRepository<TEntity> _repository;
        protected readonly IMapper _mapper;

        public BaseController(IEntityRepository<TEntity> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet]
        [SwaggerOperation(Summary = "To get all items")]
        [SwaggerResponse(200, "Return list of found items if it's finished successfully", typeof(RequestModel))]
        public virtual async Task<RequestModel> GetAll()
        {
            var entities =(await _repository.GetEntities()).Select(e => _mapper.Map<TEntityDTO>(e));
            return await RequestModel.SuccessAsync(entities);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "To get an item by Id")]
        [SwaggerResponse(200, "Return the found item if it's finished successfully", typeof(RequestModel))]
        [SwaggerResponse(404, "An item with the specified ID was not found", typeof(RequestModel))]
        public virtual async Task<RequestModel> Get(Guid id)
        {
            var entity = await _repository.GetEntityByID(id);
            if (entity == null)
                return await RequestModel.NotFoundAsync();

            return await RequestModel.SuccessAsync(_mapper.Map<TEntityDTO>(entity));
        }

        [HttpPost]
        [SwaggerOperation(Summary = "To add a new item. For this you must be authorized")]
        [SwaggerResponse(200, "Return OK if it's added successfully", typeof(RequestModel))]
        [SwaggerResponse(400, "Entity can'tbe null", typeof(RequestModel))]
        public virtual async Task<RequestModel> Create([FromBody] TEntityDTO entity)
        {
            if (entity == null)
                return await RequestModel.ErrorRequestAsync("An item can not be null");

            var createdEntity = await _repository.InsertEntity(_mapper.Map<TEntity>(entity));

            return await RequestModel.SuccessAsync(_mapper.Map<TEntityDTO>(createdEntity));
        }

        [HttpPut]
        [SwaggerOperation(Summary = "To update exists an item. For this you must be authorized")]
        [SwaggerResponse(200, "Return OK if it's updated successfully", typeof(RequestModel))]
        [SwaggerResponse(400, "Entity can'tbe null", typeof(RequestModel))]
        [SwaggerResponse(404, "An item with the specified ID was not found", typeof(RequestModel))]
        public virtual async Task<RequestModel> Update([FromBody] TEntityDTO entity)
        {
            if (entity == null)
                return await RequestModel.ErrorRequestAsync("An item can not be null");

            var oldEntity = await _repository.GetEntityByID(entity.Id);
            if (oldEntity == null)
                return await RequestModel.NotFoundAsync();

            _mapper.Map(entity, oldEntity);
            await _repository.UpdateEntity(oldEntity);

            return await RequestModel.SuccessAsync();
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "To delete an item. For this you must be authorized")]
        [SwaggerResponse(200, "Return OK if it's deleted successfully", typeof(RequestModel))]
        [SwaggerResponse(404, "An item with the specified ID was not found", typeof(RequestModel))]
        public virtual async Task<RequestModel> Delete(Guid id)
        {
            if (await _repository.GetEntityByID(id) == null)
                return await RequestModel.NotFoundAsync();

            await _repository.DeleteEntity(id);
            return await RequestModel.SuccessAsync();
        }
    }
}
