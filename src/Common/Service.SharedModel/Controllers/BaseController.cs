using Jobs.SharedModel.Models;
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
        public virtual async Task<IActionResult> Get()
        {
            var entities = await _pepository.GetEntities();
            return new OkObjectResult(entities);
        }

        [SwaggerOperation(Summary = "To get an item by Id")]
        [HttpGet("{id}")]
        public virtual async Task<IActionResult> Get(Guid id)
        {
            var entity = await _pepository.GetEntityByID(id);
            return new OkObjectResult(entity);
        }

        [SwaggerOperation(Summary = "To create a new item")]
        [HttpPost]
        public virtual async Task<IActionResult> Post([FromBody] TEntity entity)
        {
            await _pepository.InsertEntity(entity);
            return CreatedAtAction(nameof(Get), new { id = entity.Id }, entity);
        }

        [SwaggerOperation(Summary = "To update exists an item")]
        [HttpPut]
        public virtual async Task<IActionResult> Put([FromBody] TEntity entity)
        {
            if (entity != null)
            {
                await _pepository.UpdateEntity(entity);
                return new OkResult();
            }
            return new NoContentResult();
        }

        [SwaggerOperation(Summary = "To delete an item")]
        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> Delete(Guid id)
        {
            await _pepository.DeleteEntity(id);
            return new OkResult();
        }
    }
}
