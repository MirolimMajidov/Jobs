using Jobs.SharedModel.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.SharedModel.Repository;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading.Tasks;

namespace Service.SharedModel.Controllers
{
    [Produces("application/json")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1")]
    public abstract class BaseController<TEntity> : ControllerBase where TEntity : IEntity
    {
        private readonly IEntityRepository<TEntity> _pepository;

        public BaseController(IEntityRepository<TEntity> pepository)
        {
            _pepository = pepository;
        }

        [SwaggerOperation(Summary = "To get all the items")]
        [HttpGet]
        [SwaggerResponse(200, "Return list of found items if it's finished successfully", typeof(OkObjectResult))]
        public virtual async Task<IActionResult> Get()
        {
            var entities = await _pepository.GetEntities();
            return new OkObjectResult(entities);
        }

        [SwaggerOperation(Summary = "To get an item by Id")]
        [HttpGet("{id}")]
        [SwaggerResponse(200, "Return the found item if it's finished successfully", typeof(OkObjectResult))]
        [SwaggerResponse(404, "An item with the specified ID was not found", typeof(NotFoundResult))]
        public virtual async Task<IActionResult> Get(Guid id)
        {
            var entity = await _pepository.GetEntityByID(id);
            if (entity == null)
                return new NotFoundResult();

            return new OkObjectResult(entity);
        }

        [SwaggerOperation(Summary = "To create a new item")]
        [HttpPost]
        [SwaggerResponse(200, "Return OK if it's added successfully", typeof(OkResult))]
        public virtual async Task<IActionResult> Post([FromBody] TEntity entity)
        {
            var createdEntity = await _pepository.InsertEntity(entity);
            return new OkObjectResult(createdEntity);
        }

        [SwaggerOperation(Summary = "To update exists an item")]
        [HttpPut]
        [SwaggerResponse(200, "Return OK if it's updated successfully", typeof(OkResult))]
        public virtual async Task<IActionResult> Put([FromBody] TEntity entity)
        {
            if (entity != null)
            {
                await _pepository.UpdateEntity(entity);
                return new OkResult();
            }
            return new NoContentResult();
        }

        [Authorize]
        [SwaggerOperation(Summary = "To delete an item")]
        [HttpDelete("{id}")]
        [SwaggerResponse(200, "Return OK if it's deleted successfully", typeof(OkResult))]
        public virtual async Task<IActionResult> Delete(Guid id)
        {
            await _pepository.DeleteEntity(id);
            return new OkResult();
        }
    }
}
